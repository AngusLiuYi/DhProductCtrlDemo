using AngusTools.FileHelper;
using csLTDMC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH_Control_Demo.TestDemo
{
    internal static class TestPE
    {
        static TestPE()
        {
            ErrMsg = "未启动";
            ProcessStep = 0;
        }
        public static ProcessMachine Machine { get; private set; }

        public static string ErrMsg { get; private set; }

        public static int ProcessStep { get; private set; }

        public static short? Result { get; private set; }

        private static DataTable[] dtPoint = new DataTable[2];

        private static bool IsRun { get; set; }


        private static Task? ProcessTask;

        private static short WaitMoveDone(int axisNum)
        {
            double dunitPos = 0;
            LTDMC.dmc_set_factor_error(1, 0, 1, 10);
            LTDMC.dmc_set_factor_error(1, 1, 1, 10);
            short res = WaitCheckDone(axisNum);
            if (res != 1)
                return res;
            if (LTDMC.dmc_check_success_encoder(1, (ushort)axisNum) == 1) return 0;
            //LTDMC.dmc_get_position_unit(1, (ushort)axisNum, ref dunitPos);
            //AngusTools.LogHelper.LogManager.Info($"检出CheckDone时轴：{axisNum},指令位置：{dunitPos}");
            //AngusTools.LogHelper.LogManager.Info($"实际位置：{GVL.AxisMontions[axisNum].AxisAct.Position}");
            //AngusTools.LogHelper.LogManager.Info($"偏差：{dunitPos - GVL.AxisMontions[axisNum].AxisAct.Position}");

            //Thread.Sleep(100);
            //DateTime dt = DateTime.Now;
            //do
            //{
            //    if (LTDMC.dmc_check_success_encoder(1, (ushort)axisNum) == 1) return 0;// && Math.Abs(_AxisAct.Velocity) < 0.1
            //    Application.DoEvents();
            //    Thread.Sleep(0);
            //} while ((DateTime.Now - dt).TotalMilliseconds < 100);
            ////res = LTDMC.dmc_check_success_encoder(1, (ushort)axisNum);
            ////if (res != 1)
            //LTDMC.dmc_get_position_unit(1, (ushort)axisNum, ref dunitPos);
            //AngusTools.LogHelper.LogManager.Info($"定位超时时轴：{axisNum},指令位置：{dunitPos}");
            //AngusTools.LogHelper.LogManager.Info($"实际位置：{GVL.AxisMontions[axisNum].AxisAct.Position}");
            //AngusTools.LogHelper.LogManager.Info($"偏差：{dunitPos - GVL.AxisMontions[axisNum].AxisAct.Position}");

            return 3;
        }
        private static short WaitCheckDone(int axisNum)
        {
            DateTime dt = DateTime.Now;
            do
            {
                if (LTDMC.dmc_check_done(1, (ushort)axisNum) == 1) return 1;// && Math.Abs(_AxisAct.Velocity) < 0.1
                Application.DoEvents();
                Thread.Sleep(0);
            } while ((DateTime.Now - dt).TotalMilliseconds < 50000);
            return 2;
        }

        private static void ProcessRun()
        {
            int axisNum = 0, posNum = 0;
            while (IsRun)
            {
                switch (ProcessStep)
                {
                    case 0:
                        ErrMsg = "初始化中";
                        ProcessStep = 10; break;
                    case 10:
                        axisNum = 0;
                        Result = GVL.AxisMontions?[axisNum].ServoGoHome(AxisControl.DH_AxisCtrl.ServoGoHomeTypeEnum.NonGenericServoInternal, 253, 5, 10, 30000, 30000, 0, 0, 500, 500);
                        Result = GVL.AxisMontions?[axisNum].WaitHomeDone(30000);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}回零失败！"); break;
                    case 20:
                        axisNum = 1;
                        Result = GVL.AxisMontions?[axisNum].ServoGoHome(AxisControl.DH_AxisCtrl.ServoGoHomeTypeEnum.GenericServoInternal, 34, 20, 50, 30000, 30000, 0, 0, 500, 500);
                        Result = GVL.AxisMontions?[axisNum].WaitHomeDone(60000);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}回零失败！"); break;
                    case 30:
                        ErrMsg = "初始化完成";
                        ProcessStep = 100; break;


                    case 100:
                        ErrMsg = "自动运行中";
                        axisNum = 1;
                        posNum = 0;
                        Result = GVL.AxisMontions[axisNum].ServoMoveAbs(Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Position"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Velocity"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]));
                        Result = WaitMoveDone(axisNum);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}定位到{posNum}时定位失败！"); break;

                    case 110:
                        axisNum = 0;
                        posNum = 0;
                        Result = GVL.AxisMontions[axisNum].ServoMoveAbs(Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Position"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Velocity"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]));
                        Result = WaitMoveDone(axisNum);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}定位到{posNum}时定位失败！"); break;


                    case 120:
                        axisNum = 0;
                        posNum = 1;
                        Result = GVL.AxisMontions[axisNum].ServoMoveAbs(Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Position"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Velocity"]),
                                                                                Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]));

                        Result = GVL.AxisMontions[1].ServoMoveAbs(Convert.ToDouble(dtPoint[1].Rows[posNum]["Position"]),
                                                Convert.ToDouble(dtPoint[1].Rows[posNum]["Velocity"]),
                                                Convert.ToDouble(dtPoint[1].Rows[posNum]["Acc/Dec"]),
                                                Convert.ToDouble(dtPoint[1].Rows[posNum]["Acc/Dec"]));

                        Result = WaitMoveDone(axisNum);
                        Result = WaitMoveDone(1);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}定位到{posNum}时定位失败！"); break;

                    case 130:
                        axisNum = 1;
                        posNum = 2;
                        Result = GVL.AxisMontions[axisNum].ServoMoveAbs(Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Position"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Velocity"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]));
                        Result = WaitMoveDone(axisNum);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}定位到{posNum}时定位失败！"); break;

                    case 140:
                        axisNum = 1;
                        posNum = 3;
                        Result = GVL.AxisMontions[axisNum].ServoMoveAbs(Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Position"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Velocity"]),
                                                                            Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]),
                                                                        Convert.ToDouble(dtPoint[axisNum].Rows[posNum]["Acc/Dec"]));
                        Result = WaitMoveDone(axisNum);
                        ProcessStep = ErrorCheck(ProcessStep, $"轴{axisNum}定位到{posNum}时定位失败！"); break;

                    case 150:
                        ErrMsg = "循环完成";
                        Thread.Sleep(1000);
                        AngusTools.LogHelper.LogManager.Info($"{DateTime.Now}完成一个循环");
                        ProcessStep = 100; break;
                    default:
                        break;
                }

                Thread.Sleep(0);

            }
        }

        private static int ErrorCheck(int currentStep, string msg, int NextStep = -1)
        {
            if (Result != 0)
            {
                IsRun = false;
                Machine = ProcessMachine.Error;
                ErrMsg = $"流程步{currentStep}指令失败，{msg}";
                return currentStep;
            }
            Thread.Sleep(500);
            //ErrMsg = "运行正常";
            if (NextStep == -1)
            {
                return currentStep += 10;
            }
            return NextStep;
        }

        public static void ProcessStart()
        {
            if (ProcessTask != null)
            {
                if (IsRun || ProcessTask.Status == TaskStatus.Running) return;
            }
            dtPoint[0] = CsvHelper.CsvToDataTable($@"..\..\..\Data\PointData_轴0.csv");
            dtPoint[1] = CsvHelper.CsvToDataTable($@"..\..\..\Data\PointData_轴1.csv");
            LTDMC.dmc_set_factor_error(1, 0, 1, 20);
            LTDMC.dmc_set_factor_error(1, 1, 1, 100);
            IsRun = true;
            ErrMsg = "程序启动";
            ProcessTask = new(ProcessRun);
            ProcessTask.Start();
            Machine = ProcessMachine.Running;
        }

        public static async Task ProcessStop()
        {
            IsRun = false;
            if (ProcessTask != null)
            {
                if (ProcessTask.Status == TaskStatus.Running)
                    await ProcessTask;
            }
            ProcessStep = 0;
            ErrMsg = "程序停止";
            Machine = ProcessMachine.NoInit;
        }

        public static async Task ProcessPause()
        {
            IsRun = false;
            if (ProcessTask != null)
            {
                if (ProcessTask.Status == TaskStatus.Running)
                    await ProcessTask;
            }
            ErrMsg = "程序暂停";
            Machine = ProcessMachine.Stopping;
        }


    }
    public enum ProcessMachine
    {
        NoInit,
        Stopping,
        Running,
        Error,
    }
}
