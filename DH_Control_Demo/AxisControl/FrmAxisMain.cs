using AngusTools.FileHelper;
using AntdUI;
using csLTDMC;
using DH_Control_Demo.AxisControl.DH_AxisCtrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#pragma warning disable CS8602 // 解引用可能出现空引用。

namespace DH_Control_Demo.AxisControl
{
    public partial class FrmAxisMain : Form
    {
        public FrmAxisMain()
        {
            InitializeComponent();
        }

        #region 轴状态显示与绑定

        /// <summary>
        /// 轴号
        /// 当前界面控制的轴
        /// </summary>
        private int _AxisNum;

        /// <summary>
        /// 每次切换界面时，变更界面绑定轴的事件订阅
        /// </summary>
        /// <param name="axisNum"></param>
        public void OnFrmShow_Or_OnChangeAxisNem(int axisNum)
        {
            _AxisNum = axisNum;
            //解绑原有事件订阅
            for (int i = 0; i < GVL.AxisMontions?.Length; i++)
            {
                if (GVL.AxisMontions[i] != null)
                    GVL.AxisMontions[i].AxisStateActionEvent -= FrmAxisMain_AxisStateActionEvent1;
            }

            //订阅新轴号事件
            if (GVL.AxisMontions != null)
                GVL.AxisMontions[_AxisNum].AxisStateActionEvent += FrmAxisMain_AxisStateActionEvent1;
            if (GVL.AxisMontions[_AxisNum].AxisAct.Name.ToLower().Contains("ce") || GVL.AxisMontions[_AxisNum].AxisAct.Name.Contains("电缸"))
                avatar1.Image = Image.FromFile(@"Image\\MCE.png");
            else
                avatar1.Image = Image.FromFile(@"Image\\VLA.png");
            //更新界面选定数据
            selHomeControler_SelectedIndexChanged(new object(), new IntEventArgs(0));
            //更新点位数据
            GetPointData();
        }

        short MoveDoneTrig;
        short RefreshInterval = 0;
        /// <summary>
        /// 轴事件触发，更新界面轴实时状态显示
        /// </summary>
        /// <param name="act">轴实时状态结构体</param>
        private void FrmAxisMain_AxisStateActionEvent1(AxisActStruct act)
        {
            if (GVL.AxisMontions == null)
                return;
            //在界面线程中更新
            PanAxisAct.Invoke(new Action(() =>
            {
                //状态机显示
                PanActMachine.Text = GVL.AxisMontions[_AxisNum].AxisStateMachine(act.AxisMachine, 0);
                PanActMachine.Icon = act.AxisMachine switch
                {
                    0 => TType.Warn,
                    4 => TType.Success,
                    6 => TType.Error,
                    7 => TType.Error,
                    _ => TType.Info,
                };
                //运动状态显示
                if (act.IsStopping == 0)
                {
                    bdgIsMov.Text = "运动中";
                    bdgIsMov.State = TState.Processing;
                }
                else if (act.IsStopping == 1)
                {
                    bdgIsMov.Text = "停止中";
                    bdgIsMov.State = TState.Warn;
                }
                else
                {
                    bdgIsMov.Text = "未检测状态";
                    bdgIsMov.State = TState.Default;
                }
                PanActPosition.Text = act.Position.ToString();//实时位置
                if (RefreshInterval < 100) RefreshInterval++;
                else
                {
                    PanActVel.Text = act.Velocity.ToString();//实时速度
                    PanActTorque.Text = act.Torque.ToString();//实时力矩
                    RefreshInterval = 0;
                }
                PanActState.Text = act.AxisState.ToString();//实时状态
                PanActErr.Text = act.ErrorCode.ToString();//错误代码
                PanStatus.Text = act.StatusStr;//状态释义

                if (MoveDoneTrig != act.IsStopping)
                {
                    MoveDoneTrig = act.IsStopping;
                }
            }));
        }
        #endregion

        #region 轴基础操作
        private void BtnServoStop_Click(object sender, EventArgs e) => GVL.AxisMontions?[_AxisNum].ServoStop(0);//伺服停止

        private void BtnServoReset_Click(object sender, EventArgs e) => GVL.AxisMontions?[_AxisNum].ErrorReset();//伺服故障清除

