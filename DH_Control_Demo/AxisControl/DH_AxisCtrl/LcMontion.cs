using csLTDMC;
using lctdevice;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using static lctdevice.ecat_motion;

namespace DH_Control_Demo.AxisControl.DH_AxisCtrl
{
    internal class LcMontion : ParentAxisMontion
    {
        public LcMontion(AxisInitConfigStruct axisInitConfig, LcConfigStruct config) : base(axisInitConfig)
        {
            _Config = config;
            M_SetAxisBand(_AxisNum, (uint)(0.1 * _Equiv), 10, _CardID);//设定默认Inp参数，0.1unit and 10ms
        }

        #region 私有字段定义
        /// <summary>
        /// 凌臣卡号从0开始，保持默认
        /// </summary>
        private short _CardID { get => (short)_ParentCardId; }

        /// <summary>
        /// 凌臣从站ID从1开始，需要加1
        /// </summary>
        private ushort _Slave { get => (ushort)(_ParentSlave + 1); }

        /// <summary>
        /// 凌臣轴号从1开始，需要加1
        /// </summary>
        private short _AxisNum { get => (short)(_ParentAxisNum+1); }

        /// <summary>
        /// 专属配置
        /// </summary>
        private readonly LcConfigStruct _Config;
        #endregion

        #region 读取总线数据

        /// <summary>
        /// 实时读取轴状态信息的实现
        /// </summary>
        private protected override short GetAxisAct()
        {
            List<short> result = new();
            _AxisAct.Name = _Name;
            _AxisAct.Torque = 0;

            short emgFlag = 0, torAct = 0;

            result.Clear();
            lock (_LockAxisAct)
            {
                result.Add(M_GetEmg(ref emgFlag, _CardID));//轴卡EMG信号状态,1:急停中（保持信号），0:表示急停动作未触发。
                result.Add(M_GetEncPos(_AxisNum, out double encPos, 1, _CardID));//获取编码器位置

                result.Add(M_GetEncVel(_AxisNum, out double encVel, 1, _CardID));//获取编码器速度

                result.Add(M_GetSts(_AxisNum, out _AxisAct.AxisState, 1, _CardID));//获取轴状态)

                result.Add(M_ReadActualTorque(_AxisNum, ref torAct, 1, _CardID));//获取当前力矩，必须在PDO中配置0x6077

                //ecat_motion.M_ReadActualPosition(currentAxisID, out DriverActualPos, 1, card);//获取驱动器内部实际位置
                //ecat_motion.M_GetTargetPos(currentAxisID, ref TargetPos, card);//获取轴目标位置
                //ecat_motion.M_GetCmd(currentAxisID, out CmdPos, 1, card);//获取轴规划位置
                //ecat_motion.M_GetCmdVel(currentAxisID, out CmdVel, 1, card);//获取轴规划速度

                //读取轴状态机
                //读取错误代码

                //状态字解析--按bit位解析
                // 1-伺服报警        5-正极限        6-负极限
                // 9-使能ON         10-运动中       11-到位
                //16-回零错误       17-回零完成     18-目标到达
                //20-原点开关       24-掉线

                //状态机简易映射
                if ((_AxisAct.AxisState & 1 << 1) > 1) _AxisAct.AxisMachine = 6;
                else if ((_AxisAct.AxisState & 1 << 9) > 1) _AxisAct.AxisMachine = 4;
                else if ((_AxisAct.AxisState & 1 << 24) > 1) _AxisAct.AxisMachine = 0;
                else _AxisAct.AxisMachine = 3;

                //轴状态字简易映射
                if (_AxisAct.AxisMachine == 4 && (_AxisAct.AxisState & 1 << 10) > 1)
                {
                    _AxisAct.StatusStr = "运动中";
                    _AxisAct.IsStopping = 0;
                }
                else if (_AxisAct.AxisMachine == 4 && (_AxisAct.AxisState & 1 << 11) > 1)
                {
                    _AxisAct.StatusStr = "使能停止";
                    _AxisAct.IsStopping = 1;
                }
                else _AxisAct.IsStopping = 2;

                if (_AxisAct.AxisMachine == 6 || _AxisAct.AxisMachine == 7) _AxisAct.StatusStr = "故障触发";
                else if (_AxisAct.AxisMachine == 3) _AxisAct.StatusStr = "总线状态正常";
                else if (_AxisAct.AxisMachine == 0) _AxisAct.StatusStr = "总线无法正常建立";


                //取小数点前三位
                _AxisAct.Position = Math.Round(encPos / _Equiv, 3);
                _AxisAct.Velocity = Math.Round(encVel / _Equiv, 2);
                _AxisAct.Torque = torAct;
                //_AxisAct.Current = Math.Round(_AxisAct.Current, 3);
            }
            return (short)result.Sum(x => x);
        }
        #endregion

