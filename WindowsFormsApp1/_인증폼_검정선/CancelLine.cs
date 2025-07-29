using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1._인증폼_검정선
{
    public partial class CancelLine : Templete.BF
    {
        private Label cancelLine;

        public CancelLine()
        {
            InitializeComponent();
        }

        private void CancelLine_Load(object sender, EventArgs e)
        {
            cancelLine = new Label()
            {
                Dock = DockStyle.Fill,
                Parent = textBox1
            };
            cancelLine.Paint += Lbl_Paint;
        }

        private void Lbl_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            Random random = new Random();
            g.DrawLine(Pens.Black, random.Next(20), random.Next(textBox1.Height - 5), random.Next(textBox1.Width - 40, textBox1.Width - 20), random.Next(textBox1.Height - 5));
            g.DrawLine(Pens.Black, random.Next(20), random.Next(textBox1.Height - 5), random.Next(textBox1.Width - 40, textBox1.Width - 20), random.Next(textBox1.Height - 5));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cancelLine.Refresh();
        }
    }
}
