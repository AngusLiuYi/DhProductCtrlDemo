using AngusTools.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DH_Control_Demo.AxisControl.DH_AxisCtrl
{
    public abstract class ParentAxisMontion
    {
        public ParentAxisMontion(AxisInitConfigStruct axisInitConfig) 
        {
            _ParentCardId = axisInitConfig.CardID; 
            _ParentSlave = axisInitConfig.SlaveID; 
            _ParentAxisNum = axisInitConfig.AxisID;
            _Name = axisInitConfig.Name;
            _ServoDriveType = axisInitConfig.ServoDriveType;
            _Equiv = axisInitConfig.Pluse / axisInitConfig.Unit;
            adr = new DriveCtrlAdr(_ServoDriveType);
            CurrentConfig = axisInitConfig;
        }

        #region 字段定义
        /// <summary>
        /// 当前运行的初始化配置信息
        /// 仅可在子类中赋值，外部可访问
        /// </summary>
        public AxisInitConfigStruct CurrentConfig { get; protected set; }

        /// <summary>
        /// 卡号，传入卡号从0开始
        /// </summary>
        private protected int _ParentCardId { get; }

        /// <summary>
        /// 从站ID从0开始
        /// </summary>
        private protected int _ParentSlave { get; }

        /// <summary>
        /// 轴号，默认从0开始
        /// </summary>
        private protected int _ParentAxisNum { get; }

        /// <summary>
        /// 轴名称
        /// </summary>
        private protected string _Name { get; }

        /// <summary>
        /// 减速比
        /// </summary>
        private protected double _Equiv { get; }

        /// <summary>
        /// 驱动器类别
        /// </summary>
        private protected ServoDriveTypeEnum _ServoDriveType { get; }

        private protected DriveCtrlAdr adr;

        /// <summary>
        /// 控制异常时通过委托传递ex
        /// </summary>
        public Action<string> OnError { get; set; }

        /// <summary>
        /// 集合错误代码
        /// </summary>
        /// <param name="msg">异常信息，文字描述</param>
        /// <param name="obj">错误代码，[0]定义错误代码</param>
        /// <returns>错误代码</returns>
        private protected short ErrorMsg(string msg, params object[] obj)
        {
            string str = string.Format(msg, obj);//组合错误代码
            OnError?.Invoke(str);//委托传递错误信息

            //LogManager.Error(str);//根据需要拓展日志直接写入

            //汇川板卡返回的错误代码通常超过short型MAX，为了整合故进行舍弃
            return (short)Convert.ToInt64(obj[0]);//返回错误代码，用于程序判断非0
        }
        #endregion

        #region 读取总线数据

        /// <summary>
        /// 内部数据-轴实时状态
        /// </summary>
        private protected AxisActStruct _AxisAct = new();

        /// <summary>
        /// 安全锁
        /// </summary>
        private protected object _LockAxisAct = new();

        /// <summary>
        /// 轴数据正常读取事件时触发
        /// </summary>
        public event Action<AxisActStruct>? AxisStateActionEvent;

        /// <summary>
        /// 轴实时状态反馈
        /// </summary>
        public AxisActStruct AxisAct { get => _AxisAct; }

        /// <summary>
        /// 实时读取轴状态信息的实现
        /// </summary>
        private protected abstract short GetAxisAct();

        /// <summary>
        /// 多线程读取轴状态
        /// </summary>
        public void AxisActTaskMethod()
        {
            if (GetAxisAct() == 0)
                AxisStateActionEvent?.Invoke(_AxisAct);
        }
        #endregion

        #region 轴基础操作

        /// <summary>
        /// 轴停止
        /// </summary>
        /// <param name="stop_mode">0：正常停止，1：急停</param>
        /// <returns></returns>
        public abstract short ServoStop(ushort stop_mode);

        /// <summary>
        /// 轴使能
        /// </summary>
        /// <param name="isEnable">0:失能，1:使能</param>
        /// <returns></returns>
        public abstract short ServoEnable(bool isEnable);

        /// <summary>
        /// 清除轴错误
        /// </summary>
        public abstract void ErrorReset();
        #endregion

        #region 回零操作

        /// <summary>
        /// 伺服回零
        /// </summary>
        /// <param name="homeType">回零执行目标(Enum)</param>
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
        public short ServoGoHome(ServoGoHomeTypeEnum homeType, byte homeMode, double velLo, double velHi, double acc, double dec, double homeOffset, short backDistance, short stallTime, short stallCurrent)
        {
            return homeType switch
            {
                ServoGoHomeTypeEnum.GenericServoInternal => ServoGoHome_Generic(homeMode, velLo, velHi, acc, dec, homeOffset),
                ServoGoHomeTypeEnum.NonGenericServoInternal => ServoGoHome_NonGenericServoInternal(homeMode, velLo, velHi, acc, dec, homeOffset, backDistance, stallTime, stallCurrent),
                ServoGoHomeTypeEnum.NonGenericServoExternal => ServoGoHome_NonGenericServoExternal(),
                _ => ErrorMsg("回零时模式为选择或选择非预定义类型", 30011)
            } ;
        }

        /// <summary>
        /// 等待回零完成，异步等待
        /// </summary>
        /// <param name="homeTimeOut">等待超时时间(ms)</param>
        /// <param name="isAsync">是否开启异步</param>
        /// <returns></returns>
        public async Task<short> WaitHomeDone(int homeTimeOut,bool isAsync)
        {
            if (!isAsync)
                return WaitHomeDone(homeTimeOut);
            Thread.Sleep(500);
            DateTime dt = DateTime.Now;
            return await Task<short>.Factory.StartNew(() =>
            {
                do
                {
                    if (_AxisAct.IsStopping == 1)
                        if (Math.Abs(_AxisAct.Velocity) < 1 && Math.Abs(_AxisAct.Position - 0) < 0.1) return 0;
                    Application.DoEvents();
                    Thread.Sleep(0);
                } while (!((DateTime.Now - dt).TotalMilliseconds > homeTimeOut));
                return ErrorMsg("轴在规定时间内未回零完成！",30010);
            });
        }

        /// <summary>
        /// 等待回零完成，阻塞等待
        /// </summary>
        /// <param name="homeTimeOut">等待超时时间(ms)</param>
        /// <returns></returns>
        public short WaitHomeDone(int homeTimeOut)
        {
            Thread.Sleep(500);
            DateTime dt = DateTime.Now;
            do
            {
                if (_AxisAct.IsStopping == 1)
                    if (Math.Abs(_AxisAct.Velocity) < 0.1 && Math.Abs(_AxisAct.Position - 0) < 0.1) return 0;
                Application.DoEvents();
                Thread.Sleep(0);
            } while (!((DateTime.Now - dt).TotalMilliseconds > homeTimeOut));
            return ErrorMsg("轴在规定时间内未回零完成！", 30010);
        }

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
        private protected abstract short ServoGoHome_Generic(byte homeMode, double velLo, double velHi, double acc, double dec, double homeOffset);

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
        private protected abstract short ServoGoHome_NonGenericServoInternal(byte homeMode, double velLo, double velHi, double acc, double dec, double homeOffset, short backDistance, short stallTime, short stallCurrent);
        
        /// <summary>
        /// 通过程序算法控制伺服外部非标回零
        /// 针对运控卡无法调用伺服内部非标回零时使用，暂不实现此方法
        /// </summary>
        /// <returns></returns>
        private protected abstract short ServoGoHome_NonGenericServoExternal();

        #endregion

        #region 运动控制
        /// <summary>
        /// 轴控，绝对定位。
        /// 只做触发，不等待完成信号
        /// </summary>
        /// <param name="positionRel">目标位置(unit)</param>
        /// <param name="vel">目标速度(unit/s)</param>
        /// <param name="acc">加速度(unit/s^2)</param>
        /// <param name="dec">减速度(unit/s^2)</param>
        /// <returns></returns>
        public abstract short ServoMoveAbs(double positionAbs, double vel, double acc, double dec);

        /// <summary>
        /// 轴控，相对定位。
        /// 只做触发，不等待完成信号
        /// </summary>
        /// <param name="positionRel">目标位置(unit)</param>
        /// <param name="vel">目标速度(unit/s)</param>
        /// <param name="acc">加速度(unit/s^2)</param>
        /// <param name="dec">减速度(unit/s^2)</param>
        /// <returns></returns>
        public abstract short ServoMoveRel(double positionRel, double vel, double acc, double dec);

        /// <summary>
        /// 等待运动完成，异步等待
        /// 位置到达？扭矩到达？
        /// </summary>
        /// <param name="position">目标位置(unit)</param>
        /// <param name="torque">目标扭矩(千分比)</param>
        /// <param name="moveTimeout">定位超时时间(ms)</param>
        /// <param name="peInpos">位置到达误差阈值(unit)</param>
        /// <param name="peIntorque">力矩到达误差阈值(千分比)</param>
        /// <param name="isAsync">是否开启异步</param>
        /// <returns>=1：位置到达，2：扭矩到达</returns>
        public async Task<short> WaitMoveDone(double position, int torque, double peInpos, double peIntorque, uint moveTimeout,bool isAsync)
        {
            if (!isAsync)
                return WaitMoveDone(position, torque, peInpos, peIntorque, moveTimeout);
            //--待引入力矩稳定时间，目前默认20ms
            DateTime dt = DateTime.Now;
            Thread.Sleep(20);
            return await Task<short>.Factory.StartNew(() =>
            {
                do
                {
                    if (_AxisAct.IsStopping == 1)
                        if (Math.Abs(_AxisAct.Position - position) < peInpos) return 1;// && Math.Abs(_AxisAct.Velocity) < 0.1
                    if (Delay(() =>
                    {
                        Thread.Sleep(1);
                        return Math.Abs(_AxisAct.Torque - torque) <= peIntorque && Math.Abs(_AxisAct.Velocity) < 1;
                    }, 20))
                        return 2;
                    Application.DoEvents();
                    Thread.Sleep(0);
                } while (!((DateTime.Now - dt).TotalMilliseconds > moveTimeout));
                return ErrorMsg("轴在规定时间未达到目标位置或目标扭矩！",30010);
            });
        }

        /// <summary>
        /// 等待运动完成，阻塞等待
        /// 位置到达？扭矩到达？
        /// </summary>
        /// <param name="position">目标位置(unit)</param>
        /// <param name="torque">目标扭矩(千分比)</param>
        /// <param name="moveTimeout">定位超时时间(ms)</param>
        /// <param name="peInpos">位置到达误差阈值(unit)</param>
        /// <param name="peIntorque">力矩到达误差阈值(千分比)</param>
        /// <returns>1：位置到达，2：扭矩到达</returns>
        public short WaitMoveDone(double position, int torque, double peInpos, double peIntorque, uint moveTimeout)
        {
            //--待引入力矩稳定时间
            DateTime dt = DateTime.Now;
            Thread.Sleep(20);
            do
            {
                if (_AxisAct.IsStopping == 1)
                    if (Math.Abs(_AxisAct.Position - position) < peInpos) 
                        return 1;// && Math.Abs(_AxisAct.Velocity) < 0.1
                if (Delay(() =>
                {
                    Thread.Sleep(1);
                    return Math.Abs(_AxisAct.Torque - torque) <= peIntorque && Math.Abs(_AxisAct.Velocity) < 0.1;
                },20))
                    return 2;
                Application.DoEvents();
                Thread.Sleep(0);
            } while (!((DateTime.Now - dt).TotalMilliseconds > moveTimeout));

            return ErrorMsg("轴在规定时间未达到目标位置或目标扭矩！", 30010);
        }

        #endregion

        #region 软着陆控制

        /// <summary>
        /// 力矩限制设定
        /// </summary>
        /// <param name="TorLimit">电流限定(千分比)</param>
        /// <returns>写入成功为true</returns>
        public short TorqueLimitSet(double TorLimit)
        {
            if (TorLimit < 0 || TorLimit > 3000)
                return ErrorMsg("设定力矩不在 0<TorqueSet<3000 内", 30012);
            return SetSdo(adr.TorLimitAdr, 0x00, (short)(TorLimit * adr.TorLimitScale));
        }

        /// <summary>
        /// 外部控制电机两段速软着陆
        /// 流程如下：
        /// 1、快速接近产品
        /// 2、限制电流输出
        /// 3、慢速接触产品
        /// 4、[可选]自定义函数处理
        /// 5、保压预定时间
        /// 6、快速返回待机位置
        /// 7、解除电流限制
        /// </summary>
        /// <param name="ppPosition">PP段位置(产品上方1mm)(unit)</param>
        /// <param name="ptPosition">PT段位置(过压产品1mm)(unit)</param>
        /// <param name="pbPosition">PB段位置(返回待机位置)(unit)</param>
        /// <param name="ppVel">高速运动运动速度(unit/s)</param>
        /// <param name="ptVel">低速运动运动速度(unit/s)</param>
        /// <param name="pbVel">保压完成后返回起始位置的速度(unit/s)</param>
        /// <param name="acc">加速度(unit/s^2)</param>
        /// <param name="dec">减速度(unit/s^2)</param>
        /// <param name="torqueLimit">限制力矩(‰)</param>
        /// <param name="installTime">保压时间(ms)</param>
        /// <param name="action">自定义函数处理(Action)</param>
        /// <returns>错误代码</returns>
        public async Task<short> SoftLand_ServoExternal(double ppPosition, double ptPosition, double pbPosition, double ppVel, double ptVel, double pbVel, double acc, double dec, int torqueLimit, int installTime, Func<short>? func)
        {
            DateTime dt = DateTime.Now;
            List<short> result = new();
            int step = 0;
            double InstallPos = 0;
            return await Task<short>.Factory.StartNew(() =>
            {
                while (true)
                {
                    switch (step)
                    {
                        case 0://设定转矩限制为最大
                            result.Add(TorqueLimitSet(3000));
                            if (result.Last() != 0) return ErrorMsg("软着陆：力矩设定出错！错误代码：{0}，步序：{1}。", result.Last(), step);
                            else step = 10;
                            break;

                        case 10://快速段，快速接近产品上方0.5-1mm位置
                            result.Add(ServoMoveAbs(ppPosition, ppVel, acc, dec));
                            if (result.Last() != 0) return ErrorMsg("软着陆：快速段触发定位指令出错！错误代码：{0}，步序：{1}。", result.Last(), step);

                            if (Wait(() => Math.Abs(ppPosition - _AxisAct.Position) < 0.1, 3000) == true) step = 20;
                            else return ErrorMsg("软着陆：快速段等待到位出错！错误代码：{0}，步序：{1}。",30010,step);
                            break;

                        case 20://设定转矩限制为预定转矩
                            result.Add(TorqueLimitSet(torqueLimit));
                            if (result.Last() != 0) return ErrorMsg("软着陆：力矩限制出错！错误代码：{0}，步序：{1}。", result.Last(), step);
                            else step = 30;
                            break;

                        case 30://慢速段，低速接触产品
                            result.Add(ServoMoveAbs(ptPosition, ptVel, acc, dec));
                            if (result.Last() != 0) return ErrorMsg("软着陆：慢速段触发定位指令出错！错误代码：{0}，步序：{1}。", result.Last(), step);

                            result.Add(WaitMoveDone(ptPosition, torqueLimit, 0.1, 20, 3000));
                            if (result.Last() == 2) step = 35;
                            else return ErrorMsg("软着陆：快速段等待力矩到达出错！错误代码：{0}，步序：{1}。", 30010, step);
                            break;

                        case 35://调用回调函数。用户自定义处理，如（保存接触产品位置、关闭真空、联动其他机构等）
                            if (func != null) result.Add(func.Invoke());
                            if (func == null || result.Last() == 0) step = 40;
                            else return ErrorMsg("软着陆：用户自定义函数执行异常，错误代码：{0}，步序：{1}。", result.Last(), step);
                            break;

                        case 40://按预定时间保压
                            Thread.Sleep(installTime);
                            InstallPos = _AxisAct.Position;
                            step = 50;
                            break;

                        case 50://返回段，快速返回到起始位置
                            result.Add(ServoMoveAbs(pbPosition, pbVel, acc, dec));
                            if (result.Last() != 0) return ErrorMsg("软着陆：返回段触发定位指令出错！错误代码：{0}，步序：{1}。", result.Last(), step);

                            //检测电机开始运动，离开接触面后将电流限制放开
                            //直接放开电流限制后，会有一个突变点，有过压伤产品的风险
                            //故改用此方法
                            if (Wait(() => _AxisAct.Position <= InstallPos - 0.5, 500) == false) 
                                return ErrorMsg("软着陆：返回段等待轴响应出错！错误代码：{0}，步序：{1}。", 30010, step);

                            result.Add(TorqueLimitSet(3000));
                            if (result.Last() != 0) return ErrorMsg("软着陆：力矩解除限制出错！错误代码：{0}，步序：{1}。", result.Last(), step);

                            else
                            {
                                result.Add(WaitMoveDone(pbPosition, 3000, 0.2, 20, 3000));
                                if (result.Last() == 1) step = 100;
                                else return ErrorMsg("软着陆：慢速段返回到位出错！错误代码：{0}，步序：{1}。", 30010, step);
                            }
                            break;

                        case 100://保压完成，软着陆结束。返回
                            return 0;

                        default:
                            return ErrorMsg("软着陆：步序出错！错误代码：{0}，步序：{1}。", -1, step);
                    }
                    Thread.Sleep(0);
                }
            });
        }

        public Task<short> SoftLand_ServoInternal(double ppPosition, double ptPosition, double pbPosition, double ppVel, double ptVel, double pbVel, double acc, double dec, int torqueLimit, int installTime, Action? action = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 小工具

        /// <summary>
        /// 错误代码查询
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <returns>释义</returns>
        public string ErrCodeSearch(short err)
        {
            string msg = err switch
            {
                30010 => "常规控制，运动超时",
                30011 => "参数异常，未定义参数",
                30012 => "参数异常，力矩限制统一为千分比",
                30013 => "参数异常，写入SDO类型未实现",
                30014 => "参数异常，轴数量与配置数量不符",
                30015 => "",
                30016 => "SDO写入失败",
                30017 => "",
                30018 => "",
                30019 => "未在实例中实现的方法被调用",
                30020 => "凌臣，切换CSP模式失败",
                30021 => "",
                30022 => "",
                30023 => "",
                30024 => "",
                30025 => "",
                30026 => "",
                30027 => "",
                30028 => "",
                30029 => "",
                _ => "未找到记录",
            };
            return msg;
        }

        /// <summary>
        /// 向目标从站写SDO
        /// </summary>
        /// <typeparam name="T">字节、短整型、整形</typeparam>
        /// <param name="address">SDO地址</param>
        /// <param name="subAddress">SDO字地址</param>
        /// <param name="data">要写入的数据</param>
        /// <returns></returns>
        public abstract short SetSdo<T>(ushort address, ushort subAddress, T data);

        /// <summary>
        /// 从目标从站读取SDO数据
        /// </summary>
        /// <param name="address">SDO地址</param>
        /// <param name="subAddress">SDO子地址</param>
        /// <param name="bitsCont">SDO长度，bit单位</param>
        /// <param name="data">取得的数据</param>
        /// <returns></returns>
        public abstract short GetSdo(ushort address, ushort subAddress, ushort bitsCont, out int data);

        /// <summary>
        /// 轴402状态机
        /// </summary>
        /// <param name="axisState">代码</param>
        /// <param name="language">语言--0:英文，1:中文</param>
        /// <returns>释义</returns>
        public string AxisStateMachine(ushort axisState, ushort language)
        {
            if (language == 0)
                return axisState switch
                {
                    0 => "NOT_READY_SWITCH_ON",// "轴处于未启动状态";
                    1 => "SWITCH_ON_DISABLE",//"轴处于启动禁止状态";
                    2 => "READY_TO_SWITCH_ON",//"轴处于准备启动状态";
                    3 => "SWITCH_ON",//"轴处于启动状态";
                    4 => "OP_ENABLE",//"轴处于操作使能状态";
                    5 => "QUICK_STOP",//"轴处于停止状态";
                    6 => "FAULT_ACTIVE",//"轴处于错误触发状态";
                    7 => "FAULT",//"轴处于错误状态";
                    _ => "NOT_NOT",
                };
            else
                return axisState switch
                {
                    0 => "轴处于未启动状态",
                    1 => "轴处于启动禁止状态",
                    2 => "轴处于准备启动状态",
                    3 => "轴处于启动状态",
                    4 => "轴处于操作使能状态",
                    5 => "轴处于停止状态",
                    6 => "轴处于错误触发状态",
                    7 => "轴处于错误状态",
                    _ => "读取错误",
                };
        }

        /// <summary>
        /// 如果enable为真并保持times时间后返回真，否则返回假
        /// 阻塞等待
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool Delay(Func<bool> enable, double times)
        {
            DateTime datatimeStart = DateTime.Now;
            while (enable.Invoke())
            {
                if ((DateTime.Now - datatimeStart).TotalMilliseconds >= times)
                    return true;
                Application.DoEvents();
                Thread.Sleep(1);
            }
            return false;
        }

        /// <summary>
        /// 如果enable为真并保持times时间后返回真，否则返回假
        /// 异步等待
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="times"></param>
        /// <param name="isAsync">是否启用异步</param>
        /// <returns></returns>
        public async Task<bool> Delay(Func<bool> enable, double times,bool isAsync)
        {
            if(!isAsync)
                return Delay(enable, times);
            DateTime datatimeStart = DateTime.Now;
            return await Task<bool>.Factory.StartNew(() =>
            {
                while (enable.Invoke())
                {
                    if ((DateTime.Now - datatimeStart).TotalMilliseconds >= times)
                        return true;
                    Application.DoEvents();
                    Thread.Sleep(1);
                }
                return false;
            });
        }

        /// <summary>
        /// 等待flag信号在times时间内变为真则返回真，否则返回假
        /// 阻塞等待
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool Wait(Func<bool> flag, double times)
        {
            DateTime datatimeStart = DateTime.Now;
            while (true)
            {
                if (flag.Invoke())
                    return true;
                if ((DateTime.Now - datatimeStart).TotalMilliseconds >= times && times != 0)
                    return false;
                Application.DoEvents();
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 等待flag信号在times时间内变为真则返回真，否则返回假
        /// 异步等待
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="times"></param>
        /// <param name="isAsync">是否启用异步</param>
        /// <returns></returns>
        public async Task<bool> Wait(Func<bool> flag, double times,bool isAsync)
        {
            if (!isAsync)
                return Wait(flag, times);
            DateTime datatimeStart = DateTime.Now;
            return await Task<bool>.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (flag.Invoke())
                        return true;
                    if ((DateTime.Now - datatimeStart).TotalMilliseconds >= times && times != 0)
                        return false;
                    Application.DoEvents();
                    Thread.Sleep(1);
                }
            });
        }

        #endregion
    }
    public class DriveCtrlAdr
    {
        public DriveCtrlAdr(ServoDriveTypeEnum drive)
        {
            _Drive=drive;
        }

        /// <summary>
        /// 驱动器类型
        /// </summary>
        public readonly ServoDriveTypeEnum _Drive;

        /// <summary>
        /// 回零方式写入地址
        /// </summary>
        public ushort HomeMethodAdr { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x6098,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x6098,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x6898,
                    ServoDriveTypeEnum.行动元 => throw new NotImplementedException(),
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 回零堵转电流
        /// </summary>
       public ushort HomeStallCurrentAdr { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x2138,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x5000,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x5800,
                    ServoDriveTypeEnum.行动元 => throw new NotImplementedException(),
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 回零堵转电流子地址
        /// </summary>
        public ushort HomeStallCurrentAdrSub { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x0,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x5,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x5,
                    ServoDriveTypeEnum.行动元 => throw new NotImplementedException(),
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 回零堵转时间
        /// </summary>
        public ushort HomeStallTimeAdr { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x2137,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x5000,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x5800,
                    ServoDriveTypeEnum.行动元 => throw new NotImplementedException(),
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 回零堵转时间子地址
        /// </summary>
        public ushort HomeStallTimeAdrSub { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x0,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x6,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x6,
                    ServoDriveTypeEnum.行动元 => throw new NotImplementedException(),
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 力矩限制
        /// </summary>
        public ushort TorLimitAdr{ get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 0x60E0,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 0x5018,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 0x5818,
                    ServoDriveTypeEnum.行动元 => 0x60E0,
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }

        /// <summary>
        /// 力矩限制缩放
        /// </summary>
        public double TorLimitScale { get {
                return _Drive switch
                {
                    ServoDriveTypeEnum.ISD大寰驱动器 => 10,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴一 => 1,
                    ServoDriveTypeEnum.SAC_N2双轴控制器_轴二 => 1,
                    ServoDriveTypeEnum.行动元 => 10,
                    ServoDriveTypeEnum.高创 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            } }


    }
    #region 结构体

    /// <summary>
    /// 轴实时状态反馈
    /// </summary>
    public struct AxisActStruct
    {
        /// <summary>
        /// 轴名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 402状态机
        /// </summary>
        public ushort AxisMachine;

        /// <summary>
        /// 轴状态-预留
        /// </summary>
        public int AxisState;

        /// <summary>
        /// 当前位置(unit)
        /// </summary>
        public double Position;

        /// <summary>
        /// 当前速度(unit/s)
        /// </summary>
        public double Velocity;

        //public double Current;

        /// <summary>
        /// 当前力矩(千分比)
        /// </summary>
        public int Torque;

        /// <summary>
        /// 错误代码
        /// </summary>
        public ushort ErrorCode;

        /// <summary>
        /// 轴当前状态释义
        /// </summary>
        public string StatusStr;

        /// <summary>
        /// 轴是否在运动中
        /// 0:运动中，1:停止，2:未获取状态
        /// </summary>
        public short IsStopping;
    }

    /// <summary>
    /// 轴初始化配置文件
    /// </summary>
    public struct AxisInitConfigStruct
    {
        /// <summary>
        /// 轴名称，用于显示与区分
        /// </summary>
        public string Name;

        /// <summary>
        /// 卡号
        /// 统一从0开始
        /// </summary>
        public short CardID;

        /// <summary>
        /// 从站号，组态生成
        /// </summary>
        public short SlaveID;

        /// <summary>
        /// 轴号
        /// 统一从0开始
        /// </summary>
        public short AxisID;

        /// <summary>
        /// 驱动器类别，不同驱动器部分地址不一致
        /// </summary>
        public ServoDriveTypeEnum ServoDriveType;

        /// <summary>
        /// 减速比-脉冲单位
        /// </summary>
        public double Pluse;

        /// <summary>
        /// 减速比-用户单位
        /// </summary>
        public double Unit;
    }

    /// <summary>
    /// 驱动器类型
    /// </summary>
    public enum ServoDriveTypeEnum
    {
        ISD大寰驱动器,
        SAC_N2双轴控制器_轴一,
        SAC_N2双轴控制器_轴二,
        行动元,
        高创
    }

    /// <summary>
    /// 调用回零类别
    /// </summary>
    public enum ServoGoHomeTypeEnum
    {
        /// <summary>
        /// 常规通用回零方法
        /// 1--37
        /// </summary>
        GenericServoInternal = 0,

        /// <summary>
        /// 调用伺服内部音圈回零方法
        /// -1 -- -4
        /// </summary>
        NonGenericServoInternal,

        /// <summary>
        /// 上位机控制轴运动实现音圈回零方法
        /// 等同-1 -- -4
        /// </summary>
        NonGenericServoExternal,
    }

    #endregion

}