        #region 轴基础操作
        override public void ErrorReset()
        {
            M_ClrEmg(_CardID);//清除急停状态
            M_ClrSts(_AxisNum, 1, _CardID);//清除故障
        }

        override public short ServoEnable(bool isEnable)
        {
            if (isEnable) return M_Servo_On(_AxisNum, _CardID);//使能
            else return M_Servo_Off(_AxisNum, _CardID);//失能
        }

        override public short ServoStop(ushort stop_mode)
        {
            ServoModesChangeCsp();
            return M_StopSingleAxis(_AxisNum, stop_mode, _CardID);
        }

        /// <summary>
        /// 检查当前模式，切换为CSP
        /// </summary>
        private short ServoModesChangeCsp()
        {
            //检测当前如果处于回零模式中，需要先取消回零
            short actMode = 0;
            M_EcatGetOperationMode(_AxisNum, ref actMode, _CardID);
            if (actMode == 6)
            {
                try
                {
                    M_HomeCancelSingleAxis(_AxisNum, _CardID);//取消回零
                }
                catch { }
                Thread.Sleep(50);//必要延时
            }
            else if (actMode == 8 || actMode == 10) return 0;//如果当前非CSP模式，进行切换。
            for (int i = 0; i < 10; i++)
            {
                if (M_SetHomingMode(_AxisNum, 8, _CardID) == 0) return 0;
                Thread.Sleep(500);
            }
            //10次切换均无法成功
            return ErrorMsg("凌臣：模式变更出错！",30020);
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
                return ErrorMsg("凌臣：调用标准回零时传入未定义的回零方法，无法执行！",30011);
            List<short> result = new();
            uint velHiUint = (uint)(velHi * _Equiv);
            uint velLoUint = (uint)(velLo * _Equiv);
            uint accUint = (uint)(acc * _Equiv);

            result.Add(M_SetHomingPrm(_AxisNum, homeMode, (int)homeOffset, velHiUint, velLoUint, accUint, 0, _CardID));//设置回零参数
            if (result.Last() != 0)
                return ErrorMsg("凌臣：调用标准回零时设定回零参数失败！", result.Last());

            result.Add(M_SetHomingMode(_AxisNum, 6, _CardID));//切换至回零模式，Mods_of 6
            Thread.Sleep(50);//必要延时
            if (result.Last() != 0) return ErrorMsg("凌臣：调用标准回零时切换回零模式失败！", result.Last());

            result.Add(M_HomingStart(_AxisNum, _CardID));//启动回零
            if (result.Last() != 0) return ErrorMsg("凌臣：调用标准回零时回零启动失败！", result.Last());
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
            uint velHiUint = (uint)(velHi * _Equiv);
            uint velLoUint = (uint)(velLo * _Equiv);
            uint accUint = (uint)(acc * _Equiv);

            //常规方式传入回零参数，模式缺省为34
            result.Add(M_SetHomingPrm(_AxisNum, 34, (int)homeOffset, velHiUint, velLoUint, accUint, 0, _CardID));//设置回零参数

            //SDO写入的方式将正确回零模式、堵转电流、堵转时间进行写入
            result.Add(SetSdo(adr.HomeMethodAdr, 0x00, homeMode));
            result.Add(SetSdo(adr.HomeStallTimeAdr, adr.HomeStallTimeAdrSub, stallTime));
            result.Add(SetSdo(adr.HomeStallCurrentAdr, adr.HomeStallCurrentAdrSub, stallCurrent));

            if (result.Sum(x => x) != 0) return ErrorMsg("凌臣：非标回零时必要参数写入失败！", 30011);
            result.Add(M_SetHomingMode(_AxisNum, 6, _CardID));//切换至回零模式，Mods_of 6
            Thread.Sleep(50);//必要延时
            if (result.Last() != 0) return ErrorMsg("凌臣：非标回零时切换模式失败！", result.Last());

            result.Add(M_HomingStart(_AxisNum, _CardID));//启动回零
            if (result.Last() != 0) return ErrorMsg("凌臣：非标回零时启动回零失败！", result.Last());

            return 0;
        }

