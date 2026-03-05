using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DH_Control_Demo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            AntdUI.Notification.success(this, "初始化成功 ", "运动控制卡初始化成功！", TAlignFrom.BR, Font);
            if (GVL.AxisMontions != null)
            {
                menuMain.Items[0].Sub.Clear();
                for (int i = 0; i < GVL.AxisMontions.Length; i++)
                {
                    menuMain.Items[0].Sub.Add(new MenuItem(GVL.AxisMontions[i].AxisAct.Name));
                    menuMain.Items[0].Sub[i].ID = i.ToString();
                }
                menuMain.Items[0].Select = true;
                menuMain.Items[0].Sub[0].Select = true;
            }
            menuMain_SelectChanged(sender, new MenuSelectEventArgs(menuMain.Items[0].Sub[0]));
        }

        AxisControl.FrmAxisMain? frmAxisMain;
        GripperControl.FrmGripperMain? frmGripperMain;
        private void menuMain_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            if (menuMain.Items[0].Select)
            {
                for (int i = 0; i < menuMain.Items[0].Sub.Count; i++)
                {
                    if (menuMain.Items[0].Sub[i].Select)
                    {
                        frmAxisMain ??= new AxisControl.FrmAxisMain();
                        ShowFrmInPanel(frmAxisMain);
                        frmAxisMain.Invoke(new Action(() => { frmAxisMain.OnFrmShow_Or_OnChangeAxisNem(i); }));
                    }
                }
            }
            else if (menuMain.Items[1].Select)
            {
                frmGripperMain ??= new GripperControl.FrmGripperMain();
                ShowFrmInPanel(frmGripperMain);
            }
        }
        private void ShowFrmInPanel(Form frm)
        {
            frm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(frm);
            frm.Show();
        }
    }
}
