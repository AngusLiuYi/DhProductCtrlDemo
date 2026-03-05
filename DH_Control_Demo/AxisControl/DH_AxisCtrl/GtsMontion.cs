using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GTN;
using static System.Formats.Asn1.AsnWriter;

namespace DH_Control_Demo.AxisControl.DH_AxisCtrl
{
    internal class GtsMontion : ParentAxisMontion
    {
        /// <summary>
        /// 初始化轴对象
        /// </summary>
        public GtsMontion(AxisInitConfigStruct axisConfig, GtsConfigStruct gtsConfig) : base(axisConfig)
        {
            _Config = gtsConfig;
            
        }

        #region 私有字段定义

        /// <summary>
        /// 专属配置
        /// </summary>
        private readonly GtsConfigStruct _Config;

        /// <summary>
        /// 固高卡号从1开始，需要加1
        /// </summary>
        private short _CardID { get => (short)(_ParentCardId + 1); }

        /// <summary>
        /// 从站ID从0开始
        /// </summary>
        private ushort _Slave { get => (ushort)(_ParentSlave + 0); }

        /// <summary>
        /// 轴号从1开始，需要加1
        /// </summary>
        private short _AxisNum { get => (short)(_ParentAxisNum + 1); }

        #endregion

        #region 读取总线数据
        private short AxisInHome = 0;

        /// <summary>
        /// 实时读取轴状态信息的实现
        /// </summary>
        private protected override short GetAxisAct()
        {
            List<short> result = new();
            _AxisAct.Name = _Name;
            //_AxisAct.Torque = 0;
            result.Clear();
            lock (_LockAxisAct)
            {
                result.Add(mc.GTN_GetAxisEncVel(_CardID, _AxisNum, out double velValue, 1, out _));
                _AxisAct.Velocity = velValue * 1000 / _Equiv;//读取轴速度(pluse/ms => unit/s)
                result.Add(mc.GTN_GetAxisEncPos(_CardID, _AxisNum, out double posValue, 1, out _));
                _AxisAct.Position = posValue / _Equiv;//读取编码器位置值

                result.Add(mc.GTN_GetSts(_CardID, _AxisNum, out int stsInt, 1, out _));

                mc.GTN_IsEcatReady(_CardID, out short ecatReady);//查询EtherCAT通讯状态,0：通讯未完全建立；1：通讯完全建立。

                if ((stsInt & (1 << 1)) > 1)//bit1==1，驱动器报警
                {
                    _AxisAct.AxisMachine = 6;
                    AxisInHome = 0;
                }
                else if ((stsInt & (1 << 9)) > 1)//bit9==1，电机使能
                    _AxisAct.AxisMachine = 4;
                else if (ecatReady == 1)
                    _AxisAct.AxisMachine = 3;
                else
                    _AxisAct.AxisMachine = 0;

                //轴状态字简易映射
                if (_AxisAct.AxisMachine == 4 && (stsInt & (1 << 10)) > 1)
                {
                    _AxisAct.StatusStr = "运动中";
                    _AxisAct.IsStopping = 0;
                }
                else if ((_AxisAct.AxisMachine == 3 || _AxisAct.AxisMachine == 4) && (stsInt & (1 << 10)) <= 1)
                {
                    _AxisAct.StatusStr = "使能停止";
                    _AxisAct.IsStopping = 1;
                }
                else _AxisAct.IsStopping = 2;


                if (_AxisAct.AxisMachine == 6 || _AxisAct.AxisMachine == 7) _AxisAct.StatusStr = "故障触发";
                else if (_AxisAct.AxisMachine == 0 || ecatReady == 0) _AxisAct.StatusStr = "总线无法正常建立";
                ReadCurrent();

                //result.Add(LTDMC.nmc_get_errcode(_CardID, 2, ref _AxisAct.ErrorCode));//读取错误代码

                //取小数点前三位
                _AxisAct.Position = Math.Round(_AxisAct.Position, 3);
                _AxisAct.Velocity = Math.Round(_AxisAct.Velocity, 2);
                //_AxisAct.Current = Math.Round(_AxisAct.Current, 3);
            }
            return (short)result.Sum(x => x);

        }

