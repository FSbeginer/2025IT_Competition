using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Controls;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1._지도그리기_점연결
{
    public partial class 지도 : BF
    {
        public 지도()
        {
            InitializeComponent();
        }

        private void 지도_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            panel1.Controls.Add(new 맵(comboBox1));
        }
    }
}
