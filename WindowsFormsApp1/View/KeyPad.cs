using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class KeyPad : BF
    {
        TextBox txt;
        public KeyPad(TextBox txt)
        {
            InitializeComponent();
            this.txt = txt;
        }

        private void KeyPad_Load(object sender, EventArgs e)
        {
            addNumber();
            Location = new Point(Location.X, Location.Y + 200);
        }

        private void addNumber()
        {
            tableLayoutPanel1.Controls.Clear();
            List<int> nums = new List<int> { 0,1,2,3,4,5,6,7,8,9,10,10};
            Random r= new Random();
            nums = nums.OrderBy(x => r.Next()).ToList();
            foreach (var item in nums)
            {
                Label lbl = new Label { TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0)};
                lbl.Text = item!=10? item.ToString() : "";
                lbl.BackColor  = item!=10? Color.White : Color.Gray;
                lbl.Click += Lbl_Click;
                tableLayoutPanel1.Controls.Add(lbl);
            }
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            Label lbl = sender  as Label;
            if (lbl.BackColor != Color.White) return;
            txt.Text = txt.Text + lbl.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txt.Text = "";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            txt.Text = txt.Text.Substring(0, txt.TextLength-1);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            addNumber();
        }
    }
}