        private bool ReadCurrentIsSusses;
        private void ReadCurrent()
        {
            //读取SDO比PDO或拓展PDO读取更耗时
            //为提高标准协议中PDO刷新速度
            //SDO的读取，依靠异步功能获取

            //设定标志位，如果标志位没有置位时，开启线程读取
            //读取完成后标志位复位
            //如果程序执行时标志位已被置位
            //则上次读取尚未完成，直接跳过此步骤，
            if (!ReadCurrentIsSusses)
            {
                ReadCurrentIsSusses = true;
                byte[] value = new byte[4];
                Task.Factory.StartNew(new Action(() =>
                {
                    //读取电流反馈
                    mc.GTN_EcatSDOUpload(_CardID, _Slave, 0x6077, 0, out value[0], 32, out _, out _);
                    _AxisAct.Torque = BitConverter.ToInt32(value, 0);
                    ReadCurrentIsSusses = false;
                }));
            }
        }

        #endregion

        #region 轴基础操作
        override public void ErrorReset() => mc.GTN_ClrSts(_CardID, _AxisNum, 1);

        override public short ServoStop(ushort stop_mode) => mc.GTN_Stop(_CardID, 1 << (_AxisNum - 1), stop_mode);

        override public short ServoEnable(bool isEnable)
        {
            if (isEnable)
                return mc.GTN_AxisOn(_CardID, _AxisNum);
            else
            {
                AxisInHome = 0;
                return mc.GTN_AxisOff(_CardID, _AxisNum);
            }
        }
        #endregion

        #region 回零操作

        /// <summary>
        /// 调用伺服内部常规回零方法回零
        /// </summary>
        /// <param name="homeMode">回零方式(null)</param>
        /// <param name="velLo">回零低速(unit/s)</param>
        /// <param name="velHi">回零高速(unit/s)</param>
        /// <param name="acc">回零加速度(unit/s^2)</param>
        /// <param name="dec">回零减速度(unit/s^2)</param>
        /// <param name="homeOffset">回零偏置(unit)</param>
        /// <returns></returns>
        override private protected short ServoGoHome_Generic(byte homeMode, double velLo, double velHi, double acc, double dec, double homeOffset)
        {
            if (homeMode < 0 || homeMode > 37)
                return ErrorMsg("固高：调用标准回零时传入未定义的回零方法，无法执行！", 30011);//未定义回零方法，无法执行
            List<short> result = new();
            Stopwatch sw = new();
            sw.Start();

            result.Add(mc.GTN_SetHomingMode(_CardID, _AxisNum, 6));//切换轴回零模式
            result.Add(mc.GTN_SetEcatHomingPrm(_CardID, _AxisNum, (short)homeMode, velHi * _Equiv, velLo * _Equiv, acc * _Equiv, 0, 0));//传入回零参数
            if (result.Sum(x => x) != 0)
                return ErrorMsg("固高：切换回零模式异常", result.Last());

            result.Add(mc.GTN_StartEcatHoming(_CardID, _AxisNum));//开始回零
            if (result[^1] != 0)
                return ErrorMsg("固高：启动回零异常", result.Last());

            ushort goHomeResult;
            do
            {
                mc.GTN_GetEcatHomingStatus(_CardID, _AxisNum, out goHomeResult);//查询EtherCAT轴的回零状态
                if (sw.ElapsedMilliseconds >= 10000)
                    return ErrorMsg("固高：回零超过固定时间未返回完成状态", goHomeResult);
                Application.DoEvents();
                Thread.Sleep(0);
            } while (goHomeResult != 3);

            result.Add(mc.GTN_SetHomingMode(_CardID, _AxisNum, 8));//切换轴模式为CSP
            if (result[^1] != 0)
                return ErrorMsg("固高：切换模式到CSP时异常", result.Last());

            result.Add(mc.GTN_ZeroPos(_CardID, _AxisNum, 1));//清除当前位置与规划位置
            if (result[^1] != 0)
                return ErrorMsg("固高：清除零漂时异常", result.Last());
            sw.Stop();
            AxisInHome = 1;
            return 0;
        }

