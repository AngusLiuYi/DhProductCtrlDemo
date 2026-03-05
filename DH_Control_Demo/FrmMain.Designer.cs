namespace DH_Control_Demo
{
    partial class FrmMain
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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            menuMain = new AntdUI.Menu();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.Dock = DockStyle.Left;
            menuItem1.IconSvg = "MailOutlined";
            menuItem2.Text = "Z轴";
            menuItem3.Text = "相石-DLAR-20-40-H1-R";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Text = "Cia402轴";
            menuItem4.Expand = false;
            menuItem4.IconSvg = "AppstoreOutlined";
            menuItem5.Text = "PGE-50-26-O-W";
            menuItem4.Sub.Add(menuItem5);
            menuItem4.Text = "电爪";
            menuItem6.Expand = false;
            menuItem6.IconSvg = "SettingOutlined";
            menuItem7.Text = "MCE-3G-01-030-C-O";
            menuItem6.Sub.Add(menuItem7);
            menuItem6.Text = "电缸";
            menuMain.Items.Add(menuItem1);
            menuMain.Items.Add(menuItem4);
            menuMain.Items.Add(menuItem6);
            menuMain.Location = new Point(0, 0);
            menuMain.Margin = new Padding(3, 4, 3, 4);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(210, 660);
            menuMain.TabIndex = 5;
            menuMain.Unique = true;
            menuMain.SelectChanged += menuMain_SelectChanged;
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(210, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1055, 660);
            panel1.TabIndex = 6;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1265, 660);
            Controls.Add(panel1);
            Controls.Add(menuMain);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmMain";
            Load += FrmMain_Load;
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private AntdUI.Menu menuMain;
    }
}