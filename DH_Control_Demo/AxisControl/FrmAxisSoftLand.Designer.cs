namespace DH_Control_Demo.AxisControl
{
    partial class FrmAxisSoftLand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAxisSoftLand));
            pictureBox1 = new PictureBox();
            inPbPos = new AntdUI.InputNumber();
            InInstallTime = new AntdUI.InputNumber();
            inInstallPress = new AntdUI.InputNumber();
            inPpPos = new AntdUI.InputNumber();
            inPtVel = new AntdUI.InputNumber();
            inPpVel = new AntdUI.InputNumber();
            inPtPos = new AntdUI.InputNumber();
            inAcc = new AntdUI.InputNumber();
            panel1 = new Panel();
            BtnSoftLand = new AntdUI.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(389, 580);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // inPbPos
            // 
            inPbPos.BorderActive = SystemColors.ActiveCaptionText;
            inPbPos.BorderColor = Color.Brown;
            inPbPos.BorderHover = Color.Aqua;
            inPbPos.DecimalPlaces = 3;
            inPbPos.Location = new Point(285, 506);
            inPbPos.Margin = new Padding(3, 4, 3, 4);
            inPbPos.Name = "inPbPos";
            inPbPos.Size = new Size(89, 36);
            inPbPos.TabIndex = 52;
            inPbPos.Text = "0.000";
            // 
            // InInstallTime
            // 
            InInstallTime.BorderActive = SystemColors.ActiveCaptionText;
            InInstallTime.BorderColor = Color.Brown;
            InInstallTime.BorderHover = Color.Aqua;
            InInstallTime.Location = new Point(285, 475);
            InInstallTime.Margin = new Padding(3, 4, 3, 4);
            InInstallTime.Name = "InInstallTime";
            InInstallTime.Size = new Size(89, 36);
            InInstallTime.TabIndex = 51;
            InInstallTime.Text = "100";
            InInstallTime.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // inInstallPress
            // 
            inInstallPress.BorderActive = SystemColors.ActiveCaptionText;
            inInstallPress.BorderColor = Color.Brown;
            inInstallPress.BorderHover = Color.Aqua;
            inInstallPress.DecimalPlaces = 2;
            inInstallPress.Location = new Point(285, 415);
            inInstallPress.Margin = new Padding(3, 4, 3, 4);
            inInstallPress.Name = "inInstallPress";
            inInstallPress.Size = new Size(89, 36);
            inInstallPress.TabIndex = 50;
            inInstallPress.Text = "500.00";
            inInstallPress.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // inPpPos
            // 
            inPpPos.BorderActive = SystemColors.ActiveCaptionText;
            inPpPos.BorderColor = Color.Brown;
            inPpPos.BorderHover = Color.Aqua;
            inPpPos.DecimalPlaces = 3;
            inPpPos.Location = new Point(285, 65);
            inPpPos.Margin = new Padding(3, 4, 3, 4);
            inPpPos.Name = "inPpPos";
            inPpPos.Size = new Size(89, 36);
            inPpPos.TabIndex = 45;
            inPpPos.Text = "15.000";
            inPpPos.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // inPtVel
            // 
            inPtVel.BorderActive = SystemColors.ActiveCaptionText;
            inPtVel.BorderColor = Color.Brown;
            inPtVel.BorderHover = Color.Aqua;
            inPtVel.DecimalPlaces = 1;
            inPtVel.Location = new Point(285, 381);
            inPtVel.Margin = new Padding(3, 4, 3, 4);
            inPtVel.Name = "inPtVel";
            inPtVel.Size = new Size(89, 36);
            inPtVel.TabIndex = 49;
            inPtVel.Text = "20.0";
            inPtVel.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // inPpVel
            // 
            inPpVel.BorderActive = SystemColors.ActiveCaptionText;
            inPpVel.BorderColor = Color.Brown;
            inPpVel.BorderHover = Color.Aqua;
            inPpVel.DecimalPlaces = 1;
            inPpVel.Location = new Point(285, 99);
            inPpVel.Margin = new Padding(3, 4, 3, 4);
            inPpVel.Name = "inPpVel";
            inPpVel.Size = new Size(89, 36);
            inPpVel.TabIndex = 46;
            inPpVel.Text = "1000.0";
            inPpVel.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // inPtPos
            // 
            inPtPos.BorderActive = SystemColors.ActiveCaptionText;
            inPtPos.BorderColor = Color.Brown;
            inPtPos.BorderHover = Color.Aqua;
            inPtPos.DecimalPlaces = 3;
            inPtPos.Location = new Point(285, 351);
            inPtPos.Margin = new Padding(3, 4, 3, 4);
            inPtPos.Name = "inPtPos";
            inPtPos.Size = new Size(89, 36);
            inPtPos.TabIndex = 48;
            inPtPos.Text = "16.000";
            inPtPos.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // inAcc
            // 
            inAcc.BorderActive = SystemColors.ActiveCaptionText;
            inAcc.BorderColor = Color.Brown;
            inAcc.BorderHover = Color.Aqua;
            inAcc.DecimalPlaces = 1;
            inAcc.Location = new Point(285, 141);
            inAcc.Margin = new Padding(3, 4, 3, 4);
            inAcc.Name = "inAcc";
            inAcc.Size = new Size(89, 36);
            inAcc.TabIndex = 47;
            inAcc.Text = "30000.0";
            inAcc.Value = new decimal(new int[] { 30000, 0, 0, 0 });
            // 
            // panel1
            // 
            panel1.Controls.Add(BtnSoftLand);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(389, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(393, 580);
            panel1.TabIndex = 53;
            // 
            // BtnSoftLand
            // 
            BtnSoftLand.AutoSizeMode = AntdUI.TAutoSize.Auto;
            BtnSoftLand.IconSvg = "VerticalAlignBottomOutlined";
            BtnSoftLand.Location = new Point(30, 491);
            BtnSoftLand.Margin = new Padding(3, 4, 3, 4);
            BtnSoftLand.Name = "BtnSoftLand";
            BtnSoftLand.Size = new Size(91, 51);
            BtnSoftLand.TabIndex = 49;
            BtnSoftLand.Text = "GO";
            BtnSoftLand.Type = AntdUI.TTypeMini.Success;
            BtnSoftLand.Click += BtnSoftLand_Click;
            // 
            // FrmAxisSoftLand
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 580);
            Controls.Add(panel1);
            Controls.Add(inPbPos);
            Controls.Add(InInstallTime);
            Controls.Add(inInstallPress);
            Controls.Add(inPpPos);
            Controls.Add(inPtVel);
            Controls.Add(inPpVel);
            Controls.Add(inPtPos);
            Controls.Add(inAcc);
            Controls.Add(pictureBox1);
            Name = "FrmAxisSoftLand";
            Text = "FrmAxisSoftLand";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private AntdUI.InputNumber inPbPos;
        private AntdUI.InputNumber InInstallTime;
        private AntdUI.InputNumber inInstallPress;
        private AntdUI.InputNumber inPpPos;
        private AntdUI.InputNumber inPtVel;
        private AntdUI.InputNumber inPpVel;
        private AntdUI.InputNumber inPtPos;
        private AntdUI.InputNumber inAcc;
        private Panel panel1;
        private AntdUI.Button BtnSoftLand;
    }
}