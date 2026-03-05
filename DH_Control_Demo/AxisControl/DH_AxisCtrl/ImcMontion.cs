using csLTDMC;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;

namespace DH_Control_Demo.AxisControl.DH_AxisCtrl
{
    internal class ImcMontion : ParentAxisMontion
    {
        /// <summary>
        /// 初始化轴对象
        /// </summary>
        public ImcMontion(AxisInitConfigStruct axisConfig, ImcConfigStruct ImcConfig) : base(axisConfig)
        {
            _Config = ImcConfig;
            uint ret = ImcApi.IMC_OpenCardHandle(axisConfig.CardID, ref nCardHandle);
            if (ret != 0)
            {
                _AxisAct.AxisMachine = 0;
                _AxisAct.StatusStr = "总线无法正常建立";
            }
            double[] _setAxEquiv = { _Equiv };
            ImcApi.IMC_SetAxEquiv(nCardHandle, _AxisNum, _setAxEquiv, 1);//设定轴减速比
        }

        #region 私有字段定义
        /// <summary>
        /// 控制卡句柄
        /// </summary>
        private ulong nCardHandle = 0;

        /// <summary>
        /// 专属配置
        /// </summary>
        private readonly ImcConfigStruct _Config;

        /// <summary>
        /// 汇川卡号从0开始
        /// </summary>
        private ushort _CardID { get => (ushort)(_ParentCardId + 0); }

        /// <summary>
        /// 从站ID从0开始
        /// </summary>
        private ushort _Slave { get => (ushort)(_ParentSlave + 0); }

        /// <summary>
        /// 轴号默认
        /// </summary>
        private short _AxisNum { get => (short)(_ParentAxisNum + 0); }

        #endregion

        #region 读取总线数据
        private short[] prfMode = new short[1];
        private int[] stsGet = new int[1];
        private double[] prfPos = new double[1];
        private double[] encPos = new double[1];
        private double[] encVel = new double[1];
        private short encTor = 0;
        /// <summary>
        /// 实时读取轴状态信息的实现
        /// </summary>
        private protected override short GetAxisAct()
        {
            List<uint> result = new();
            _AxisAct.Name = _Name;
            _AxisAct.Torque = 0;
            result.Clear();
            lock (_LockAxisAct)
            {
                //【1】读取轴状态，如果出错则直接结束其它读取
                result.Add(ImcApi.IMC_GetAxPrfMode(nCardHandle, _AxisNum, prfMode, 1));
                //if (result.Last() != 0) return (short)result.Last();
                result.Add(ImcApi.IMC_GetAxSts(nCardHandle, _AxisNum, stsGet, 1));
                _AxisAct.AxisState = stsGet[0];
                /* B0-轴报警         B1-伺服使能        B2-轴忙          B3-轴到位      
                * B4-正硬极限报警    B5-负硬极限报警    B6-正软极限报警  B7-负软极限报警
                * B8-位置误差报警    B9-运动急停标志    B10-总线轴标志   
                * B11-轴异常         B12-轴警告         B13-原点信号状态   */

                //【2】获取轴规划位置
                result.Add(ImcApi.IMC_GetAxPrfPos(nCardHandle, _AxisNum, prfPos, 1));
                //result.Add(ImcApi.IMC_GetAxPrfVel(nCardHandle, _AxisNum, prfVel, 1));//轴规划速度
                //result.Add(ImcApi.IMC_GetAxPrfAcc(nCardHandle, _AxisNum, prfAcc, 1));//轴规划加速

                //【3】获取轴位置、速度、电流
                result.Add(ImcApi.IMC_GetAxEncPos(nCardHandle, _AxisNum, encPos));
                result.Add(ImcApi.IMC_GetAxEncVel(nCardHandle, _AxisNum, encVel, 1));
                result.Add(ImcApi.IMC_GetAxActTorq(nCardHandle, _AxisNum, ref encTor));

                //状态机简易映射
                if ((_AxisAct.AxisState & 1 << 10) < 1) _AxisAct.AxisMachine = 0;
                else if ((_AxisAct.AxisState & 1 << 0) >= 1) _AxisAct.AxisMachine = 6;
                else if ((_AxisAct.AxisState & 1 << 11) > 1) _AxisAct.AxisMachine = 7;
                else if ((_AxisAct.AxisState & 1 << 1) > 1) _AxisAct.AxisMachine = 4;
                else _AxisAct.AxisMachine = 3;
                //轴状态字简易映射
                if (_AxisAct.AxisMachine == 4 && (_AxisAct.AxisState & 1 << 2) > 1)
                {
                    _AxisAct.StatusStr = "运动中";
                    _AxisAct.IsStopping = 0;
                }
                else if (_AxisAct.AxisMachine == 4 && (_AxisAct.AxisState & 1 << 3) > 1)
                {
                    _AxisAct.StatusStr = "使能停止";
                    _AxisAct.IsStopping = 1;
                }
                else _AxisAct.IsStopping = 2;

                if (_AxisAct.AxisMachine == 6 || _AxisAct.AxisMachine == 7) _AxisAct.StatusStr = "故障触发";
                else if (_AxisAct.AxisMachine == 3) _AxisAct.StatusStr = "总线状态正常";
                else if (_AxisAct.AxisMachine == 0) _AxisAct.StatusStr = "总线无法正常建立";

                //取小数点前三位
                _AxisAct.Position = Math.Round(encPos[0], 3);
                _AxisAct.Velocity = Math.Round(encVel[0], 2);
                _AxisAct.Torque = (int)encTor;
                //_AxisAct.Current = Math.Round(_AxisAct.Current, 3);
            }
            return (short)result.Sum(x => x);
        }

        #endregion