        /// <summary>
        /// 通过程序算法控制伺服外部非标回零
        /// 雷赛卡可通过SDO方式实现调用伺服内部非标回零，故暂不实现此方法
        /// </summary>
        /// <returns></returns>
        override private protected short ServoGoHome_NonGenericServoExternal() => ErrorMsg("凌臣：暂未实现外部非标回零！", 30011);

        #endregion


        #region 运动控制
        override public short ServoMoveAbs(double positionAbs, double vel, double acc, double dec)
        {
            List<short> result = new();
            result.Add(ServoModesChangeCsp());//确认轴当前为CSP模式
            //设定基本运动参数，单位需转换为凌臣所支持的p/s
            CmdPrm pPrm = new()
            {
                acc = acc * _Equiv,
                dec = dec * _Equiv,
                sTime = 0
            };
            result.Add(M_SetMove(_AxisNum, ref pPrm, _CardID));

            int posInt = (int)(positionAbs * _Equiv);
            double velDou = vel * _Equiv;
            if (result.Sum(x => x) != 0)
                return ErrorMsg("凌臣：运动参数设置异常，无法启动！", 30011);
            result.Add(M_AbsMove(_AxisNum, posInt, velDou, _CardID));
            if (result.Last() != 0)
                return ErrorMsg("凌臣：启动绝对定位异常！故障：{0}", result.Last());
            return 0;
        }

        override public short ServoMoveRel(double positionRel, double vel, double acc, double dec)
        {
            List<short> result = new();
            result.Add(ServoModesChangeCsp());

            //设定基本运动参数
            CmdPrm pPrm = new()
            {
                acc = acc * _Equiv,
                dec = dec * _Equiv,
                sTime = 0
            };
            result.Add(M_SetMove(_AxisNum, ref pPrm, _CardID));

            int posInt = (int)(positionRel * _Equiv);
            double velDou = vel * _Equiv;
            if (result.Sum(x => x) != 0)
                return ErrorMsg("凌臣：运动参数设置异常，无法启动！", 30011);
            result.Add(M_RelMove(_AxisNum, posInt, velDou, _CardID));
            if (result.Last() != 0)
                return ErrorMsg("凌臣：启动相对定位异常！故障：{0}", result.Last());
            return 0;
        }
        #endregion

        #region 小工具
        override public short SetSdo<T>(ushort address, ushort subAddress, T data)
        {
            ////通过数据类型，确定数据大小
            short dataSize = (short)Marshal.SizeOf(data.GetType());

            uint dataVar = Convert.ToUInt32(data);
            short result = M_EcatSDOWrite((short)_Slave, (short)address, (short)subAddress, dataVar, dataSize, _CardID);
            if (result != 0) return ErrorMsg("凌臣：写入SDO异常，代码{0}", result);

            //回读SDO确认数据写入
            result = GetSdo(address, subAddress, (ushort)(dataSize * 8), out int dataReadBack);
            if (result != 0) return ErrorMsg("凌臣：SDO写入回读异常，代码{0}", result);
            else if (dataReadBack == dataVar) return 0;
            else return ErrorMsg("凌臣：写入SDO后回读不匹配，代码{0}", 30016);
        }

        override public short GetSdo(ushort address, ushort subAddress, ushort bitsCont, out int data)
        {
            //凌臣对SDO操作指令的数据长度以字节长度计算，为了保证程序统一，对数据进行转换
            short result = M_EcatSDORead((short)_Slave, (short)address, (short)subAddress, (short)(bitsCont / 8), out uint value, 1, _CardID);
            if (result != 0)
            {
                data = 0;
                return ErrorMsg("凌臣：读取SDO失败，代码{0}", result, value) ;
            }
            else
            {
                data = (int)value;
                return 0;
            }
        }

        #endregion

    }
    /// <summary>
    /// 凌臣卡私有变量
    /// 主用于兼容不同板卡间共性参数传递
    /// </summary>
    public struct LcConfigStruct
    {
        public ushort ushort1;
    }

}
