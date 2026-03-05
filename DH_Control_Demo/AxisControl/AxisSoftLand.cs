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
    public partial class AxisSoftLand : UserControl
    {
        int _axisNum;
        public AxisSoftLand(int axisNum)
        {
            InitializeComponent();
            _axisNum = axisNum;
        }
    }
}