        /// <summary>
        /// 通过其他方法调用伺服内部非标回零
        /// </summary>
        /// <param name="homeMode">回零方式(null)</param>
        /// <param name="velLo">回零低速(unit/s)</param>
        /// <param name="velHi">回零高速(unit/s)</param>
        /// <param name="acc">回零加速度(unit/s^2)</param>
        /// <param name="dec">回零减速度(unit/s^2)</param>
        /// <param name="homeOffset">回零偏置(unit)</param>
        /// <param name="backDistance">非标回零时反向运动距离(unit)</param>
        /// <param name="stallTime">非标回零时堵转时间(ms)</param>
        /// <param name="stallCurrent">非标回零时堵转电流(千分比)</param>
        /// <returns></returns>
        override private protected short ServoGoHome_NonGenericServoInternal(byte homeMode, double velLo, double velHi, double acc, double dec, double homeOffset, short backDistance, short stallTime, short stallCurrent)
        {
            List<short> result = new();
            Stopwatch sw = new();
            sw.Start();

            result.Add(mc.GTN_SetHomingMode(_CardID, _AxisNum, 6));//切换轴回零模式
            result.Add(mc.GTN_SetEcatHomingPrm(_CardID, _AxisNum, (short)homeMode, velHi * _Equiv, velLo * _Equiv, acc * _Equiv, 0, 0));//传入回零参数
            if (result.Sum(x => x) != 0)
                return ErrorMsg("固高：切换回零模式异常", result.Last());

            //SDO写入的方式将正确回零模式、堵转电流、堵转时间进行写入
            result.Add(SetSdo(adr.HomeMethodAdr, 0x00, homeMode));
            result.Add(SetSdo(adr.HomeStallTimeAdr, adr.HomeStallTimeAdrSub, stallTime));
            result.Add(SetSdo(adr.HomeStallCurrentAdr, adr.HomeStallCurrentAdrSub, stallCurrent));

            if (result.Sum(x => x) != 0)
                return ErrorMsg("固高：调用非标回零时参数写入失败", 30011);

            result.Add(mc.GTN_StartEcatHoming(_CardID, _AxisNum));//开始回零
            if (result[^1] != 0)
                return ErrorMsg("固高：启动回零异常", result.Last());

            ushort goHomeResult;
            do
            {
                mc.GTN_GetEcatHomingStatus(_CardID, _AxisNum, out goHomeResult);//查询EtherCAT轴的回零状态
                if (sw.ElapsedMilliseconds >= 10000)
                    return ErrorMsg("固高：回零超过固定时间未返回完成状态", goHomeResult);
                Application.DoEvents();
                Thread.Sleep(0);
            } while (goHomeResult != 3);

            result.Add(mc.GTN_SetHomingMode(_CardID, _AxisNum, 8));//切换轴模式为CSP
            if (result[^1] != 0)
                return ErrorMsg("固高：切换模式到CSP时异常", result.Last());
            result.Add(mc.GTN_ZeroPos(_CardID, _AxisNum, 1));//清除当前位置与规划位置
            if (result[^1] != 0)
                return ErrorMsg("固高：清除零漂时异常", result.Last());
            sw.Stop();
            AxisInHome = 1;
            return 0;

        }

        /// <summary>
        /// 通过程序算法控制伺服外部非标回零
        /// 固高卡可通过SDO方式实现调用伺服内部非标回零，故暂不实现此方法
        /// </summary>
        /// <returns></returns>
        override private protected short ServoGoHome_NonGenericServoExternal() => ErrorMsg("固高：暂未实现外部非标回零！", 30011);

