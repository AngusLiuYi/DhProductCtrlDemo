using AntdUI;
using csLTDMC;
using DH_Control_Demo.AxisControl.DH_AxisCtrl;
using GTN;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using lctdevice;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Vanara.PInvoke;
using static System.Formats.Asn1.AsnWriter;

namespace DH_Control_Demo
{
    public partial class FrmLoadCard : Form
    {
        public FrmLoadCard()
        {
            InitializeComponent();
            DsConfig = new DataSet();
            sltCardType.SelectedIndex = 0;
        }
        private void FrmLoadCard_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();

        /// <summary>
        /// 板卡初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnOK_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            List<AxisInitConfigStruct> axis = new();
            List<LtdmcConfigStruct> ltdmcConfigs = new();
            List<LcConfigStruct> lcConfigs = new();
            List<GtsConfigStruct> gtsConfigs = new();
            List<ImcConfigStruct> imcConfigs = new();

            GetAxisConfigData(ref axis, ref ltdmcConfigs, ref lcConfigs, ref gtsConfigs,ref imcConfigs);

            Task<string> task1 = Task<string>.Factory.StartNew(() =>
            {
                //Thread.Sleep(300);
                return sltCardType.SelectedIndex switch
                {
                    0 => LtdmcInitCard(axis, ltdmcConfigs),
                    1 => GtsInitCard(axis, gtsConfigs),
                    2 => LcInitCard(axis, lcConfigs),
                    3 => ImcInitCard(axis,imcConfigs),
                    _ => "运动控制卡类型选择错误！",
                } ;
            });
            await task1;
            btn.Loading = false;

            if (task1.Result != 0.ToString())
                AntdUI.Modal.open(new Modal.Config(this, "板卡初始化失败", new AntdUI.Modal.TextLine[] {
                    new Modal.TextLine( "无法正常初始化板卡及加载页面，流程被迫中止"),
                    new Modal.TextLine(task1.Result)
                    }, TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "确认"
                });
            else this.Dispose();
        }

        #region 初始化板卡

        /// <summary>
        /// 雷赛板卡初始化
        /// </summary>
        /// <returns></returns>
        private static string LtdmcInitCard(List<AxisInitConfigStruct> axisInitConfigs, List<LtdmcConfigStruct> ltdmcConfigs)
        {
            if (axisInitConfigs.Count != ltdmcConfigs.Count) return "参数异常，轴数量与配置数量不符";

            short num = LTDMC.dmc_board_init();//获取卡数量
            if (num <= 0 || num > 8) return "雷赛板卡--初始卡失败!";

            //【0】程序退出关联关卡
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit_LtdmcCloseCard;

            ushort _num = 0;
            ushort[] cardids = new ushort[8];
            uint[] cardtypes = new uint[8];
            short res = LTDMC.dmc_get_CardInfList(ref _num, cardtypes, cardids);
            if (res != 0) return "雷赛板卡--获取卡信息失败!";
            else if (axisInitConfigs[0].CardID != (short)(cardids[0] - 1)) return "设定卡号与扫描到的卡号不符";//默认卡号为0，雷赛需要加一处理

            //初始化轴
            GVL.AxisMontions = new LtdmcMontion[axisInitConfigs.Count];
            for (int i = 0; i < GVL.AxisMontions.Length; i++)
            {
                GVL.AxisMontions[i] = new LtdmcMontion(axisInitConfigs[i], ltdmcConfigs[i]);
            }
            return 0.ToString();
        }

