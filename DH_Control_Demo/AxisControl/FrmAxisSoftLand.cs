using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DH_Control_Demo.AxisControl
{
    public partial class FrmAxisSoftLand : Form
    {
        private readonly int _axisNum;
        public FrmAxisSoftLand(int axisNum)
        {
            InitializeComponent();
            _axisNum = axisNum;
        }

        private async void BtnSoftLand_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn=(AntdUI.Button)sender;
            btn.Loading = true;
            await GVL.AxisMontions[_axisNum].SoftLand_ServoExternal (
                ppPosition: (double)inPpPos.Value,
                ptPosition: (double)inPtPos.Value,
                pbPosition: (double)inPbPos.Value,
                ppVel: (double)inPpVel.Value,
                ptVel: (double)inPtVel.Value,
                pbVel: (double)inPpVel.Value,
                acc: (double)inAcc.Value,
                dec: (double)inAcc.Value,
                torqueLimit: (int)inInstallPress.Value,
                installTime: (int)InInstallTime.Value,
                func:null
                );
            btn.Loading = false;
        }
    }
}
