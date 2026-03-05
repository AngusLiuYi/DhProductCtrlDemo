
namespace DH_Control_Demo
{
    partial class FrmLoadCard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            sltCardType = new AntdUI.Select();
            label1 = new Label();
            btnOK = new AntdUI.Button();
            panTop = new Panel();
            panBot = new Panel();
            panMain = new Panel();
            table1 = new AntdUI.Table();
            contextMenuStrip1 = new ContextMenuStrip(components);
            删除ToolStripMenuItem = new ToolStripMenuItem();
            增加ToolStripMenuItem = new ToolStripMenuItem();
            保存ToolStripMenuItem = new ToolStripMenuItem();
            重新加载ToolStripMenuItem = new ToolStripMenuItem();
            panTop.SuspendLayout();
            panBot.SuspendLayout();
            panMain.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // sltCardType
            // 
            sltCardType.DropDownArrow = true;
            sltCardType.Items.AddRange(new object[] { "雷赛（DMC-E3032）", "固高（GEN-08）", "凌臣（PCI-M60）", "汇川（IMC30G-E）" });
            sltCardType.List = true;
            sltCardType.ListAutoWidth = true;
            sltCardType.LocalizationPlaceholderText = "Select.{id}";
            sltCardType.Location = new Point(141, 11);
            sltCardType.Margin = new Padding(2, 3, 2, 3);
            sltCardType.Name = "sltCardType";
            sltCardType.PlaceholderText = "选择加载的板卡";
            sltCardType.Placement = AntdUI.TAlignFrom.BR;
            sltCardType.Size = new Size(227, 35);
            sltCardType.TabIndex = 3;
            sltCardType.SelectedIndexChanged += sltCardType_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(19, 11);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(128, 20);
            label1.TabIndex = 5;
            label1.Text = "选择加载板卡：";
            // 
            // btnOK
            // 
            btnOK.AutoSizeMode = AntdUI.TAutoSize.Height;
            btnOK.BackExtend = "135, #6253E1, #04BEAE";
            btnOK.Location = new Point(522, 6);
            btnOK.Margin = new Padding(2, 3, 2, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(106, 42);
            btnOK.TabIndex = 6;
            btnOK.Text = "确定";
            btnOK.Type = AntdUI.TTypeMini.Primary;
            btnOK.Click += BtnOK_Click;
            // 
            // panTop
            // 
            panTop.Controls.Add(sltCardType);
            panTop.Controls.Add(label1);
            panTop.Dock = DockStyle.Top;
            panTop.Location = new Point(0, 0);
            panTop.Margin = new Padding(2, 3, 2, 3);
            panTop.Name = "panTop";
            panTop.Size = new Size(637, 53);
            panTop.TabIndex = 7;
            // 
            // panBot
            // 
            panBot.Controls.Add(btnOK);
            panBot.Dock = DockStyle.Bottom;
            panBot.Location = new Point(0, 436);
            panBot.Margin = new Padding(2, 3, 2, 3);
            panBot.Name = "panBot";
            panBot.Size = new Size(637, 56);
            panBot.TabIndex = 8;
            // 
            // panMain
            // 
            panMain.Controls.Add(table1);
            panMain.Dock = DockStyle.Fill;
            panMain.Location = new Point(0, 53);
            panMain.Margin = new Padding(2, 3, 2, 3);
            panMain.Name = "panMain";
            panMain.Size = new Size(637, 383);
            panMain.TabIndex = 9;
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table1.ContextMenuStrip = contextMenuStrip1;
            table1.Dock = DockStyle.Fill;
            table1.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            table1.Location = new Point(0, 0);
            table1.Margin = new Padding(2, 3, 2, 3);
            table1.Name = "table1";
            table1.Radius = 6;
            table1.Size = new Size(637, 383);
            table1.TabIndex = 3;
            table1.CellDoubleClick += table1_CellDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 删除ToolStripMenuItem, 增加ToolStripMenuItem, 保存ToolStripMenuItem, 重新加载ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(125, 92);
            // 
            // 删除ToolStripMenuItem
            // 
            删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            删除ToolStripMenuItem.Size = new Size(124, 22);
            删除ToolStripMenuItem.Text = "删除";
            删除ToolStripMenuItem.Click += Table1Delet;
            // 
            // 增加ToolStripMenuItem
            // 
            增加ToolStripMenuItem.Name = "增加ToolStripMenuItem";
            增加ToolStripMenuItem.Size = new Size(124, 22);
            增加ToolStripMenuItem.Text = "增加";
            增加ToolStripMenuItem.Click += Table1Add;
            // 
            // 保存ToolStripMenuItem
            // 
            保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            保存ToolStripMenuItem.Size = new Size(124, 22);
            保存ToolStripMenuItem.Text = "保存";
            保存ToolStripMenuItem.Click += Table1Save;
            // 
            // 重新加载ToolStripMenuItem
            // 
            重新加载ToolStripMenuItem.Name = "重新加载ToolStripMenuItem";
            重新加载ToolStripMenuItem.Size = new Size(124, 22);
            重新加载ToolStripMenuItem.Text = "重新加载";
            重新加载ToolStripMenuItem.Click += Table1Reload;
            // 
            // FrmLoadCard
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(637, 492);
            Controls.Add(panMain);
            Controls.Add(panTop);
            Controls.Add(panBot);
            Margin = new Padding(2, 3, 2, 3);
            Name = "FrmLoadCard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += FrmLoadCard_FormClosing;
            panTop.ResumeLayout(false);
            panTop.PerformLayout();
            panBot.ResumeLayout(false);
            panBot.PerformLayout();
            panMain.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Select sltCardType;
        private Label label1;
        private AntdUI.Button btnOK;
        private Panel panTop;
        private Panel panBot;
        private Panel panMain;
        private AntdUI.Table table1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 增加ToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem;
        private ToolStripMenuItem 重新加载ToolStripMenuItem;
    }
}