        private void BtnServoDisEnable_Click(object sender, EventArgs e) => GVL.AxisMontions?[_AxisNum].ServoEnable(false);//伺服失能

        private void BtnServoEnable_Click(object sender, EventArgs e) => GVL.AxisMontions?[_AxisNum].ServoEnable(true);//伺服使能

        #endregion

        #region 轴回零触发

        /// <summary>
        /// 根据选择的回零控制类型，更新回零方式的选择界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selHomeControler_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            selHomeMethod.Items.Clear();
            switch (selHomeControler.SelectedIndex)
            {
                case 0://如果选择常规方式回零，更新1-35常规回零方法
                    for (int i = 0; i < 36; i++)
                    {
                        selHomeMethod.Items.Add(new MenuItem("常规方法：" + i.ToString()));
                    }
                    selHomeMethod.SelectedIndex = 34;
                    break;
                case 1:
                case 2://如果选择的非标回零，更新非标回零方法
                    selHomeMethod.Items.Add(new MenuItem("负硬限位:-1"));
                    selHomeMethod.Items.Add(new MenuItem("正硬限位:-2"));
                    selHomeMethod.Items.Add(new MenuItem("负硬限位+Z相:-3"));
                    selHomeMethod.Items.Add(new MenuItem("正硬限位+Z相:-4"));
                    selHomeMethod.Items.Add(new MenuItem("负硬限位+原点:-19"));
                    selHomeMethod.Items.Add(new MenuItem("正硬限位+原点:-20"));
                    selHomeMethod.SelectedIndex = 2;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 回零触发
        /// 必要条件：正确选择回零模式并进行关键参数设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnServoGoHome_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;

            //确认回零控制模式及回零方式
            short homeMethod;
            ServoGoHomeTypeEnum type;
            if (selHomeControler.SelectedIndex == 0)
            {
                homeMethod = (short)selHomeMethod.SelectedIndex;
                type = ServoGoHomeTypeEnum.GenericServoInternal;
            }
            else
            {
                homeMethod = selHomeMethod.SelectedIndex switch
                {
                    0 => -1,
                    1 => -2,
                    2 => -3,
                    3 => -4,
                    4 => -19,
                    5 => -20,
                    _ => 0
                };
                type = ServoGoHomeTypeEnum.NonGenericServoInternal;
            }

            //调用回零
            short result = (short)(GVL.AxisMontions?[_AxisNum].ServoGoHome(homeType: type,
                                                               homeMode: (byte)homeMethod,
                                                               velLo: (double)inHomeVL.Value,
                                                               velHi: (double)inHomeVH.Value,
                                                               acc: (double)inHomeACC.Value,
                                                               dec: (double)inHomeACC.Value,
                                                               homeOffset: 0,
                                                               backDistance: 1,
                                                               stallTime: (short)inHomeStime.Value,
                                                               stallCurrent: (short)inHomeSCurrent.Value));

            //如果调用出错，弹窗报警并结束流程
            if (result != 0)
                AntdUI.Modal.open(this, "命令回零启动失败",
                                         "在调用回零指令时出现异常" + "\r\n" + "错误代码：" + result.ToString(),
                                         AntdUI.TType.Error);
            else
            {
                //等待回零完成
                result = await GVL.AxisMontions?[_AxisNum].WaitHomeDone(10000, true);
                //如果超时未回零完成，弹窗报警并结束流程
                if (result != 0) AntdUI.Modal.open(this, "等待回零完成超时",
                                             "未在预定时间内回零完成" + "\r\n" + "错误代码：" + result.ToString(),
                                             AntdUI.TType.Error);
            }
            btn.Loading = false;
        }

        #endregion

        #region 点动

        /// <summary>
        /// 点动/定长运动
        /// 默认点动是给通过相对运动给一个很大的位置进行触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJogCw_MouseDown(object sender, MouseEventArgs e)
        {
            double dir;
            if (sender == BtnJogCw)
                dir = -9999;
            else if (sender == BtnJogCCW)
                dir = 9999;
            else
                return;
            GVL.AxisMontions[_AxisNum].ServoMoveRel(dir, (double)inJogVel.Value, 30000, 30000);
        }

