using csLTDMC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DH_Control_Demo.AxisControl.DH_AxisCtrl
{
    internal class LtdmcMontion : ParentAxisMontion
    {
        /// <summary>
        /// 初始化轴对象
        /// </summary>
        public LtdmcMontion(AxisInitConfigStruct axisConfig, LtdmcConfigStruct ltdmcConfig) : base(axisConfig)
        {
            _Config = ltdmcConfig;
            LTDMC.dmc_set_equiv(_CardID, _AxisNum, _Equiv);//设定轴减速比
            CurrentConfig = new AxisInitConfigStruct()
            {
                CardID = (short)_CardID,
                SlaveID=(short)_Slave,
                AxisID=(short)_AxisNum,
            };
        }

        #region 私有字段定义

        /// <summary>
        /// 专属配置
        /// </summary>
        private readonly LtdmcConfigStruct _Config;

        /// <summary>
        /// 雷赛卡号从1开始，需要加1
        /// </summary>
        private ushort _CardID { get => (ushort)(_ParentCardId + 1); }

        /// <summary>
        /// 从站ID从1001开始
        /// </summary>
        private ushort _Slave { get => (ushort)(_ParentSlave + 1001); }

        /// <summary>
        /// 轴号默认
        /// </summary>
        private ushort _AxisNum { get => (ushort)_ParentAxisNum; }

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
            short checkDone = 0;
            result.Clear();
            lock (_LockAxisAct)
            {
                result.Add(LTDMC.nmc_get_axis_state_machine(_CardID, _AxisNum, ref _AxisAct.AxisMachine));//读取轴状态机
                result.Add(LTDMC.nmc_get_errcode(_CardID, 2, ref _AxisAct.ErrorCode));//读取错误代码
                result.Add(LTDMC.dmc_read_current_speed_unit(_CardID, _AxisNum, ref _AxisAct.Velocity)); // 读取轴当前速度
                result.Add(LTDMC.dmc_get_encoder_unit(_CardID, _AxisNum, ref _AxisAct.Position));//读取编码器位置值
                checkDone = LTDMC.dmc_check_done(_CardID, _AxisNum);//读取运动状态，0：运动中，1：停止
                                                                    //LTDMC.dmc_get_position_unit(_CardID, _Axis, ref dunitPos); //读取指令位置值
                                                                    //int Pos = LTDMC.dmc_get_position(_CardID, _Axis);//读取指定轴的脉冲值

                //通过拓展PDO的方式读取当前力矩值，需要提前在过程数据及主站配置中组态0x6077的值
                //雷赛专属方式，其他控制卡不支持
                if (_Config.torqueTxPdoAddress >= 0)
                    result.Add(LTDMC.nmc_read_txpdo_extra(_CardID, 2, (ushort)_Config.torqueTxPdoAddress, 1, ref _AxisAct.Torque));

                if (_AxisAct.AxisMachine == 4 && checkDone == 0)
                {
                    _AxisAct.StatusStr = "运动中";
                    _AxisAct.IsStopping = 0;
                }
                else if (_AxisAct.AxisMachine == 4 && checkDone == 1)
                {
                    _AxisAct.StatusStr = "使能停止";
                    _AxisAct.IsStopping = 1;
                }
                else _AxisAct.IsStopping = 2;

                if (_AxisAct.AxisMachine == 6 || _AxisAct.AxisMachine == 7) _AxisAct.StatusStr = "故障触发";
                else if (_AxisAct.AxisMachine == 3) _AxisAct.StatusStr = "总线状态正常";
                else if (_AxisAct.AxisMachine == 0) _AxisAct.StatusStr = "总线无法正常建立";

                //取小数点前三位
                _AxisAct.Position = Math.Round(_AxisAct.Position, 3);
                _AxisAct.Velocity = Math.Round(_AxisAct.Velocity, 2);
                //_AxisAct.Current = Math.Round(_AxisAct.Current, 3);
            }
            return (short)result.Sum(x => x);

        }

        #endregion

        #region 轴基础操作
        override public void ErrorReset()
        {
            LTDMC.nmc_clear_axis_errcode(_CardID, _AxisNum);//清除错误代码
            LTDMC.dmc_clear_stop_reason(_CardID, _AxisNum);//清除停止信息
        }

        override public short ServoStop(ushort stop_mode)
        {
            return LTDMC.dmc_stop(_CardID, _AxisNum, stop_mode);
        }

        override public short ServoEnable(bool isEnable)
        {
            if (isEnable)
            {
                return LTDMC.nmc_set_axis_enable(_CardID, _AxisNum);
            }
            else return LTDMC.nmc_set_axis_disable(_CardID, _AxisNum);
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
                return ErrorMsg("雷赛：调用标准回零时传入未定义的回零方法，无法执行！", 30011);//未定义回零方法，无法执行
            double tacc = velLo / acc;//将 unit/s^2 转换为 s单位
            double tdec = velHi / dec;//将 unit/s^2 转换为 s单位
            List<short> result = new();
            result.Add(LTDMC.nmc_set_home_profile(_CardID, _AxisNum, homeMode, velLo, velHi, tacc, tdec, 0));//设定回零参数
            if (result.Last() != 0) return ErrorMsg("雷赛：调用标准回零时切换回零模式失败！", result.Last());

            result.Add(LTDMC.nmc_home_move(_CardID, _AxisNum));//启动回零
            if (result.Last() != 0) return ErrorMsg("雷赛：调用标准回零时回零启动失败！", result.Last());
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
            double tacc = velLo / acc;//将 unit/s^2 转换为 s单位
            double tdec = velHi / dec;//将 unit/s^2 转换为 s单位
            //常规方式传入回零参数，模式缺省为34
            result.Add(LTDMC.nmc_set_home_profile(_CardID, _AxisNum, 34, velLo, velHi, tacc, tdec, 0));

            //SDO写入的方式将正确回零模式、堵转电流、堵转时间进行写入
            result.Add(SetSdo(adr.HomeMethodAdr, 0x00, homeMode));
            result.Add(SetSdo(adr.HomeStallTimeAdr, adr.HomeStallTimeAdrSub, stallTime));
            result.Add(SetSdo(adr.HomeStallCurrentAdr, adr.HomeStallCurrentAdrSub, stallCurrent));

            if (result.Sum(x => x) != 0)
                return ErrorMsg("雷赛：调用非标回零时参数写入失败", 30011);

            result.Add(LTDMC.nmc_home_move(_CardID, _AxisNum));//常规方式启动回零
            if (result.Last() != 0)
                return ErrorMsg("雷赛：调用非标回零时启动异常！代码：{0}", result.Last());
            return 0;
        }

        /// <summary>
        /// 通过程序算法控制伺服外部非标回零
        /// 雷赛卡可通过SDO方式实现调用伺服内部非标回零，故暂不实现此方法
        /// </summary>
        /// <returns></returns>
        override private protected short ServoGoHome_NonGenericServoExternal() => ErrorMsg("雷赛：暂未实现外部非标回零！", 30011);

        #endregion

        #region 运动控制
        override public short ServoMoveAbs(double positionAbs, double vel, double acc, double dec)
        {
            List<short> result = new();
            double tacc = vel / acc;//将 unit/s^2 转换为 s单位
            double tdec = vel / dec;
            result.Add(LTDMC.dmc_set_profile_unit(_CardID, _AxisNum, 0, vel, tacc, tdec, dec));//设置速度参数
            //设置速度参数时如果轴正在运动中，会反馈错误代码：4，此时需要使用在线变速的方式进行速度更改
            //且此时无法变更S段速度参数，故不对其进行写入
            if (result.Last() == 4)
                result[^1] = LTDMC.dmc_change_speed_unit(_CardID, _AxisNum, vel, tacc);
            else
            {
                result.Add(LTDMC.dmc_set_s_profile(_CardID, _AxisNum, 0, tacc));//设置S段速度参数

                result.Add(LTDMC.dmc_set_dec_stop_time(_CardID, _AxisNum, tdec)); //设置减速停止时间
            }
            if (result.Sum(x => x) != 0)
                return ErrorMsg("雷赛：绝对定位参数写入异常！", 30011);
            result.Add(LTDMC.dmc_pmove_unit(_CardID, _AxisNum, positionAbs, 1));//绝对位置定位运动指令下发

            //如果运动指令下发时轴处于运动中，会反馈错误代码：4/1002，此时需要使用在线变位的方式进行速度更改
            if (result.Last() == 4 || result.Last() == 1002)
                result[^1] = LTDMC.dmc_update_target_position_unit(_CardID, _AxisNum, positionAbs);
            if (result.Last() != 0)
                return ErrorMsg("雷赛：绝对定位启动异常！代码：{0}", result.Last());
            return 0;
        }

        override public short ServoMoveRel(double positionRel, double vel, double acc, double dec)
        {
            List<short> result = new();
            double tacc = vel / acc;//将 unit/s^2 转换为 s单位
            double tdec = vel / dec;
            result.Add(LTDMC.dmc_set_profile_unit(_CardID, _AxisNum, 0, vel, tacc, tdec, dec));//设置速度参数
            //设置速度参数时如果轴正在运动中，会反馈错误代码：4，此时需要使用在线变速的方式进行速度更改
            //且此时无法变更S段速度参数，故不对其写入
            if (result.Last() == 4)
                result[^1] = LTDMC.dmc_change_speed_unit(_CardID, _AxisNum, vel, tacc);
            else
            {
                result.Add(LTDMC.dmc_set_s_profile(_CardID, _AxisNum, 0, tacc));//设置S段速度参数

                result.Add(LTDMC.dmc_set_dec_stop_time(_CardID, _AxisNum, tdec)); //设置减速停止时间
            }
            if (result.Sum(x => x) != 0)
                return ErrorMsg("雷赛：相对定位参数写入异常！", 30011);

                result.Add(LTDMC.dmc_pmove_unit(_CardID, _AxisNum, positionRel, 0));//相对位置定位运动指令下发

                //如果运动指令下发时轴处于运动中，会反馈错误代码：4/1002，此时需要使用在线变位的方式进行速度更改
                if (result.Last() == 4 || result.Last() == 1002)
                    result[^1]= LTDMC.dmc_update_target_position_unit(_CardID, _AxisNum, positionRel);
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
            ushort dataSize = (ushort)(Marshal.SizeOf(data.GetType()) * 8);
            int dataVar = Convert.ToInt32(data);
            short result = LTDMC.nmc_set_node_od(_CardID, 2, _Slave, address, subAddress, dataSize, dataVar);
            if (result != 0) return ErrorMsg("雷赛：写入SDO异常，代码{0}", result);

            result = GetSdo(address, subAddress, dataSize, out int dataReadBack);
            if (result != 0) return ErrorMsg("雷赛：SDO写入回读异常，代码{0}", result);
            else if (dataReadBack == dataVar) return 0;
            else return ErrorMsg("雷赛：写入SDO后回读不匹配，代码{0}", 30016);
        }

        override public short GetSdo(ushort address, ushort subAddress, ushort bitsCont, out int data)
        {
            int value = 0;
            short result = LTDMC.nmc_get_node_od(_CardID, 2, _Slave, address, subAddress, bitsCont, ref value);
            if (result != 0)
            {
                data = 0;
                return ErrorMsg("雷赛：读取SDO失败，代码{0}", result, value);
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
    /// 雷赛卡私有变量
    /// 主用于兼容不同板卡间共性参数传递
    /// </summary>
    public struct LtdmcConfigStruct
    {
        /// <summary>
        /// 拓展PDO中读取当前力矩的Pdo地址
        /// </summary>
        public short torqueTxPdoAddress;
    }
}