        #endregion

        #region 运动控制
        override public short ServoMoveAbs(double positionAbs, double vel, double acc, double dec)
        {
            if (AxisInHome != 1)
                return ErrorMsg("固高:未回零下进行定位操作", 01);

            List<short> result = new();
            mc.TMoveAbsolutePrm moveAbsolutePrm = new mc.TMoveAbsolutePrm
            {
                pos = (int)Math.Round(positionAbs * _Equiv),
                vel = vel * _Equiv / 1000,
                acc = acc * _Equiv / 1000000,
                dec = dec * _Equiv / 1000000,
                percent = 10,
            };
            result.Add(mc.GTN_MoveAbsolute(_CardID, _AxisNum, ref moveAbsolutePrm));
            if (result.Last() != 0)
                return ErrorMsg("固高：绝对定位启动异常！代码：{0}", result.Last());
            return 0;
        }

        override public short ServoMoveRel(double positionRel, double vel, double acc, double dec)
        {
            List<short> result = new();
            result.Add(mc.GTN_GetPrfPos(_CardID, _AxisNum, out double prfPos, 1, out _));//读取轴规划位置
            mc.TMoveAbsolutePrm moveAbsolutePrm = new mc.TMoveAbsolutePrm
            {
                pos = (int)Math.Round(prfPos + positionRel * _Equiv),
                vel = vel * _Equiv / 1000,
                acc = acc * _Equiv / 1000000,
                dec = dec * _Equiv / 1000000,
                percent = 10,
            };
            result.Add(mc.GTN_MoveAbsolute(_CardID, _AxisNum, ref moveAbsolutePrm));
            if (result.Last() != 0)
                return ErrorMsg("雷赛：相对定位启动异常！代码：{0}", result.Last());
            return 0;
        }
        #endregion

        #region 软着陆控制

        #endregion

        #region 小工具

        override public short SetSdo<T>(ushort address, ushort subAddress, T data)
        {
            //通过数据类型，确定数据大小
            //按BIT计，即short型返回16，byte型返回8
            ushort dataSize = (ushort)(Marshal.SizeOf(data.GetType()) * 8);
            int dataVar = Convert.ToInt32(data);
            List<short> result = new();

            result.Add(mc.GTN_EcatSDODownload(_CardID, _Slave, address, (byte)subAddress, ref BitConverter.GetBytes(dataVar)[0], (uint)(dataSize / 8), out uint a));
            if (result[^1] != 0 || a != 0) 
                return ErrorMsg("固高：写入SDO异常，程序代码:{1}，固高返回代码：{2}", result.Last() + a, result.Last(), a);

            result.Add(GetSdo(address, subAddress, dataSize, out int dataReadBack));
            if (result[^1] != 0) return ErrorMsg("固高：SDO写入回读异常，代码{0}", result);
            else if (dataReadBack == dataVar) return 0;
            else return ErrorMsg("固高：写入SDO后回读不匹配，代码{0}", 30016);
        }

        override public short GetSdo(ushort address, ushort subAddress, ushort bitsCont, out int data)
        {
            byte[] value = new byte[4];
            List<short> result = new();
            result.Add(mc.GTN_EcatSDOUpload(_CardID, _Slave, address, (byte)subAddress, out value[0], (uint)(bitsCont / 8), out _, out uint y));
            if (result[^1] != 0)
            {
                data = 0;
                return ErrorMsg("固高：读取SDO异常，程序代码:{1}，固高返回代码：{2}", result.Last() + y, result.Last(), y);
            }
            else
            {
                data = BitConverter.ToInt32(value, 0);
                return 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// 固高卡私有变量
    /// 主用于兼容不同板卡间共性参数传递
    /// </summary>
    public struct GtsConfigStruct
    {
        /// <summary>
        /// 拓展PDO中读取当前力矩的Pdo地址
        /// </summary>
        public short torqueTxPdoAddress;
    }
}
