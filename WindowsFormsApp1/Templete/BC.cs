using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Templete
{
    public partial class BC : UserControl
    {
        public Color[] colors = { Color.FromArgb(255, 0, 0), Color.FromArgb(0, 100, 200), Color.FromArgb(150, 200, 230), Color.FromArgb(170, 200, 150), Color.FromArgb(0, 255, 0), Color.FromArgb(255, 165, 0), Color.FromArgb(190, 190, 190) };
        public BC()
        {
            InitializeComponent();
        }

        private void BC_Load(object sender, EventArgs e)
        {

        }
        public void MsgInfo(string msg) { MessageBox.Show(msg, "정보", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        public void MsgErr(string msg) { MessageBox.Show(msg, "정보", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