        #region 轴基础操作
        override public void ErrorReset()
        {
            ImcApi.IMC_ClrAxSts(nCardHandle, _AxisNum);
        }

        override public short ServoStop(ushort stop_mode)
        {
            return (short)ImcApi.IMC_AxMoveStop(nCardHandle, _AxisNum, (short)stop_mode);
        }

        override public short ServoEnable(bool isEnable)
        {
            if (isEnable)
            {
                return (short)ImcApi.IMC_AxServoOn(nCardHandle, _AxisNum);
            }
            else return (short)ImcApi.IMC_AxServoOff(nCardHandle, _AxisNum);
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
                return ErrorMsg("汇川：调用标准回零时传入未定义的回零方法，无法执行！", 30011);//未定义回零方法，无法执行

            List<uint> result = new();
            ImcApi.THomingPara tHomingPara = new()
            {
                homeMethod = (short)homeMode,
                offset = (int)(homeOffset * _Equiv),
                highVel = (uint)(velHi * _Equiv),
                lowVel = (uint)(velLo * _Equiv),
                acc = (uint)(acc * _Equiv)
            };

            result.Add(ImcApi.IMC_StartHoming(nCardHandle, _AxisNum, ref tHomingPara));
            if (result.Last() != 0) return ErrorMsg("汇川：调用标准回零时回零启动失败！", result.Last());
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
            List<uint> result = new();

            //SDO写入的方式将、堵转电流、堵转时间进行写入
            result.Add((uint)SetSdo(adr.HomeStallTimeAdr, adr.HomeStallTimeAdrSub, stallTime));
            result.Add((uint)SetSdo(adr.HomeStallCurrentAdr, adr.HomeStallCurrentAdrSub, stallCurrent));
            if (result.Sum(x => x) != 0)
                return ErrorMsg("汇川：调用非标回零时参数写入失败", 30011);

            ImcApi.THomingPara tHomingPara = new()
            {
                homeMethod = (short)homeMode,
                offset = (int)(homeOffset * _Equiv),
                highVel = (uint)(velHi * _Equiv),
                lowVel = (uint)(velLo * _Equiv),
                acc = (uint)(acc * _Equiv)
            };
            result.Add(ImcApi.IMC_StartHoming(nCardHandle, _AxisNum, ref tHomingPara));
            if (result.Last() != 0) return ErrorMsg("汇川：调用非标回零时回零启动失败！", result.Last());
            return 0;
        }

        /// <summary>
        /// 通过程序算法控制伺服外部非标回零
        /// 雷赛卡可通过SDO方式实现调用伺服内部非标回零，故暂不实现此方法
        /// </summary>
        /// <returns></returns>
        override private protected short ServoGoHome_NonGenericServoExternal() => ErrorMsg("汇川：暂未实现外部非标回零！", 30011);

        #endregion

        #region 运动控制
        override public short ServoMoveAbs(double positionAbs, double vel, double acc, double dec)
        {
            List<uint> result = new();

            result.Add(ImcApi.IMC_SetSingleAxMvPara(nCardHandle, _AxisNum, vel, acc, dec));
            if (result.Last() != 0) return ErrorMsg("汇川：绝对定位时写入运动参数异常！", result.Last());

            result.Add(ImcApi.IMC_StartPtpMove(nCardHandle, _AxisNum, positionAbs));
            if (result.Last() != 0) return ErrorMsg("汇川：绝对定位启动异常！代码：{0}", result.Last());
            return 0;
        }

        override public short ServoMoveRel(double positionRel, double vel, double acc, double dec)
        {
            List<uint> result = new();

            result.Add(ImcApi.IMC_SetSingleAxMvPara(nCardHandle, _AxisNum, vel, acc, dec));
            if (result.Last() != 0) return ErrorMsg("汇川：相对定位时写入运动参数异常！", result.Last());

            result.Add(ImcApi.IMC_StartPtpMove(nCardHandle, _AxisNum, positionRel, 1)) ;
            if (result.Last() != 0) return ErrorMsg("汇川：相对定位启动异常！代码：{0}", result.Last());
            return 0;
        }
        #endregion

        #region 软着陆控制

        #endregion

        #region 小工具

        override public short SetSdo<T>(ushort address, ushort subAddress, T data)
        {
            //通过数据类型，确定数据大小
            ushort dataSize = (ushort)(Marshal.SizeOf(data.GetType()) * 8);
            int dataVar = Convert.ToInt32(data);

            uint abortCode = 0;
            uint result = ImcApi.IMC_SetEcatSdo(nCardHandle, (short)_Slave, address, subAddress, BitConverter.GetBytes(dataVar), (uint)(dataSize / 8), ref abortCode);
            if (result != 0) return ErrorMsg("汇川：写入SDO异常，代码{0}", result);

            result =(uint)GetSdo(address, subAddress, dataSize, out int dataReadBack);
            if (result != 0) return ErrorMsg("汇川：SDO写入回读异常，代码{0}", result);
            else if (dataReadBack == dataVar) return 0;
            else return ErrorMsg("汇川：写入SDO后回读不匹配，代码{0}", 30016);
        }

        override public short GetSdo(ushort address, ushort subAddress, ushort bitsCont, out int data)
        {
            byte[] value = new byte[4];
            uint actSize = 0;
            uint abortCode = 0;

            uint result = ImcApi.IMC_GetEcatSdo(nCardHandle, (short)_Slave, address, subAddress, value, (uint)(bitsCont / 8), ref actSize, ref abortCode);
            if (result != 0)
            {
                data = 0;
                return ErrorMsg("汇川：读取SDO失败，代码{0}", result, value);
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
    /// 汇川卡私有变量
    /// 主用于兼容不同板卡间共性参数传递
    /// </summary>
    public struct ImcConfigStruct
    {
    }
}