        /// <summary>
        /// 关卡--雷赛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit_LtdmcCloseCard(object? sender, EventArgs e)
        {
            try
            {
                ulong nCardHandle = 0;
                LTDMC.dmc_board_close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 固高板卡初始化
        /// </summary>
        /// <returns></returns>
        private static string GtsInitCard(List<AxisInitConfigStruct> axisInitConfigs, List<GtsConfigStruct> gtsConfig)
        {

            //注意，固高卡必须要在bin文件目录下加载一下文件。所需文件可以向固高技术索取。
            //1、ecat_config.dll
            //2、ecat_master.dll
            //3、gts.dll
            //4、gts.lib
            //5、Gecat.xml

            List<short> result = new();
            //mc.GTN_Close();
            Thread.Sleep(200);

            //【0】程序退出关联关卡
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit_GtsCloseCard;

            result.Add(mc.GTN_Open(5, 2));
            if (result.Last() != 0) return "固高板卡：初始卡失败! 错误代码：" + result.Last().ToString();

            //rtn = mc.GTN_InitEcatComm(_Core);
            short _core = (short)(axisInitConfigs[0].CardID + 1);
            result.Add(mc.GTN_InitEcatComm(_core));
            //result.Add(mc.GTN_InitEcatCommEx(_core, "Gecat.XML"));
            if (result.Last() != 0) return "固高板卡：初始化总线失败! 错误代码：" + result.Last().ToString();
            short etcSts;
            Stopwatch sw = new();
            sw.Start();
            result.Add(1);
            do
            {
                // 读取EtherCAT总线状态
                result[^1] = mc.GTN_IsEcatReady(_core, out etcSts);
                if (sw.ElapsedMilliseconds > 6000)
                    return "固高板卡：初始化总线——连接从站失败! 错误代码：" + result.Last().ToString();
            } while (etcSts != 1);
            result.Add(mc.GTN_StartEcatComm(_core));
            result.Add(mc.GTN_Reset(_core));
            if (result.Sum(x => x) != 0) return "固高板卡：总线OP切换失败";

            //初始化轴
            GVL.AxisMontions = new GtsMontion[axisInitConfigs.Count];
            for (int i = 0; i < GVL.AxisMontions.Length; i++)
            {
                GVL.AxisMontions[i] = new GtsMontion(axisInitConfigs[i], gtsConfig[i]);
            }
            return 0.ToString();
        }

        /// <summary>
        /// 关卡--固高
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit_GtsCloseCard(object? sender, EventArgs e)
        {

            try
            {
                // 中断EtherCAT通讯
                GTN.mc.GTN_TerminateEcatComm(1);
                // 关闭运动控制器
                GTN.mc.GTN_Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 凌臣板卡初始化
        /// </summary>
        /// <returns></returns>
        private static string LcInitCard(List<AxisInitConfigStruct> axisInitConfigs, List<LcConfigStruct> lcConfig)
        {
            List<short> result = new();
            //凌臣卡号从0开始
            short cardID = axisInitConfigs[0].CardID;

            //【0】程序退出关联关卡
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit_LcCloseCard;

            //1、获取卡号=>开卡
            result.Add(ecat_motion.M_Open(cardID, 0));//初始化PCI-M60板卡，默认卡号0，必要参数0
            if (result.Last() != 0) return result.Last().ToString();

            //2、设置急停回路
            result.Add(ecat_motion.M_SetEmgAction(0x00, cardID));//0x00：不减速，直接掉使能（默认值）
            result.Add(ecat_motion.M_SetEmgInv(0, cardID));//设置急停极性，1：常闭-NC，0：常开-NO
            result.Add(ecat_motion.M_ClrEmg(cardID));//复位急停状态，清除急停报错

            //3\重置FPGA芯片
            result.Add(ecat_motion.M_ResetFpga(cardID));//复位FPGA芯片
            Thread.Sleep(500);//必要延时

            //4、加载eni文件
            result.Add(ecat_motion.M_LoadEni("C:\\Program Files (x86)\\LCT\\PCIe-M60\\Eni\\eni.xml",
                                                cardID));//加载eni.xml文件，该文件为驱动器和远程IO模块的配置文件
            if (result.Sum(x => x) != 0) return "加载eni文件失败";

            if (false)
            {
                //连接检测功能启用时，才加载此语句，否则不需要执行
                result.Add(ecat_motion.M_LoadEcatConfigDefault(cardID));//加载EcatConfiguration.ini文件
                if (result.Last() != 0) return "C:\\ProgramData\\LCT Devices路径下没有EcatConfiguration.ini文件，请先使用motion assistant软件成功连接从站，它会自动生成该文件";
            }
            //5、连接总线
            result.Add(ecat_motion.M_ConnectECAT(0, cardID));//连接总线，option--设置断线输出保持的参数，0:不保持；1:保持
            if (result.Last() == 30) return "总线拓扑结构发生改变，连接失败！请检查";
            if (result.Last() != 0) return "总线连接失败";//需测试，demo中不启用连接检测时无此语句
            Thread.Sleep(500);//必要延时
            if (false)
            {
                //6、加载配置参数--需测试，不加载参数是否可以
                result.Add(ecat_motion.M_LoadParamFromFile("C:\\Program Files (x86)\\LCT\\PCIe-M60\\Motion_Assistant\\AxisParam\\ParamCard0.ini", cardID));//从文件加载参数
                if (result.Last() != 0) return "从文件加载参数失败";//需测试，demo中不启用连接检测时无此语句
            }
            //初始化轴
            GVL.AxisMontions = new LcMontion[axisInitConfigs.Count];
            for (int i = 0; i < GVL.AxisMontions.Length; i++)
            {
                GVL.AxisMontions[i] = new LcMontion(axisInitConfigs[i], lcConfig[i]);
            }
            return 0.ToString();

            //7、获取从站资源--需测试，不获取从站资源是否可以
            ecat_motion.SL_RES sL_RES = new ecat_motion.SL_RES();
            result.Add(ecat_motion.M_GetSlaveResource(out sL_RES, cardID));
            if (result.Last() != 0) return "获取从站资源失败";
            string str = "从站数量：" + sL_RES.SlaveNum.ToString() + "\r\n";
            str += "轴数量：" + sL_RES.AxisNum.ToString() + "\r\n";
            str += "IO从站数：" + sL_RES.IoSlaveNum.ToString() + "\r\n";
            str += "DI数量：" + sL_RES.DiNum.ToString() + "\r\n";
            str += "DO数量：" + sL_RES.DoNum.ToString() + "\r\n";
            str += "AI数量：" + sL_RES.AiNum.ToString() + "\r\n";
            str += "AO数量：" + sL_RES.AoNum.ToString() + "\r\n";

            return 10.ToString();
        }

        /// <summary>
        /// 关卡--凌臣
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit_LcCloseCard(object? sender, EventArgs e)
        {
            try
            {
                ulong nCardHandle = 0;
                ImcApi.IMC_OpenCardHandle(0, ref nCardHandle);
                ImcApi.IMC_CloseCard(nCardHandle);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 汇川板卡初始化
        /// </summary>
        /// <param name="axisInitConfigs"></param>
        /// <param name="ImcConfigs"></param>
        /// <returns></returns>
        private static string ImcInitCard(List<AxisInitConfigStruct> axisInitConfigs,List<ImcConfigStruct> ImcConfigs)
        {
            if (axisInitConfigs.Count != ImcConfigs.Count) return "参数异常，轴数量与配置数量不符";
            List<uint> result= new();
            //【1】获取卡
            Int32 nCardNum = 0;
            result.Add(ImcApi.IMC_GetCardsNum(ref nCardNum));
            if (result.Last() != 0) return "汇川获取轴卡失败,错误代码为0x：" + result.Last().ToString();
            if (nCardNum <= 0) return "汇川未找到有效运动卡，0x" + nCardNum.ToString();

            //【2】打开卡句柄
            ulong nCardHandle = 0;
            result.Add(ImcApi.IMC_OpenCardHandle(axisInitConfigs[0].CardID, ref nCardHandle));
            if (result.Last() != 0) return "汇川获取卡句柄失败,错误代码为0x：" + result.Last().ToString();

            //【3】下载设备参数
            result.Add(ImcApi.IMC_DownLoadDeviceConfig(nCardHandle, ".\\deviceConfig.xml"));
            if (result.Last() != 0) return "汇川下载设备参数失败,错误代码为0x：" + result.Last().ToString();

            //【3.5】程序退出关联关卡
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit_ImcCloseCard;

            //【4】启动主站
            uint masterStatus = 0;
            result.Add(ImcApi.IMC_GetECATMasterSts(nCardHandle, ref masterStatus));
            if (result.Last() != 0) return "汇川获取主站状态失败,错误代码为0x：" + result.Last().ToString();
            if (masterStatus != ImcApi.EC_MASTER_OP)
            {
                result.Add(ImcApi.IMC_ScanCardECAT(nCardHandle, 1));     //默认阻塞式启动EtherCAT
                if (result.Last() != 0) return "汇川启动EtherCAT失败,错误代码为0x：" + result.Last().ToString();
            }

            //【5】下载系统参数
            result.Add(ImcApi.IMC_DownLoadSystemConfig(nCardHandle, ".\\systemConfig.xml"));
            if (result.Last() != 0) return "汇川下载系统参数失败,错误代码为0x：" + result.Last().ToString();

            //【6】扫描卡内资源
            ImcApi.TRsouresNum tResource = new ImcApi.TRsouresNum();//实例化板卡外设硬件资源
            result.Add(ImcApi.IMC_GetCardResource(nCardHandle, ref tResource));
            if (result.Last() != 0) return "扫描系统资源失败,错误代码为0x：" + result.Last().ToString();

            //初始化轴
            GVL.AxisMontions = new ImcMontion[axisInitConfigs.Count];
            for (int i = 0; i < GVL.AxisMontions.Length; i++)
            {
                GVL.AxisMontions[i] = new ImcMontion(axisInitConfigs[i], ImcConfigs[i]);
                GVL.AxisMontions[i].OnError = (msg) => MessageBox.Show(msg);
            }
            return 0.ToString();
        }

        /// <summary>
        /// 关卡--汇川
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit_ImcCloseCard(object? sender, EventArgs e)
        {
            try
            {
                ulong nCardHandle = 0;
                ImcApi.IMC_OpenCardHandle(0, ref nCardHandle);
                ImcApi.IMC_CloseCard(nCardHandle);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region 轴配置数据拉取

        DataSet DsConfig;

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
            string str = (string)DsConfig.Tables[0].Rows[e.RowIndex - 1][e.ColumnIndex];

            //如果是驱动器列，弹出下拉选项卡
            //否则弹出数据输入框
            if (e.ColumnIndex == 4)
            {
                Select select = new();
                select.Size = new Size(240, 40);
                select.Items.AddRange(Enum.GetNames(typeof(ServoDriveTypeEnum)));
                select.SelectedIndex = (int)Enum.Parse(typeof(ServoDriveTypeEnum), str);
                if (AntdUI.Modal.open(new Modal.Config(
                        this, DsConfig.Tables[0].Columns[e.ColumnIndex].ToString(),
                        select, TType.Info)) == DialogResult.OK)
                    DsConfig.Tables[0].Rows[e.RowIndex - 1][e.ColumnIndex] = select.Items[select.SelectedIndex].ToString();
            }
            else
            {
                Input input = new Input()
                {
                    Size = new Size(240, 40),
                    Text = str,
                };
                if (AntdUI.Modal.open(new Modal.Config(
                                        this, DsConfig.Tables[0].Columns[e.ColumnIndex].ToString(),
                                        input, TType.Info)) == DialogResult.OK)
                    DsConfig.Tables[0].Rows[e.RowIndex - 1][e.ColumnIndex] = input.Text;
            }
            RefreshTableData();

        }

        /// <summary>
        /// 重新加载并填充表单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Reload(object sender, EventArgs e)
        {
            sltCardType_SelectedIndexChanged(sender, new IntEventArgs(sltCardType.SelectedIndex));
        }

        /// <summary>
        /// 删除某行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Delet(object sender, EventArgs e)
        {
            if (table1.SelectedIndex > 0 && DsConfig.Tables[0].Rows.Count > 1)
                DsConfig.Tables[0].Rows.Remove(DsConfig.Tables[0].Rows[table1.SelectedIndex - 1]);
            RefreshTableData();
        }

        /// <summary>
        /// 新建轴配置，默认参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Add(object sender, EventArgs e)
        {
            DataRow dr = DsConfig.Tables[0].NewRow();
            dr["Name"] = "未定义轴";
            dr["CardID"] = DsConfig.Tables[0].Rows[^1]["CardID"];
            dr["SlaveID"] = (Convert.ToInt16(DsConfig.Tables[0].Rows[^1]["SlaveID"]) + 1).ToString();
            dr["AxisID"] = (Convert.ToInt16(DsConfig.Tables[0].Rows[^1]["AxisID"]) + 1).ToString();
            dr["ServoDriveType"] = "ISD大寰驱动器";
            dr["Pluse"] = "1";
            dr["Unit"] = "1";
            dr["TorqueTxPdoAddress"] = "-1";
            DsConfig.Tables[0].Rows.Add(dr);
            RefreshTableData();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table1Save(object sender, EventArgs e)
        {
            DsConfig.WriteXml(@"Config\\AxisConfig.config");

        }

        /// <summary>
        /// 读取配置文件到DataSet中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sltCardType_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            DsConfig = new DataSet();
            try
            {
                DsConfig.ReadXml(@"Config\\AxisConfig.config");
                RefreshTableData();
            }
            catch { }
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
            table1.DataSource = DsConfig.Tables[0];

        }

        private void GetAxisConfigData(ref List<AxisInitConfigStruct> axis, ref List<LtdmcConfigStruct> ltdmcConfigs, ref List<LcConfigStruct> lcConfigs, ref List<GtsConfigStruct> gtsConfigs,ref List<ImcConfigStruct> imcConfigs)
        {
            foreach (DataRow dr in DsConfig.Tables[0].Rows)
            {
                AxisInitConfigStruct axisConfig = new AxisInitConfigStruct()
                {
                    Name = dr["Name"].ToString(),
                    CardID = Convert.ToInt16(dr["CardID"]),
                    SlaveID = Convert.ToInt16(dr["SlaveID"]),
                    AxisID = Convert.ToInt16(dr["AxisID"]),
                    ServoDriveType = (ServoDriveTypeEnum)Enum.Parse(typeof(ServoDriveTypeEnum), dr["ServoDriveType"].ToString()),
                    Pluse = Convert.ToDouble(dr["Pluse"]),
                    Unit = Convert.ToDouble(dr["Unit"]),
                };
                LtdmcConfigStruct ltdmc = new LtdmcConfigStruct()
                {
                    torqueTxPdoAddress = Convert.ToInt16(dr["torqueTxPdoAddress"])
                };
                LcConfigStruct lc = new LcConfigStruct();
                GtsConfigStruct gts = new GtsConfigStruct();
                ImcConfigStruct imc = new ImcConfigStruct();
                axis.Add(axisConfig);
                ltdmcConfigs.Add(ltdmc);
                lcConfigs.Add(lc);
                gtsConfigs.Add(gts);
                imcConfigs.Add(imc);
            }
        }
        #endregion
    }
}