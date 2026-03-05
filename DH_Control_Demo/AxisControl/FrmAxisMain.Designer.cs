namespace DH_Control_Demo.AxisControl
{
    partial class FrmAxisMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAxisMain));
            avatar1 = new AntdUI.Avatar();
            panel4 = new AntdUI.Panel();
            contextMenuStrip1 = new ContextMenuStrip(components);
            删除ToolStripMenuItem = new ToolStripMenuItem();
            增加ToolStripMenuItem = new ToolStripMenuItem();
            保存ToolStripMenuItem = new ToolStripMenuItem();
            重新加载ToolStripMenuItem = new ToolStripMenuItem();
            panel5 = new Panel();
            panEnable = new AntdUI.Panel();
            BtnServoEnable = new AntdUI.Button();
            panel2 = new Panel();
            inputNumber1 = new AntdUI.InputNumber();
            inJogVel = new AntdUI.InputNumber();
            BtnJogCCW = new AntdUI.Button();
            BtnJogCw = new AntdUI.Button();
            BtnServoReset = new AntdUI.Button();
            BtnServoStop = new AntdUI.Button();
            BtnServoDisEnable = new AntdUI.Button();
            panel7 = new Panel();
            panel3 = new AntdUI.Panel();
            tag13 = new AntdUI.Tag();
            tabControl1 = new TabControl();
            TpGoHome = new TabPage();
            alert11 = new AntdUI.Alert();
            panHomeMethod = new Panel();
            BtnServoGoHome = new AntdUI.Button();
            selHomeMethod = new AntdUI.Select();
            selHomeControler = new AntdUI.Select();
            tag6 = new AntdUI.Tag();
            inHomeSCurrent = new AntdUI.InputNumber();
            tag5 = new AntdUI.Tag();
            inHomeStime = new AntdUI.InputNumber();
            tag3 = new AntdUI.Tag();
            inHomeACC = new AntdUI.InputNumber();
            tag2 = new AntdUI.Tag();
            inHomeVL = new AntdUI.InputNumber();
            tag1 = new AntdUI.Tag();
            inHomeVH = new AntdUI.InputNumber();
            TpMoveAbs = new TabPage();
            table1 = new AntdUI.Table();
            TpSoftLand = new TabPage();
            tag15 = new AntdUI.Tag();
            tag14 = new AntdUI.Tag();
            panel6 = new Panel();
            inInstallTime = new AntdUI.InputNumber();
            inInstallPress = new AntdUI.InputNumber();
            BtnSoftLand = new AntdUI.Button();
            inPbPos = new AntdUI.InputNumber();
            inPtVel = new AntdUI.InputNumber();
            inPtPos = new AntdUI.InputNumber();
            inAcc = new AntdUI.InputNumber();
            inPpVel = new AntdUI.InputNumber();
            inPpPos = new AntdUI.InputNumber();
            pictureBox1 = new PictureBox();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            panel8 = new Panel();
            BtnRead = new AntdUI.Button();
            tag10 = new AntdUI.Tag();
            iRdata = new AntdUI.Input();
            tag11 = new AntdUI.Tag();
            iRselet = new AntdUI.Select();
            tag17 = new AntdUI.Tag();
            tag16 = new AntdUI.Tag();
            iRsubAdr = new AntdUI.Input();
            iRadr = new AntdUI.Input();
            panel1 = new Panel();
            tag9 = new AntdUI.Tag();
            iWdata = new AntdUI.Input();
            tag8 = new AntdUI.Tag();
            tag7 = new AntdUI.Tag();
            tag4 = new AntdUI.Tag();
            iWselet = new AntdUI.Select();
            iWsubAdr = new AntdUI.Input();
            iWadr = new AntdUI.Input();
            BtnW = new AntdUI.Button();
            TpFlexibleForce = new TabPage();
            Table_FlexibleForce = new AntdUI.Table();
            panel10 = new Panel();
            Tv_Error = new AntdUI.Shield();
            Tv_Status = new AntdUI.Shield();
            Tv_ActForce = new AntdUI.Shield();
            panel9 = new Panel();
            Btn_ForceStop = new AntdUI.Button();
            Btn_ForceStart = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            PanAxisAct = new AntdUI.Panel();
            tag12 = new AntdUI.Tag();
            bdgIsMov = new AntdUI.Badge();
            PanStatus = new AntdUI.Alert();
            label8 = new Label();
            PanActErr = new AntdUI.Alert();
            label3 = new Label();
            PanActState = new AntdUI.Alert();
            label6 = new Label();
            PanActTorque = new AntdUI.Alert();
            label2 = new Label();
            PanActVel = new AntdUI.Alert();
            label1 = new Label();
            PanActMachine = new AntdUI.Alert();
            PanActPosition = new AntdUI.Alert();
            label5 = new Label();
            divider1 = new AntdUI.Divider();
            label7 = new Label();
            panel4.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel5.SuspendLayout();
            panEnable.SuspendLayout();
            panel2.SuspendLayout();
            panel7.SuspendLayout();
            panel3.SuspendLayout();
            tabControl1.SuspendLayout();
            TpGoHome.SuspendLayout();
            panHomeMethod.SuspendLayout();
            TpMoveAbs.SuspendLayout();
            TpSoftLand.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage3.SuspendLayout();
            panel8.SuspendLayout();
            panel1.SuspendLayout();
            TpFlexibleForce.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            PanAxisAct.SuspendLayout();
            SuspendLayout();
            // 
            // avatar1
            // 
            avatar1.Dock = DockStyle.Fill;
            avatar1.Image = (Image)resources.GetObject("avatar1.Image");
            avatar1.Location = new Point(44, 44);
            avatar1.Margin = new Padding(3, 4, 3, 4);
            avatar1.Name = "avatar1";
            avatar1.Radius = 6;
            avatar1.Size = new Size(205, 192);
            avatar1.TabIndex = 9;
            // 
            // panel4
            // 
            panel4.ArrowSize = 10;
            panel4.Controls.Add(avatar1);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(14);
            panel4.Radius = 10;
            panel4.Shadow = 24;
            panel4.ShadowOpacityAnimation = true;
            panel4.Size = new Size(293, 280);
            panel4.TabIndex = 20;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 删除ToolStripMenuItem, 增加ToolStripMenuItem, 保存ToolStripMenuItem, 重新加载ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(139, 100);
            // 
            // 删除ToolStripMenuItem
            // 
            删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            删除ToolStripMenuItem.Size = new Size(138, 24);
            删除ToolStripMenuItem.Text = "删除";
            删除ToolStripMenuItem.Click += Table1Delet;
            // 
            // 增加ToolStripMenuItem
            // 
            增加ToolStripMenuItem.Name = "增加ToolStripMenuItem";
            增加ToolStripMenuItem.Size = new Size(138, 24);
            增加ToolStripMenuItem.Text = "增加";
            增加ToolStripMenuItem.Click += Table1Add;
            // 
            // 保存ToolStripMenuItem
            // 
            保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            保存ToolStripMenuItem.Size = new Size(138, 24);
            保存ToolStripMenuItem.Text = "保存";
            保存ToolStripMenuItem.Click += Table1Save;
            // 
            // 重新加载ToolStripMenuItem
            // 
            重新加载ToolStripMenuItem.Name = "重新加载ToolStripMenuItem";
            重新加载ToolStripMenuItem.Size = new Size(138, 24);
            重新加载ToolStripMenuItem.Text = "重新加载";
            重新加载ToolStripMenuItem.Click += Table1Reload;
            // 
            // panel5
            // 
            panel5.Controls.Add(panEnable);
            panel5.Controls.Add(panel4);
            panel5.Dock = DockStyle.Left;
            panel5.Location = new Point(0, 0);
            panel5.Margin = new Padding(3, 4, 3, 4);
            panel5.Name = "panel5";
            panel5.Size = new Size(293, 660);
            panel5.TabIndex = 9;
            // 
            // panEnable
            // 
            panEnable.ArrowSize = 10;
            panEnable.Controls.Add(BtnServoEnable);
            panEnable.Controls.Add(panel2);
            panEnable.Controls.Add(BtnServoReset);
            panEnable.Controls.Add(BtnServoStop);
            panEnable.Controls.Add(BtnServoDisEnable);
            panEnable.Dock = DockStyle.Top;
            panEnable.Location = new Point(0, 280);
            panEnable.Margin = new Padding(3, 4, 3, 4);
            panEnable.Name = "panEnable";
            panEnable.Padding = new Padding(14);
            panEnable.Radius = 10;
            panEnable.Shadow = 24;
            panEnable.ShadowOpacityAnimation = true;
            panEnable.Size = new Size(293, 254);
            panEnable.TabIndex = 21;
            // 
            // BtnServoEnable
            // 
            BtnServoEnable.AutoSizeMode = AntdUI.TAutoSize.Auto;
            BtnServoEnable.IconSvg = "PoweroffOutlined";
            BtnServoEnable.Location = new Point(159, 104);
            BtnServoEnable.Margin = new Padding(3, 4, 3, 4);
            BtnServoEnable.Name = "BtnServoEnable";
            BtnServoEnable.Size = new Size(79, 45);
            BtnServoEnable.TabIndex = 8;
            BtnServoEnable.Text = "励磁";
            BtnServoEnable.Type = AntdUI.TTypeMini.Info;
            BtnServoEnable.Click += BtnServoEnable_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(inputNumber1);
            panel2.Controls.Add(inJogVel);
            panel2.Controls.Add(BtnJogCCW);
            panel2.Controls.Add(BtnJogCw);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(44, 169);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(205, 41);
            panel2.TabIndex = 7;
            // 
            // inputNumber1
            // 
            inputNumber1.Dock = DockStyle.Fill;
            inputNumber1.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inputNumber1.JoinLeft = true;
            inputNumber1.JoinRight = true;
            inputNumber1.Location = new Point(68, 0);
            inputNumber1.Margin = new Padding(3, 4, 3, 4);
            inputNumber1.Name = "inputNumber1";
            inputNumber1.Size = new Size(69, 41);
            inputNumber1.TabIndex = 30;
            inputNumber1.Text = "10";
            inputNumber1.TextAlign = HorizontalAlignment.Center;
            inputNumber1.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // inJogVel
            // 
            inJogVel.Dock = DockStyle.Fill;
            inJogVel.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inJogVel.JoinLeft = true;
            inJogVel.JoinRight = true;
            inJogVel.Location = new Point(68, 0);
            inJogVel.Margin = new Padding(3, 4, 3, 4);
            inJogVel.Name = "inJogVel";
            inJogVel.Size = new Size(69, 41);
            inJogVel.TabIndex = 29;
            inJogVel.Text = "10";
            inJogVel.TextAlign = HorizontalAlignment.Center;
            inJogVel.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // BtnJogCCW
            // 
            BtnJogCCW.Dock = DockStyle.Right;
            BtnJogCCW.IconSvg = "RightOutlined";
            BtnJogCCW.JoinLeft = true;
            BtnJogCCW.Location = new Point(137, 0);
            BtnJogCCW.Margin = new Padding(3, 4, 3, 4);
            BtnJogCCW.Name = "BtnJogCCW";
            BtnJogCCW.Size = new Size(68, 41);
            BtnJogCCW.TabIndex = 8;
            BtnJogCCW.Type = AntdUI.TTypeMini.Success;
            BtnJogCCW.MouseDown += BtnJogCw_MouseDown;
            BtnJogCCW.MouseUp += BtnJogCw_MouseUp;
            // 
            // BtnJogCw
            // 
            BtnJogCw.Dock = DockStyle.Left;
            BtnJogCw.IconSvg = "LeftOutlined";
            BtnJogCw.JoinRight = true;
            BtnJogCw.Location = new Point(0, 0);
            BtnJogCw.Margin = new Padding(3, 4, 3, 4);
            BtnJogCw.Name = "BtnJogCw";
            BtnJogCw.Size = new Size(68, 41);
            BtnJogCw.TabIndex = 7;
            BtnJogCw.Type = AntdUI.TTypeMini.Success;
            BtnJogCw.MouseDown += BtnJogCw_MouseDown;
            BtnJogCw.MouseUp += BtnJogCw_MouseUp;
            // 
            // BtnServoReset
            // 
            BtnServoReset.AutoSizeMode = AntdUI.TAutoSize.Auto;
            BtnServoReset.IconSvg = "ReloadOutlined";
            BtnServoReset.Location = new Point(48, 104);
            BtnServoReset.Margin = new Padding(3, 4, 3, 4);
            BtnServoReset.Name = "BtnServoReset";
            BtnServoReset.Size = new Size(79, 45);
            BtnServoReset.TabIndex = 6;
            BtnServoReset.Text = "复位";
            BtnServoReset.Type = AntdUI.TTypeMini.Success;
            BtnServoReset.Click += BtnServoReset_Click;
            // 
            // BtnServoStop
            // 
            BtnServoStop.AutoSizeMode = AntdUI.TAutoSize.Auto;
            BtnServoStop.IconSvg = "PauseOutlined";
            BtnServoStop.Location = new Point(48, 47);
            BtnServoStop.Margin = new Padding(3, 4, 3, 4);
            BtnServoStop.Name = "BtnServoStop";
            BtnServoStop.Size = new Size(79, 45);
            BtnServoStop.TabIndex = 5;
            BtnServoStop.Text = "停止";
            BtnServoStop.Type = AntdUI.TTypeMini.Error;
            BtnServoStop.Click += BtnServoStop_Click;
            // 
            // BtnServoDisEnable
            // 
            BtnServoDisEnable.AutoSizeMode = AntdUI.TAutoSize.Auto;
            BtnServoDisEnable.IconSvg = "PoweroffOutlined";
            BtnServoDisEnable.Location = new Point(159, 47);
            BtnServoDisEnable.Margin = new Padding(3, 4, 3, 4);
            BtnServoDisEnable.Name = "BtnServoDisEnable";
            BtnServoDisEnable.Size = new Size(79, 45);
            BtnServoDisEnable.TabIndex = 4;
            BtnServoDisEnable.Text = "失能";
            BtnServoDisEnable.Type = AntdUI.TTypeMini.Warn;
            BtnServoDisEnable.Click += BtnServoDisEnable_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(panel3);
            panel7.Controls.Add(PanAxisAct);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(293, 0);
            panel7.Margin = new Padding(3, 4, 3, 4);
            panel7.Name = "panel7";
            panel7.Size = new Size(763, 660);
            panel7.TabIndex = 52;
            // 
            // panel3
            // 
            panel3.ArrowSize = 10;
            panel3.Controls.Add(tag13);
            panel3.Controls.Add(tabControl1);
            panel3.Controls.Add(divider2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 280);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Radius = 10;
            panel3.Shadow = 24;
            panel3.ShadowOpacity = 0.18F;
            panel3.ShadowOpacityAnimation = true;
            panel3.Size = new Size(763, 380);
            panel3.TabIndex = 51;
            // 
            // tag13
            // 
            tag13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag13.Font = new Font("Microsoft YaHei UI", 8F);
            tag13.LocalizationText = "Badge.{id}";
            tag13.Location = new Point(2794, 118);
            tag13.Margin = new Padding(3, 4, 3, 4);
            tag13.Name = "tag13";
            tag13.Size = new Size(68, 25);
            tag13.TabIndex = 49;
            tag13.Text = "位置锁存";
            tag13.Type = AntdUI.TTypeMini.Primary;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(TpGoHome);
            tabControl1.Controls.Add(TpMoveAbs);
            tabControl1.Controls.Add(TpSoftLand);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(TpFlexibleForce);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.HotTrack = true;
            tabControl1.Location = new Point(30, 31);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(703, 319);
            tabControl1.TabIndex = 49;
            // 
            // TpGoHome
            // 
            TpGoHome.Controls.Add(alert11);
            TpGoHome.Controls.Add(panHomeMethod);
            TpGoHome.Controls.Add(selHomeControler);
            TpGoHome.Controls.Add(tag6);
            TpGoHome.Controls.Add(inHomeSCurrent);
            TpGoHome.Controls.Add(tag5);
            TpGoHome.Controls.Add(inHomeStime);
            TpGoHome.Controls.Add(tag3);
            TpGoHome.Controls.Add(inHomeACC);
            TpGoHome.Controls.Add(tag2);
            TpGoHome.Controls.Add(inHomeVL);
            TpGoHome.Controls.Add(tag1);
            TpGoHome.Controls.Add(inHomeVH);
            TpGoHome.Location = new Point(4, 29);
            TpGoHome.Margin = new Padding(3, 4, 3, 4);
            TpGoHome.Name = "TpGoHome";
            TpGoHome.Padding = new Padding(3, 4, 3, 4);
            TpGoHome.Size = new Size(695, 286);
            TpGoHome.TabIndex = 0;
            TpGoHome.Text = "回零";
            TpGoHome.UseVisualStyleBackColor = true;
            // 
            // alert11
            // 
            alert11.BorderWidth = 1F;
            alert11.Icon = AntdUI.TType.Info;
            alert11.Location = new Point(363, 84);
            alert11.Margin = new Padding(3, 4, 3, 4);
            alert11.Name = "alert11";
            alert11.Size = new Size(264, 111);
            alert11.TabIndex = 59;
            alert11.Text = "正在加载图片...";
            alert11.TextTitle = "示意图";
            // 
            // panHomeMethod
            // 
            panHomeMethod.BackColor = Color.Transparent;
            panHomeMethod.Controls.Add(BtnServoGoHome);
            panHomeMethod.Controls.Add(selHomeMethod);
            panHomeMethod.Location = new Point(6, 44);
            panHomeMethod.Margin = new Padding(3, 4, 3, 4);
            panHomeMethod.Name = "panHomeMethod";
            panHomeMethod.Size = new Size(240, 39);
            panHomeMethod.TabIndex = 4;
            // 
            // BtnServoGoHome
            // 
            BtnServoGoHome.Dock = DockStyle.Right;
            BtnServoGoHome.IconSvg = "DoubleRightOutlined";
            BtnServoGoHome.JoinLeft = true;
            BtnServoGoHome.Location = new Point(190, 0);
            BtnServoGoHome.Margin = new Padding(3, 4, 3, 4);
            BtnServoGoHome.Name = "BtnServoGoHome";
            BtnServoGoHome.Size = new Size(50, 39);
            BtnServoGoHome.TabIndex = 3;
            BtnServoGoHome.Type = AntdUI.TTypeMini.Primary;
            BtnServoGoHome.Click += BtnServoGoHome_Click;
            // 
            // selHomeMethod
            // 
            selHomeMethod.AllowClear = true;
            selHomeMethod.Dock = DockStyle.Fill;
            selHomeMethod.JoinRight = true;
            selHomeMethod.LocalizationPlaceholderText = "Select.{id}";
            selHomeMethod.Location = new Point(0, 0);
            selHomeMethod.Margin = new Padding(3, 4, 3, 4);
            selHomeMethod.Name = "selHomeMethod";
            selHomeMethod.PlaceholderText = "回零方式未选择";
            selHomeMethod.Size = new Size(240, 39);
            selHomeMethod.TabIndex = 2;
            // 
            // selHomeControler
            // 
            selHomeControler.DropDownArrow = true;
            selHomeControler.Items.AddRange(new object[] { "伺服内部常规回零", "主控调用伺服内部回零", "主控控制伺服外部回零" });
            selHomeControler.List = true;
            selHomeControler.ListAutoWidth = true;
            selHomeControler.LocalizationPlaceholderText = "Select.{id}";
            selHomeControler.Location = new Point(6, 7);
            selHomeControler.Margin = new Padding(3, 4, 3, 4);
            selHomeControler.Name = "selHomeControler";
            selHomeControler.PlaceholderText = "选择回零控制器";
            selHomeControler.Placement = AntdUI.TAlignFrom.TR;
            selHomeControler.SelectedIndex = 0;
            selHomeControler.SelectedValue = "伺服内部常规回零";
            selHomeControler.Size = new Size(240, 41);
            selHomeControler.TabIndex = 3;
            selHomeControler.Text = "伺服内部常规回零";
            selHomeControler.SelectedIndexChanged += selHomeControler_SelectedIndexChanged;
            // 
            // tag6
            // 
            tag6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag6.Font = new Font("Microsoft YaHei UI", 8F);
            tag6.LocalizationText = "Badge.{id}";
            tag6.Location = new Point(222, 140);
            tag6.Margin = new Padding(3, 4, 3, 4);
            tag6.Name = "tag6";
            tag6.Size = new Size(68, 25);
            tag6.TabIndex = 57;
            tag6.Text = "堵转电流";
            tag6.Type = AntdUI.TTypeMini.Primary;
            // 
            // inHomeSCurrent
            // 
            inHomeSCurrent.DecimalPlaces = 1;
            inHomeSCurrent.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inHomeSCurrent.Location = new Point(159, 140);
            inHomeSCurrent.Margin = new Padding(3, 4, 3, 4);
            inHomeSCurrent.Name = "inHomeSCurrent";
            inHomeSCurrent.Size = new Size(103, 44);
            inHomeSCurrent.TabIndex = 58;
            inHomeSCurrent.Text = "500.0";
            inHomeSCurrent.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // tag5
            // 
            tag5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag5.Font = new Font("Microsoft YaHei UI", 8F);
            tag5.LocalizationText = "Badge.{id}";
            tag5.Location = new Point(222, 91);
            tag5.Margin = new Padding(3, 4, 3, 4);
            tag5.Name = "tag5";
            tag5.Size = new Size(68, 25);
            tag5.TabIndex = 55;
            tag5.Text = "堵转时间";
            tag5.Type = AntdUI.TTypeMini.Primary;
            // 
            // inHomeStime
            // 
            inHomeStime.DecimalPlaces = 1;
            inHomeStime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inHomeStime.Location = new Point(159, 91);
            inHomeStime.Margin = new Padding(3, 4, 3, 4);
            inHomeStime.Name = "inHomeStime";
            inHomeStime.Size = new Size(103, 44);
            inHomeStime.TabIndex = 56;
            inHomeStime.Text = "50.0";
            inHomeStime.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // tag3
            // 
            tag3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag3.Font = new Font("Microsoft YaHei UI", 8F);
            tag3.LocalizationText = "Badge.{id}";
            tag3.Location = new Point(73, 185);
            tag3.Margin = new Padding(3, 4, 3, 4);
            tag3.Name = "tag3";
            tag3.Size = new Size(68, 25);
            tag3.TabIndex = 53;
            tag3.Text = "加减速";
            tag3.Type = AntdUI.TTypeMini.Primary;
            // 
            // inHomeACC
            // 
            inHomeACC.DecimalPlaces = 1;
            inHomeACC.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inHomeACC.Location = new Point(15, 195);
            inHomeACC.Margin = new Padding(3, 4, 3, 4);
            inHomeACC.Name = "inHomeACC";
            inHomeACC.Size = new Size(103, 44);
            inHomeACC.TabIndex = 54;
            inHomeACC.Text = "10000.0";
            inHomeACC.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // tag2
            // 
            tag2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag2.Font = new Font("Microsoft YaHei UI", 8F);
            tag2.LocalizationText = "Badge.{id}";
            tag2.Location = new Point(73, 140);
            tag2.Margin = new Padding(3, 4, 3, 4);
            tag2.Name = "tag2";
            tag2.Size = new Size(68, 25);
            tag2.TabIndex = 51;
            tag2.Text = "回零低速";
            tag2.Type = AntdUI.TTypeMini.Primary;
            // 
            // inHomeVL
            // 
            inHomeVL.DecimalPlaces = 1;
            inHomeVL.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inHomeVL.Location = new Point(15, 144);
            inHomeVL.Margin = new Padding(3, 4, 3, 4);
            inHomeVL.Name = "inHomeVL";
            inHomeVL.Size = new Size(103, 44);
            inHomeVL.TabIndex = 52;
            inHomeVL.Text = "5.0";
            inHomeVL.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // tag1
            // 
            tag1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag1.Font = new Font("Microsoft YaHei UI", 8F);
            tag1.LocalizationText = "Badge.{id}";
            tag1.Location = new Point(73, 91);
            tag1.Margin = new Padding(3, 4, 3, 4);
            tag1.Name = "tag1";
            tag1.Size = new Size(68, 25);
            tag1.TabIndex = 49;
            tag1.Text = "回零高速";
            tag1.Type = AntdUI.TTypeMini.Primary;
            // 
            // inHomeVH
            // 
            inHomeVH.DecimalPlaces = 1;
            inHomeVH.Location = new Point(15, 91);
            inHomeVH.Margin = new Padding(3, 4, 3, 4);
            inHomeVH.Name = "inHomeVH";
            inHomeVH.Size = new Size(103, 44);
            inHomeVH.TabIndex = 50;
            inHomeVH.Text = "10.0";
            inHomeVH.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // TpMoveAbs
            // 
            TpMoveAbs.Controls.Add(table1);
            TpMoveAbs.Location = new Point(4, 29);
            TpMoveAbs.Margin = new Padding(3, 4, 3, 4);
            TpMoveAbs.Name = "TpMoveAbs";
            TpMoveAbs.Size = new Size(695, 286);
            TpMoveAbs.TabIndex = 1;
            TpMoveAbs.Text = "点位";
            TpMoveAbs.UseVisualStyleBackColor = true;
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table1.ContextMenuStrip = contextMenuStrip1;
            table1.Dock = DockStyle.Fill;
            table1.Font = new Font("Microsoft YaHei UI", 11F);
            table1.Gap = 12;
            table1.Location = new Point(0, 0);
            table1.Margin = new Padding(3, 4, 3, 4);
            table1.Name = "table1";
            table1.Radius = 6;
            table1.Size = new Size(695, 286);
            table1.TabIndex = 4;
            table1.CellDoubleClick += table1_CellDoubleClick;
            // 
            // TpSoftLand
            // 
            TpSoftLand.Controls.Add(tag15);
            TpSoftLand.Controls.Add(tag14);
            TpSoftLand.Controls.Add(panel6);
            TpSoftLand.Controls.Add(inPbPos);
            TpSoftLand.Controls.Add(inPtVel);
            TpSoftLand.Controls.Add(inPtPos);
            TpSoftLand.Controls.Add(inAcc);
            TpSoftLand.Controls.Add(inPpVel);
            TpSoftLand.Controls.Add(inPpPos);
            TpSoftLand.Controls.Add(pictureBox1);
            TpSoftLand.Location = new Point(4, 29);
            TpSoftLand.Margin = new Padding(3, 4, 3, 4);
            TpSoftLand.Name = "TpSoftLand";
            TpSoftLand.Size = new Size(695, 286);
            TpSoftLand.TabIndex = 2;
            TpSoftLand.Text = "软着陆";
            TpSoftLand.UseVisualStyleBackColor = true;
            // 
            // tag15
            // 
            tag15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag15.Font = new Font("Microsoft YaHei UI", 8F);
            tag15.LocalizationText = "Badge.{id}";
            tag15.Location = new Point(498, 205);
            tag15.Margin = new Padding(3, 4, 3, 4);
            tag15.Name = "tag15";
            tag15.Size = new Size(90, 25);
            tag15.TabIndex = 58;
            tag15.Text = "保压时间-ms";
            tag15.Type = AntdUI.TTypeMini.Primary;
            // 
            // tag14
            // 
            tag14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag14.Font = new Font("Microsoft YaHei UI", 8F);
            tag14.LocalizationText = "Badge.{id}";
            tag14.Location = new Point(384, 205);
            tag14.Margin = new Padding(3, 4, 3, 4);
            tag14.Name = "tag14";
            tag14.Size = new Size(68, 25);
            tag14.TabIndex = 57;
            tag14.Text = "压力-‰";
            tag14.Type = AntdUI.TTypeMini.Primary;
            // 
            // panel6
            // 
            panel6.BackColor = Color.Transparent;
            panel6.Controls.Add(inInstallTime);
            panel6.Controls.Add(inInstallPress);
            panel6.Controls.Add(BtnSoftLand);
            panel6.Location = new Point(294, 214);
            panel6.Margin = new Padding(4);
            panel6.Name = "panel6";
            panel6.Size = new Size(372, 56);
            panel6.TabIndex = 56;
            // 
            // inInstallTime
            // 
            inInstallTime.Dock = DockStyle.Fill;
            inInstallTime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inInstallTime.JoinLeft = true;
            inInstallTime.JoinRight = true;
            inInstallTime.Location = new Point(141, 0);
            inInstallTime.Margin = new Padding(3, 4, 3, 4);
            inInstallTime.Name = "inInstallTime";
            inInstallTime.Size = new Size(135, 56);
            inInstallTime.TabIndex = 31;
            inInstallTime.Text = "500";
            inInstallTime.TextAlign = HorizontalAlignment.Center;
            inInstallTime.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // inInstallPress
            // 
            inInstallPress.Dock = DockStyle.Left;
            inInstallPress.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inInstallPress.JoinRight = true;
            inInstallPress.Location = new Point(0, 0);
            inInstallPress.Margin = new Padding(3, 4, 3, 4);
            inInstallPress.Name = "inInstallPress";
            inInstallPress.Size = new Size(141, 56);
            inInstallPress.TabIndex = 30;
            inInstallPress.Text = "100";
            inInstallPress.TextAlign = HorizontalAlignment.Center;
            inInstallPress.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // BtnSoftLand
            // 
            BtnSoftLand.Dock = DockStyle.Right;
            BtnSoftLand.IconSvg = "DoubleRightOutlined";
            BtnSoftLand.JoinLeft = true;
            BtnSoftLand.Location = new Point(276, 0);
            BtnSoftLand.Margin = new Padding(3, 4, 3, 4);
            BtnSoftLand.Name = "BtnSoftLand";
            BtnSoftLand.Size = new Size(96, 56);
            BtnSoftLand.TabIndex = 8;
            BtnSoftLand.Type = AntdUI.TTypeMini.Info;
            BtnSoftLand.Click += BtnSoftLand_Click_1Async;
            // 
            // inPbPos
            // 
            inPbPos.BorderActive = SystemColors.ActiveCaptionText;
            inPbPos.BorderColor = Color.Brown;
            inPbPos.BorderHover = Color.Aqua;
            inPbPos.DecimalPlaces = 3;
            inPbPos.Location = new Point(591, 49);
            inPbPos.Margin = new Padding(3, 4, 3, 4);
            inPbPos.Name = "inPbPos";
            inPbPos.Size = new Size(89, 36);
            inPbPos.TabIndex = 55;
            inPbPos.Text = "0.000";
            // 
            // inPtVel
            // 
            inPtVel.BorderActive = SystemColors.ActiveCaptionText;
            inPtVel.BorderColor = Color.Brown;
            inPtVel.BorderHover = Color.Aqua;
            inPtVel.DecimalPlaces = 1;
            inPtVel.Location = new Point(474, 92);
            inPtVel.Margin = new Padding(3, 4, 3, 4);
            inPtVel.Name = "inPtVel";
            inPtVel.Size = new Size(89, 36);
            inPtVel.TabIndex = 54;
            inPtVel.Text = "20.0";
            inPtVel.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // inPtPos
            // 
            inPtPos.BorderActive = SystemColors.ActiveCaptionText;
            inPtPos.BorderColor = Color.Brown;
            inPtPos.BorderHover = Color.Aqua;
            inPtPos.DecimalPlaces = 3;
            inPtPos.Location = new Point(474, 49);
            inPtPos.Margin = new Padding(3, 4, 3, 4);
            inPtPos.Name = "inPtPos";
            inPtPos.Size = new Size(89, 36);
            inPtPos.TabIndex = 53;
            inPtPos.Text = "16.000";
            inPtPos.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // inAcc
            // 
            inAcc.BorderActive = SystemColors.ActiveCaptionText;
            inAcc.BorderColor = Color.Brown;
            inAcc.BorderHover = Color.Aqua;
            inAcc.DecimalPlaces = 1;
            inAcc.Location = new Point(357, 134);
            inAcc.Margin = new Padding(3, 4, 3, 4);
            inAcc.Name = "inAcc";
            inAcc.Size = new Size(89, 36);
            inAcc.TabIndex = 52;
            inAcc.Text = "30000.0";
            inAcc.Value = new decimal(new int[] { 30000, 0, 0, 0 });
            // 
            // inPpVel
            // 
            inPpVel.BorderActive = SystemColors.ActiveCaptionText;
            inPpVel.BorderColor = Color.Brown;
            inPpVel.BorderHover = Color.Aqua;
            inPpVel.DecimalPlaces = 1;
            inPpVel.Location = new Point(357, 92);
            inPpVel.Margin = new Padding(3, 4, 3, 4);
            inPpVel.Name = "inPpVel";
            inPpVel.Size = new Size(89, 36);
            inPpVel.TabIndex = 51;
            inPpVel.Text = "1000.0";
            inPpVel.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // inPpPos
            // 
            inPpPos.BorderActive = SystemColors.ActiveCaptionText;
            inPpPos.BorderColor = Color.Brown;
            inPpPos.BorderHover = Color.Aqua;
            inPpPos.DecimalPlaces = 3;
            inPpPos.Location = new Point(357, 49);
            inPpPos.Margin = new Padding(3, 4, 3, 4);
            inPpPos.Name = "inPpPos";
            inPpPos.Size = new Size(89, 36);
            inPpPos.TabIndex = 50;
            inPpPos.Text = "15.000";
            inPpPos.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(695, 286);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(3, 4, 3, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(695, 286);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "自动往复";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(3, 4, 3, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(695, 286);
            tabPage2.TabIndex = 4;
            tabPage2.Text = "力控标定";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel8);
            tabPage3.Controls.Add(panel1);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(695, 286);
            tabPage3.TabIndex = 5;
            tabPage3.Text = "SDO读写";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            panel8.BackColor = Color.Transparent;
            panel8.Controls.Add(BtnRead);
            panel8.Controls.Add(tag10);
            panel8.Controls.Add(iRdata);
            panel8.Controls.Add(tag11);
            panel8.Controls.Add(iRselet);
            panel8.Controls.Add(tag17);
            panel8.Controls.Add(tag16);
            panel8.Controls.Add(iRsubAdr);
            panel8.Controls.Add(iRadr);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 80);
            panel8.Margin = new Padding(4);
            panel8.Name = "panel8";
            panel8.Size = new Size(695, 80);
            panel8.TabIndex = 59;
            // 
            // BtnRead
            // 
            BtnRead.Dock = DockStyle.Fill;
            BtnRead.IconSvg = "CloudUploadOutlined";
            BtnRead.JoinLeft = true;
            BtnRead.JoinRight = true;
            BtnRead.Location = new Point(289, 0);
            BtnRead.Margin = new Padding(3, 4, 3, 4);
            BtnRead.Name = "BtnRead";
            BtnRead.Size = new Size(293, 80);
            BtnRead.TabIndex = 70;
            BtnRead.Text = "读取";
            BtnRead.Type = AntdUI.TTypeMini.Info;
            BtnRead.Click += BtnRead_Click;
            // 
            // tag10
            // 
            tag10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag10.Font = new Font("Microsoft YaHei UI", 8F);
            tag10.LocalizationText = "Badge.{id}";
            tag10.Location = new Point(645, 0);
            tag10.Margin = new Padding(3, 4, 3, 4);
            tag10.Name = "tag10";
            tag10.Size = new Size(47, 25);
            tag10.TabIndex = 69;
            tag10.Text = "数据";
            tag10.Type = AntdUI.TTypeMini.Primary;
            // 
            // iRdata
            // 
            iRdata.Dock = DockStyle.Right;
            iRdata.JoinLeft = true;
            iRdata.Location = new Point(582, 0);
            iRdata.Name = "iRdata";
            iRdata.ReadOnly = true;
            iRdata.Size = new Size(113, 80);
            iRdata.TabIndex = 68;
            iRdata.Text = "500";
            // 
            // tag11
            // 
            tag11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag11.Font = new Font("Microsoft YaHei UI", 8F);
            tag11.LocalizationText = "Badge.{id}";
            tag11.Location = new Point(242, 0);
            tag11.Margin = new Padding(3, 4, 3, 4);
            tag11.Name = "tag11";
            tag11.Size = new Size(47, 25);
            tag11.TabIndex = 64;
            tag11.Text = "类型";
            tag11.Type = AntdUI.TTypeMini.Primary;
            // 
            // iRselet
            // 
            iRselet.AllowClear = true;
            iRselet.Dock = DockStyle.Left;
            iRselet.Items.AddRange(new object[] { "Byte8", "Short16", "Int32" });
            iRselet.JoinLeft = true;
            iRselet.JoinRight = true;
            iRselet.LocalizationPlaceholderText = "Select.{id}";
            iRselet.Location = new Point(189, 0);
            iRselet.Margin = new Padding(3, 4, 3, 4);
            iRselet.Name = "iRselet";
            iRselet.PlaceholderText = "";
            iRselet.SelectedIndex = 1;
            iRselet.SelectedValue = "Short16";
            iRselet.Size = new Size(100, 80);
            iRselet.TabIndex = 67;
            iRselet.Text = "Short16";
            // 
            // tag17
            // 
            tag17.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag17.Font = new Font("Microsoft YaHei UI", 8F);
            tag17.LocalizationText = "Badge.{id}";
            tag17.Location = new Point(62, 0);
            tag17.Margin = new Padding(3, 4, 3, 4);
            tag17.Name = "tag17";
            tag17.Size = new Size(47, 25);
            tag17.TabIndex = 61;
            tag17.Text = "地址";
            tag17.Type = AntdUI.TTypeMini.Primary;
            // 
            // tag16
            // 
            tag16.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag16.Font = new Font("Microsoft YaHei UI", 8F);
            tag16.LocalizationText = "Badge.{id}";
            tag16.Location = new Point(142, 0);
            tag16.Margin = new Padding(3, 4, 3, 4);
            tag16.Name = "tag16";
            tag16.Size = new Size(47, 25);
            tag16.TabIndex = 62;
            tag16.Text = "子地址";
            tag16.Type = AntdUI.TTypeMini.Primary;
            // 
            // iRsubAdr
            // 
            iRsubAdr.Dock = DockStyle.Left;
            iRsubAdr.JoinLeft = true;
            iRsubAdr.JoinRight = true;
            iRsubAdr.Location = new Point(109, 0);
            iRsubAdr.Name = "iRsubAdr";
            iRsubAdr.Size = new Size(80, 80);
            iRsubAdr.TabIndex = 66;
            iRsubAdr.Text = "0x0";
            // 
            // iRadr
            // 
            iRadr.Dock = DockStyle.Left;
            iRadr.JoinRight = true;
            iRadr.Location = new Point(0, 0);
            iRadr.Name = "iRadr";
            iRadr.Size = new Size(109, 80);
            iRadr.TabIndex = 65;
            iRadr.Text = "0x5018";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(tag9);
            panel1.Controls.Add(iWdata);
            panel1.Controls.Add(tag8);
            panel1.Controls.Add(tag7);
            panel1.Controls.Add(tag4);
            panel1.Controls.Add(iWselet);
            panel1.Controls.Add(iWsubAdr);
            panel1.Controls.Add(iWadr);
            panel1.Controls.Add(BtnW);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(695, 80);
            panel1.TabIndex = 58;
            // 
            // tag9
            // 
            tag9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag9.Font = new Font("Microsoft YaHei UI", 8F);
            tag9.LocalizationText = "Badge.{id}";
            tag9.Location = new Point(355, 0);
            tag9.Margin = new Padding(3, 4, 3, 4);
            tag9.Name = "tag9";
            tag9.Size = new Size(47, 25);
            tag9.TabIndex = 66;
            tag9.Text = "数据";
            tag9.Type = AntdUI.TTypeMini.Primary;
            // 
            // iWdata
            // 
            iWdata.Dock = DockStyle.Fill;
            iWdata.JoinLeft = true;
            iWdata.JoinRight = true;
            iWdata.Location = new Point(289, 0);
            iWdata.Name = "iWdata";
            iWdata.Size = new Size(113, 80);
            iWdata.TabIndex = 65;
            iWdata.Text = "500";
            // 
            // tag8
            // 
            tag8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag8.Font = new Font("Microsoft YaHei UI", 8F);
            tag8.LocalizationText = "Badge.{id}";
            tag8.Location = new Point(242, 0);
            tag8.Margin = new Padding(3, 4, 3, 4);
            tag8.Name = "tag8";
            tag8.Size = new Size(47, 25);
            tag8.TabIndex = 63;
            tag8.Text = "类型";
            tag8.Type = AntdUI.TTypeMini.Primary;
            // 
            // tag7
            // 
            tag7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag7.Font = new Font("Microsoft YaHei UI", 8F);
            tag7.LocalizationText = "Badge.{id}";
            tag7.Location = new Point(142, 0);
            tag7.Margin = new Padding(3, 4, 3, 4);
            tag7.Name = "tag7";
            tag7.Size = new Size(47, 25);
            tag7.TabIndex = 62;
            tag7.Text = "子地址";
            tag7.Type = AntdUI.TTypeMini.Primary;
            // 
            // tag4
            // 
            tag4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag4.Font = new Font("Microsoft YaHei UI", 8F);
            tag4.LocalizationText = "Badge.{id}";
            tag4.Location = new Point(62, 4);
            tag4.Margin = new Padding(3, 4, 3, 4);
            tag4.Name = "tag4";
            tag4.Size = new Size(47, 25);
            tag4.TabIndex = 61;
            tag4.Text = "地址";
            tag4.Type = AntdUI.TTypeMini.Primary;
            // 
            // iWselet
            // 
            iWselet.AllowClear = true;
            iWselet.Dock = DockStyle.Left;
            iWselet.Items.AddRange(new object[] { "Byte8", "Short16", "Int32" });
            iWselet.JoinLeft = true;
            iWselet.JoinRight = true;
            iWselet.LocalizationPlaceholderText = "Select.{id}";
            iWselet.Location = new Point(189, 0);
            iWselet.Margin = new Padding(3, 4, 3, 4);
            iWselet.Name = "iWselet";
            iWselet.PlaceholderText = "";
            iWselet.SelectedIndex = 1;
            iWselet.SelectedValue = "Short16";
            iWselet.Size = new Size(100, 80);
            iWselet.TabIndex = 63;
            iWselet.Text = "Short16";
            // 
            // iWsubAdr
            // 
            iWsubAdr.Dock = DockStyle.Left;
            iWsubAdr.JoinLeft = true;
            iWsubAdr.JoinRight = true;
            iWsubAdr.Location = new Point(109, 0);
            iWsubAdr.Name = "iWsubAdr";
            iWsubAdr.Size = new Size(80, 80);
            iWsubAdr.TabIndex = 62;
            iWsubAdr.Text = "0x0";
            // 
            // iWadr
            // 
            iWadr.Dock = DockStyle.Left;
            iWadr.JoinRight = true;
            iWadr.Location = new Point(0, 0);
            iWadr.Name = "iWadr";
            iWadr.Size = new Size(109, 80);
            iWadr.TabIndex = 61;
            iWadr.Text = "0x5018";
            // 
            // BtnW
            // 
            BtnW.Dock = DockStyle.Right;
            BtnW.IconSvg = "CloudDownloadOutlined";
            BtnW.JoinLeft = true;
            BtnW.Location = new Point(402, 0);
            BtnW.Margin = new Padding(3, 4, 3, 4);
            BtnW.Name = "BtnW";
            BtnW.Size = new Size(293, 80);
            BtnW.TabIndex = 8;
            BtnW.Text = "写入";
            BtnW.Type = AntdUI.TTypeMini.Info;
            BtnW.Click += BtnW_Click;
            // 
            // TpFlexibleForce
            // 
            TpFlexibleForce.Controls.Add(Table_FlexibleForce);
            TpFlexibleForce.Controls.Add(panel10);
            TpFlexibleForce.Controls.Add(panel9);
            TpFlexibleForce.Location = new Point(4, 29);
            TpFlexibleForce.Name = "TpFlexibleForce";
            TpFlexibleForce.Size = new Size(695, 286);
            TpFlexibleForce.TabIndex = 6;
            TpFlexibleForce.Text = "柔性力控";
            TpFlexibleForce.UseVisualStyleBackColor = true;
            // 
            // Table_FlexibleForce
            // 
            Table_FlexibleForce.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            Table_FlexibleForce.BorderCellWidth = 2F;
            Table_FlexibleForce.Bordered = true;
            Table_FlexibleForce.BorderWidth = 3F;
            Table_FlexibleForce.CellImpactHeight = false;
            Table_FlexibleForce.Dock = DockStyle.Fill;
            Table_FlexibleForce.EditMode = AntdUI.TEditMode.DoubleClick;
            Table_FlexibleForce.EditSelection = AntdUI.TEditSelection.All;
            Table_FlexibleForce.Gap = 12;
            Table_FlexibleForce.Location = new Point(0, 32);
            Table_FlexibleForce.Name = "Table_FlexibleForce";
            Table_FlexibleForce.Radius = 6;
            Table_FlexibleForce.Size = new Size(695, 197);
            Table_FlexibleForce.TabIndex = 5;
            Table_FlexibleForce.MouseDown += Table_FlexibleForce_MouseDown;
            // 
            // panel10
            // 
            panel10.Controls.Add(Tv_Error);
            panel10.Controls.Add(Tv_Status);
            panel10.Controls.Add(Tv_ActForce);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(0, 0);
            panel10.Name = "panel10";
            panel10.Size = new Size(695, 32);
            panel10.TabIndex = 4;
            // 
            // Tv_Error
            // 
            Tv_Error.AutoSizeMode = AntdUI.TAutoSize.Auto;
            Tv_Error.Bold = true;
            Tv_Error.Color = Color.Red;
            Tv_Error.Dock = DockStyle.Left;
            Tv_Error.Font = new Font("Microsoft YaHei UI", 8F);
            Tv_Error.Label = "预留故障代码";
            Tv_Error.Location = new Point(305, 0);
            Tv_Error.Name = "Tv_Error";
            Tv_Error.Radius = 0;
            Tv_Error.Size = new Size(138, 28);
            Tv_Error.TabIndex = 3;
            Tv_Error.Text = "0000";
            // 
            // Tv_Status
            // 
            Tv_Status.AutoSizeMode = AntdUI.TAutoSize.Auto;
            Tv_Status.Bold = true;
            Tv_Status.Dock = DockStyle.Left;
            Tv_Status.Font = new Font("Microsoft YaHei UI", 8F);
            Tv_Status.Label = "状态反馈5010.3";
            Tv_Status.Location = new Point(142, 0);
            Tv_Status.Name = "Tv_Status";
            Tv_Status.Radius = 0;
            Tv_Status.Size = new Size(163, 28);
            Tv_Status.TabIndex = 2;
            Tv_Status.Text = "0-正常";
            // 
            // Tv_ActForce
            // 
            Tv_ActForce.AutoSizeMode = AntdUI.TAutoSize.Auto;
            Tv_ActForce.Bold = true;
            Tv_ActForce.Color = Color.Blue;
            Tv_ActForce.Dock = DockStyle.Left;
            Tv_ActForce.Font = new Font("Microsoft YaHei UI", 8F);
            Tv_ActForce.Label = "力反馈5016";
            Tv_ActForce.Location = new Point(0, 0);
            Tv_ActForce.Name = "Tv_ActForce";
            Tv_ActForce.Radius = 3;
            Tv_ActForce.Size = new Size(142, 28);
            Tv_ActForce.TabIndex = 1;
            Tv_ActForce.Text = "0000 g";
            // 
            // panel9
            // 
            panel9.Controls.Add(Btn_ForceStop);
            panel9.Controls.Add(Btn_ForceStart);
            panel9.Dock = DockStyle.Bottom;
            panel9.Location = new Point(0, 229);
            panel9.Name = "panel9";
            panel9.Size = new Size(695, 57);
            panel9.TabIndex = 0;
            // 
            // Btn_ForceStop
            // 
            Btn_ForceStop.Dock = DockStyle.Right;
            Btn_ForceStop.IconRatio = 1.5F;
            Btn_ForceStop.IconSvg = "PauseOutlined";
            Btn_ForceStop.Location = new Point(445, 0);
            Btn_ForceStop.Margin = new Padding(3, 4, 3, 4);
            Btn_ForceStop.Name = "Btn_ForceStop";
            Btn_ForceStop.Size = new Size(125, 57);
            Btn_ForceStop.TabIndex = 7;
            Btn_ForceStop.Text = "停止";
            Btn_ForceStop.Type = AntdUI.TTypeMini.Error;
            Btn_ForceStop.Click += Btn_ForceStop_Click;
            // 
            // Btn_ForceStart
            // 
            Btn_ForceStart.Dock = DockStyle.Right;
            Btn_ForceStart.IconRatio = 1.5F;
            Btn_ForceStart.IconSvg = "FallOutlined";
            Btn_ForceStart.Location = new Point(570, 0);
            Btn_ForceStart.Margin = new Padding(3, 4, 3, 4);
            Btn_ForceStart.Name = "Btn_ForceStart";
            Btn_ForceStart.Size = new Size(125, 57);
            Btn_ForceStart.TabIndex = 6;
            Btn_ForceStart.Text = "启动";
            Btn_ForceStart.Type = AntdUI.TTypeMini.Success;
            Btn_ForceStart.Click += Btn_ForceStart_Click;
            // 
            // divider2
            // 
            divider2.BackColor = Color.Transparent;
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(30, 30);
            divider2.Margin = new Padding(10, 9, 10, 9);
            divider2.Name = "divider2";
            divider2.Size = new Size(703, 1);
            divider2.TabIndex = 1;
            // 
            // PanAxisAct
            // 
            PanAxisAct.ArrowSize = 10;
            PanAxisAct.Controls.Add(tag12);
            PanAxisAct.Controls.Add(bdgIsMov);
            PanAxisAct.Controls.Add(PanStatus);
            PanAxisAct.Controls.Add(label8);
            PanAxisAct.Controls.Add(PanActErr);
            PanAxisAct.Controls.Add(label3);
            PanAxisAct.Controls.Add(PanActState);
            PanAxisAct.Controls.Add(label6);
            PanAxisAct.Controls.Add(PanActTorque);
            PanAxisAct.Controls.Add(label2);
            PanAxisAct.Controls.Add(PanActVel);
            PanAxisAct.Controls.Add(label1);
            PanAxisAct.Controls.Add(PanActMachine);
            PanAxisAct.Controls.Add(PanActPosition);
            PanAxisAct.Controls.Add(label5);
            PanAxisAct.Controls.Add(divider1);
            PanAxisAct.Controls.Add(label7);
            PanAxisAct.Dock = DockStyle.Top;
            PanAxisAct.Location = new Point(0, 0);
            PanAxisAct.Margin = new Padding(3, 4, 3, 4);
            PanAxisAct.Name = "PanAxisAct";
            PanAxisAct.Radius = 10;
            PanAxisAct.Shadow = 24;
            PanAxisAct.ShadowOpacity = 0.18F;
            PanAxisAct.ShadowOpacityAnimation = true;
            PanAxisAct.Size = new Size(763, 280);
            PanAxisAct.TabIndex = 16;
            // 
            // tag12
            // 
            tag12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag12.Font = new Font("Microsoft YaHei UI", 8F);
            tag12.LocalizationText = "Badge.{id}";
            tag12.Location = new Point(1384, 58);
            tag12.Margin = new Padding(3, 4, 3, 4);
            tag12.Name = "tag12";
            tag12.Size = new Size(68, 25);
            tag12.TabIndex = 49;
            tag12.Text = "位置锁存";
            tag12.Type = AntdUI.TTypeMini.Primary;
            // 
            // bdgIsMov
            // 
            bdgIsMov.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bdgIsMov.Location = new Point(478, 54);
            bdgIsMov.Margin = new Padding(3, 4, 3, 4);
            bdgIsMov.Name = "bdgIsMov";
            bdgIsMov.Size = new Size(73, 22);
            bdgIsMov.State = AntdUI.TState.Processing;
            bdgIsMov.TabIndex = 14;
            bdgIsMov.Text = "运动中";
            // 
            // PanStatus
            // 
            PanStatus.BackgroundImageLayout = ImageLayout.None;
            PanStatus.BorderWidth = 1F;
            PanStatus.Location = new Point(499, 140);
            PanStatus.Margin = new Padding(3, 4, 3, 4);
            PanStatus.Name = "PanStatus";
            PanStatus.Size = new Size(161, 31);
            PanStatus.TabIndex = 13;
            PanStatus.Text = "总线断开";
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Microsoft YaHei UI", 11F);
            label8.Location = new Point(361, 133);
            label8.Name = "label8";
            label8.Padding = new Padding(21, 9, 0, 0);
            label8.Size = new Size(138, 45);
            label8.TabIndex = 12;
            label8.Text = "状态信息：";
            // 
            // PanActErr
            // 
            PanActErr.BackgroundImageLayout = ImageLayout.None;
            PanActErr.BorderWidth = 1F;
            PanActErr.Location = new Point(499, 196);
            PanActErr.Margin = new Padding(3, 4, 3, 4);
            PanActErr.Name = "PanActErr";
            PanActErr.Size = new Size(161, 31);
            PanActErr.TabIndex = 11;
            PanActErr.Text = "0000";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft YaHei UI", 11F);
            label3.Location = new Point(361, 189);
            label3.Name = "label3";
            label3.Padding = new Padding(21, 9, 0, 0);
            label3.Size = new Size(138, 45);
            label3.TabIndex = 10;
            label3.Text = "错误代码：";
            // 
            // PanActState
            // 
            PanActState.BackgroundImageLayout = ImageLayout.None;
            PanActState.BorderWidth = 1F;
            PanActState.Location = new Point(499, 88);
            PanActState.Margin = new Padding(3, 4, 3, 4);
            PanActState.Name = "PanActState";
            PanActState.Size = new Size(161, 31);
            PanActState.TabIndex = 9;
            PanActState.Text = "888";
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Microsoft YaHei UI", 11F);
            label6.Location = new Point(361, 81);
            label6.Name = "label6";
            label6.Padding = new Padding(21, 9, 0, 0);
            label6.Size = new Size(138, 45);
            label6.TabIndex = 8;
            label6.Text = "实时状态：";
            // 
            // PanActTorque
            // 
            PanActTorque.BackgroundImageLayout = ImageLayout.None;
            PanActTorque.BorderWidth = 1F;
            PanActTorque.Location = new Point(171, 196);
            PanActTorque.Margin = new Padding(3, 4, 3, 4);
            PanActTorque.Name = "PanActTorque";
            PanActTorque.Size = new Size(161, 31);
            PanActTorque.TabIndex = 6;
            PanActTorque.Text = "66";
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft YaHei UI", 11F);
            label2.Location = new Point(33, 189);
            label2.Name = "label2";
            label2.Padding = new Padding(21, 9, 0, 0);
            label2.Size = new Size(138, 45);
            label2.TabIndex = 5;
            label2.Text = "实时扭矩：";
            // 
            // PanActVel
            // 
            PanActVel.BackgroundImageLayout = ImageLayout.None;
            PanActVel.BorderWidth = 1F;
            PanActVel.Location = new Point(171, 140);
            PanActVel.Margin = new Padding(3, 4, 3, 4);
            PanActVel.Name = "PanActVel";
            PanActVel.Size = new Size(161, 31);
            PanActVel.TabIndex = 7;
            PanActVel.Text = "0.00";
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft YaHei UI", 11F);
            label1.Location = new Point(33, 133);
            label1.Name = "label1";
            label1.Padding = new Padding(21, 9, 0, 0);
            label1.Size = new Size(138, 45);
            label1.TabIndex = 6;
            label1.Text = "实时速度：";
            // 
            // PanActMachine
            // 
            PanActMachine.BorderWidth = 1F;
            PanActMachine.Icon = AntdUI.TType.Success;
            PanActMachine.Location = new Point(166, 44);
            PanActMachine.Margin = new Padding(3, 4, 3, 4);
            PanActMachine.Name = "PanActMachine";
            PanActMachine.Size = new Size(285, 31);
            PanActMachine.TabIndex = 5;
            PanActMachine.Text = "OP_ENABLE";
            // 
            // PanActPosition
            // 
            PanActPosition.BackgroundImageLayout = ImageLayout.None;
            PanActPosition.BorderWidth = 1F;
            PanActPosition.Location = new Point(171, 88);
            PanActPosition.Margin = new Padding(3, 4, 3, 4);
            PanActPosition.Name = "PanActPosition";
            PanActPosition.Size = new Size(161, 31);
            PanActPosition.TabIndex = 4;
            PanActPosition.Text = "0.001";
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Microsoft YaHei UI", 11F);
            label5.Location = new Point(33, 81);
            label5.Name = "label5";
            label5.Padding = new Padding(21, 9, 0, 0);
            label5.Size = new Size(138, 45);
            label5.TabIndex = 3;
            label5.Text = "实时位置：";
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(30, 88);
            divider1.Margin = new Padding(10, 9, 10, 9);
            divider1.Name = "divider1";
            divider1.Size = new Size(703, 1);
            divider1.TabIndex = 1;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Top;
            label7.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold);
            label7.Location = new Point(30, 30);
            label7.Name = "label7";
            label7.Padding = new Padding(21, 0, 0, 0);
            label7.Size = new Size(703, 58);
            label7.TabIndex = 0;
            label7.Text = "轴状态";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmAxisMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1056, 660);
            Controls.Add(panel7);
            Controls.Add(panel5);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmAxisMain";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "FrmAxisMain";
            Load += FrmAxisMain_Load;
            panel4.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panEnable.ResumeLayout(false);
            panEnable.PerformLayout();
            panel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            TpGoHome.ResumeLayout(false);
            panHomeMethod.ResumeLayout(false);
            TpMoveAbs.ResumeLayout(false);
            TpSoftLand.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage3.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel1.ResumeLayout(false);
            TpFlexibleForce.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panel9.ResumeLayout(false);
            PanAxisAct.ResumeLayout(false);
            PanAxisAct.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label4;
        private AntdUI.Avatar avatar1;
        private AntdUI.Panel panel4;
        private Panel panel5;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 增加ToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem;
        private ToolStripMenuItem 重新加载ToolStripMenuItem;
        private AntdUI.Panel panEnable;
        private AntdUI.Button BtnServoEnable;
        private Panel panel2;
        private AntdUI.InputNumber inJogVel;
        private AntdUI.Button BtnJogCCW;
        private AntdUI.Button BtnJogCw;
        private AntdUI.Button BtnServoReset;
        private AntdUI.Button BtnServoStop;
        private AntdUI.Button BtnServoDisEnable;
        private AntdUI.InputNumber inputNumber1;
        private Panel panel7;
        private AntdUI.Panel panel3;
        private AntdUI.Tag tag13;
        private TabControl tabControl1;
        private TabPage TpGoHome;
        private AntdUI.Alert alert11;
        private Panel panHomeMethod;
        private AntdUI.Button BtnServoGoHome;
        private AntdUI.Select selHomeMethod;
        private AntdUI.Select selHomeControler;
        private AntdUI.Tag tag6;
        private AntdUI.InputNumber inHomeSCurrent;
        private AntdUI.Tag tag5;
        private AntdUI.InputNumber inHomeStime;
        private AntdUI.Tag tag3;
        private AntdUI.InputNumber inHomeACC;
        private AntdUI.Tag tag2;
        private AntdUI.InputNumber inHomeVL;
        private AntdUI.Tag tag1;
        private AntdUI.InputNumber inHomeVH;
        private TabPage TpMoveAbs;
        private AntdUI.Table table1;
        private TabPage TpSoftLand;
        private Panel panel6;
        private AntdUI.Tag tag15;
        private AntdUI.Tag tag14;
        private AntdUI.InputNumber inInstallTime;
        private AntdUI.InputNumber inInstallPress;
        private AntdUI.Button BtnSoftLand;
        private AntdUI.InputNumber inPbPos;
        private AntdUI.InputNumber inPtVel;
        private AntdUI.InputNumber inPtPos;
        private AntdUI.InputNumber inAcc;
        private AntdUI.InputNumber inPpVel;
        private AntdUI.InputNumber inPpPos;
        private PictureBox pictureBox1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private AntdUI.Divider divider2;
        private AntdUI.Panel PanAxisAct;
        private AntdUI.Tag tag12;
        private AntdUI.Badge bdgIsMov;
        private AntdUI.Alert PanStatus;
        private Label label8;
        private AntdUI.Alert PanActErr;
        private Label label3;
        private AntdUI.Alert PanActState;
        private Label label6;
        private AntdUI.Alert PanActTorque;
        private Label label2;
        private AntdUI.Alert PanActVel;
        private Label label1;
        private AntdUI.Alert PanActMachine;
        private AntdUI.Alert PanActPosition;
        private Label label5;
        private AntdUI.Divider divider1;
        private Label label7;
        private TabPage tabPage3;
        private Panel panel1;
        private AntdUI.Button BtnW;
        private AntdUI.Tag tag4;
        private AntdUI.Tag tag8;
        private AntdUI.Tag tag7;
        private Panel panel8;
        private AntdUI.Tag tag16;
        private AntdUI.Tag tag17;
        private AntdUI.Tag tag11;
        private AntdUI.Select iRselet;
        private AntdUI.Input iRsubAdr;
        private AntdUI.Input iRadr;
        private AntdUI.Select iWselet;
        private AntdUI.Input iWsubAdr;
        private AntdUI.Input iWadr;
        private AntdUI.Button BtnRead;
        private AntdUI.Tag tag10;
        private AntdUI.Tag tag9;
        private AntdUI.Input iWdata;
        private AntdUI.Input iRdata;
        private TabPage TpFlexibleForce;
        private Panel panel9;
        private AntdUI.Button Btn_ForceStop;
        private AntdUI.Button Btn_ForceStart;
        private AntdUI.Table Table_FlexibleForce;
        private Panel panel10;
        private AntdUI.Shield Tv_ActForce;
        private AntdUI.Shield Tv_Status;
        private AntdUI.Shield Tv_Error;
    }
}