        private void BtnJogCw_MouseUp(object sender, MouseEventArgs e) => GVL.AxisMontions[_AxisNum].ServoStop(0);

        /// <summary>
        /// 软着陆运动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSoftLand_Click_1Async(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            await GVL.AxisMontions[_AxisNum].SoftLand_ServoExternal(
                ppPosition: (double)inPpPos.Value,
                ptPosition: (double)inPtPos.Value,
                pbPosition: (double)inPbPos.Value,
                ppVel: (double)inPpVel.Value,
                ptVel: (double)inPtVel.Value,
                pbVel: (double)inPpVel.Value,
                acc: (double)inAcc.Value,
                dec: (double)inAcc.Value,
                torqueLimit: (int)inInstallPress.Value,
                installTime: (int)inInstallTime.Value,
                func: null
                );
            btn.Loading = false;
        }
        #endregion

        #region 点位数据拉取
        DataTable DtPoint;
        private void GetPointData()
        {
            DtPoint = CsvHelper.CsvToDataTable(@"Config\\PointData.csv");
            table1.DataSource = DtPoint;
        }


        /// <summary>
        /// 单元格数据编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void table1_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex < 1)
                return;
            //获取双击单元格的数据
            string str = (string)DtPoint.Rows[e.RowIndex - 1][e.ColumnIndex];

            //如果是驱动器列，弹出下拉选项卡
            //否则弹出数据输入框
            if (e.ColumnIndex == 5)
            {
                GVL.AxisMontions[_AxisNum].ServoMoveAbs(
                                            positionAbs: Convert.ToDouble(DtPoint.Rows[e.RowIndex - 1]["Position"]),
                                            vel: Convert.ToDouble(DtPoint.Rows[e.RowIndex - 1]["Velocity"]),
                                            acc: Convert.ToDouble(DtPoint.Rows[e.RowIndex - 1]["Acc/Dec"]),
                                            dec: Convert.ToDouble(DtPoint.Rows[e.RowIndex - 1]["Acc/Dec"]));
            }
            else
            {
                Input input = new Input()
                {
                    Size = new Size(240, 40),
                    Text = str,
                };
                if (AntdUI.Modal.open(new Modal.Config(
                                        this, DtPoint.Columns[e.ColumnIndex].ToString(),
                                        input, TType.Info)) == DialogResult.OK)
                    DtPoint.Rows[e.RowIndex - 1][e.ColumnIndex] = input.Text;
                RefreshTableData();
            }

        }

        /// <summary>
        /// 重新加载并填充表单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Reload(object sender, EventArgs e)
        {
            GetPointData();
        }

        /// <summary>
        /// 删除某行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Delet(object sender, EventArgs e)
        {
            if (table1.SelectedIndex > 0 && DtPoint.Rows.Count > 1)
                DtPoint.Rows.Remove(DtPoint.Rows[table1.SelectedIndex - 1]);
            RefreshTableData();
        }

        /// <summary>
        /// 新建轴配置，默认参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Add(object sender, EventArgs e)
        {
            DataRow dr = DtPoint.NewRow();
            dr["Number"] = (Convert.ToInt16(DtPoint.Rows[^1]["Number"]) + 1).ToString();
            dr["Name"] = "未定义点位";
            dr["Position"] = DtPoint.Rows[^1]["Position"];
            dr["Velocity"] = DtPoint.Rows[^1]["Velocity"];
            dr["Acc/Dec"] = DtPoint.Rows[^1]["Acc/Dec"];
            dr["Trigger"] = "0";
            DtPoint.Rows.Add(dr);
            RefreshTableData();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Save(object sender, EventArgs e)
        {
            CsvHelper.DataTableToCsv(DtPoint, @"Config\\PointData.csv");

        }

        /// <summary>
        /// 数据绑定到表单显示
        /// </summary>
        private void RefreshTableData()
        {
            //table1.Columns.Clear();
            //table1.Columns = new AntdUI.ColumnCollection {
            //    new Column("Name", "轴名称"),
            //    //new Column("CardID", "卡号"),
            //    new ColumnCheck("CardID","驱动器")};
            table1.DataSource = DtPoint;

        }


        #endregion

        #region SDO读写
        private void BtnW_Click(object sender, EventArgs e)
        {
            byte[] bytesData = BitConverter.GetBytes(int.Parse(iWdata.Text));
            var varData = 0;
            switch (iWselet.SelectedIndex)
            {
                case 0:
                    varData = bytesData[0];
                    if (bytesData[1] != 0 || bytesData[2] != 0 || bytesData[3] != 0)
                        MessageBox.Show("数据溢出，实际转化值为：" + varData.ToString());
                    break;
                case 1:
                    varData = BitConverter.ToInt16(bytesData);
                    if (bytesData[2] != 0 || bytesData[3] != 0)
                        MessageBox.Show("数据溢出，实际转化值为：" + varData.ToString());
                    break;
                case 2:
                    varData = BitConverter.ToInt32(bytesData);
                    break;
            }
            GVL.AxisMontions[_AxisNum].SetSdo(Convert.ToUInt16(iWadr.Text, 16), Convert.ToUInt16(iWsubAdr.Text, 16), varData);
        }
        private void BtnRead_Click(object sender, EventArgs e)
        {
            ushort dataSize = iRselet.SelectedIndex switch
            {
                0 => 8,
                1 => 16,
                2 => 32
            };
            GVL.AxisMontions[_AxisNum].GetSdo(Convert.ToUInt16(iRadr.Text, 16), Convert.ToUInt16(iRsubAdr.Text, 16), dataSize, out int dataVar);
            iRdata.Text = dataVar.ToString();
        }
        #endregion


        #region 柔性力控功能---For HSD力控测试
        private DataTable DtFlexibleForceData = new();
        private void InitForceData()
        {
            DtFlexibleForceData.Clear();//清除数据
            DtFlexibleForceData = AngusTools.FileHelper.CsvHelper.CsvToDataTable(@"..\..\..\Data\FlexibleForceData.csv");//加载力控数据
            Table_FlexibleForce.DataSource = DtFlexibleForceData;
            StartMonitor();
        }

        /// <summary>
        /// 开始监控软着陆状态值
        /// </summary>
        private void StartMonitor()
        {
            ushort statusAdd = 1, errorAdd = 2,actForceAdd=3;
            int statusValue = 0, errorValue = 0,actForceValue=0;

            _ = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //读取软着陆状态
                    LTDMC.nmc_read_txpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID,
                        2,
                        statusAdd,
                        1,
                        ref statusValue);
                    //读取故障代码
                    LTDMC.nmc_read_txpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID,
                        2,
                        errorAdd,
                        1,
                        ref errorValue);
                    //读取压力传感器力值
                    LTDMC.nmc_read_txpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID,
                         2,
                         actForceAdd,
                         1,
                         ref actForceValue);
                    Tv_ActForce.Text = $"{actForceValue} g";

                    var statusStr = statusValue switch
                    {
                        0 => "IDLE状态",
                        1 => "收到启动指令",
                        2 => "PP1 运动中",
                        3 => "PP1到达",
                        6 => "PP2运动中",
                        14 => "压⼒到达95%",
                        15 => "进⼊保压阶段",
                        8 => "闭环⼒控完成",
                        11 => "准备返回启动位置",
                        12 => "电机正在返回",
                        13 => "软着陆异常",
                        _ => "值异常"
                    };
                    Tv_Status.Text = $"{statusValue}--{statusStr}";
                }
            });
        }

        /// <summary>
        /// 表格右键功能实现
        /// 加载、保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_FlexibleForce_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标在数据表格上按下鼠标右键后创建右键菜单
            if (e.Button != MouseButtons.Right) return;

            IContextMenuStripItem[] menuStripItems =
            [
                new AntdUI.ContextMenuStripItem("加载","加载历史数据").SetIcon("FolderOpenOutlined"),
                new AntdUI.ContextMenuStripItem("保存","保存数据到备份").SetIcon("SaveOutlined"),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("写入","通过SDO将数据写入到驱动器").SetIcon("DownloadOutlined")
            ];

            //委托右键菜单的实现
            AntdUI.ContextMenuStrip.open(this, e =>
            {
                switch (e.Text)
                {
                    case "加载":
                        DtFlexibleForceData.Clear();//清除数据
                        DtFlexibleForceData = AngusTools.FileHelper.CsvHelper.CsvToDataTable(@"..\..\..\Data\FlexibleForceData.csv");//加载力控数据
                        Table_FlexibleForce.DataSource = DtFlexibleForceData;
                        break;
                    case "保存":
                        AngusTools.FileHelper.CsvHelper.DataTableToCsv(DtFlexibleForceData, @"..\..\..\Data\FlexibleForceData.csv");
                        break;
                    case "写入":
                        Stopwatch sw = Stopwatch.StartNew();
                        foreach (DataRow row in DtFlexibleForceData.Rows)
                        {
                            row["Res"] = "未执行";
                        }
                        Table_FlexibleForce.DataSource = DtFlexibleForceData;
                        Task<bool> task = Task<bool>.Factory.StartNew(() =>
                        {
                            Thread.Sleep(50);
                            var isSucess = true;
                            for (int i = 0; i < DtFlexibleForceData.Rows.Count; i++)
                            {
                                var data = DtFlexibleForceData.Rows[i]["Len"].ToString() == "32" ? Convert.ToInt32(DtFlexibleForceData.Rows[i]["Value"].ToString()) : Convert.ToInt16(DtFlexibleForceData.Rows[i]["Value"].ToString());
                                var res = GVL.AxisMontions[_AxisNum].SetSdo(Convert.ToUInt16(DtFlexibleForceData.Rows[i]["Add"].ToString(), 16),
                                                              Convert.ToUInt16(DtFlexibleForceData.Rows[i]["SubAdd"].ToString(), 16),
                                                              data);
                                if (res == 0) DtFlexibleForceData.Rows[i]["Res"] = "True";
                                else
                                {
                                    DtFlexibleForceData.Rows[i]["Res"] = $"False--{res}";
                                    isSucess = false;
                                }
                                Table_FlexibleForce.DataSource = DtFlexibleForceData;
                            }
                            return isSucess;
                        });
                        task.Wait();
                        sw.Stop();
                        if (task.Result) AntdUI.Modal.open(this, "Success", $"SDO写入成功，无错误。指令执行时间：{sw.ElapsedMilliseconds}ms", TType.Success);
                        else AntdUI.Modal.open(this, "Error", $"SDO写入部分未成功。指令执行时间：{sw.ElapsedMilliseconds}ms", TType.Error);
                        break;
                }
            }, menuStripItems);
        }

        /// <summary>
        /// 启动软着陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ForceStart_Click(object sender, EventArgs e)
        {
            var btn = sender as AntdUI.Button;
            Stopwatch sw = Stopwatch.StartNew();
            btn.Loading = true;
            ushort enableAdd = 0, startAdd = 1;
            Task task = Task.Factory.StartNew(() =>
            {
                //SDO启动方法
                //GVL.AxisMontions[_AxisNum].SetSdo(0x5010, 0x2, (Int16)0);//Softland Start写0
                //Thread.Sleep(10);
                //GVL.AxisMontions[_AxisNum].SetSdo(0x5010, 0x1, (Int16)1);//SoftLand Enable写1
                //GVL.AxisMontions[_AxisNum].SetSdo(0x5010, 0x2, (Int16)1);//SoftLand Start写1

                //PDO启动方法
                LTDMC.nmc_write_rxpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID, 2, startAdd, 1, 0);
                LTDMC.nmc_write_rxpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID, 2, enableAdd, 1, 1);
                Thread.Sleep(10);
                LTDMC.nmc_write_rxpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID, 2, startAdd, 1, 1);

                //TODO 等待软着陆流程结束/超时，将启动信号复位
                do
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                } while (sw.ElapsedMilliseconds<=10000);
                LTDMC.nmc_write_rxpdo_extra((ushort)GVL.AxisMontions[_AxisNum].CurrentConfig.CardID, 2, startAdd, 1, 0);
            });

            task.Wait();
            btn.Loading = false;
        }

        /// <summary>
        /// 软着陆停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ForceStop_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(this, "Error", "暂未实现停止功能，当前固件不支持软着陆过程中停止！");
        }

        #endregion

        private void FrmAxisMain_Load(object sender, EventArgs e)
        {
            InitForceData();
        }


    }
}
