using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.__테스트폼
{
    public partial class Form1 : BF
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add(new DataGridViewImageColumn());
            dataGridView1.Rows.Add("",Properties.Resources.seat1);
        }

        private void ㅇ너ㅣ니라어ㅈ너랴ㅣㅇ너ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }
    }
}
