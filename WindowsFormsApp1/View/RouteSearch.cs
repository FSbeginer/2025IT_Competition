using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Controls;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class RouteSearch : BF
    {
        public Size ori;
        public RouteMap map;

        public RouteSearch()
        {
            InitializeComponent();
        }

        private void RouteSearch_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            ori = Size;
            panel1.Controls.Add(map = new RouteMap(comboBox1));

            if (loginUser != null)
            {
                btnStart.Visible = true;
                btnEnd.Visible = true;
                btnReservation.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Size != ori)
            {
                Thread th = new Thread(async () =>
                {
                    while (Size != ori)
                    {
                        Size = new Size(Size.Width - 1, Size.Height);
                        await Task.Delay(Size.Width % 2);
                    }
                    panel2.Visible = false;
                });
                th.Start();
            }
        }

        private void btnReservation_Click(object sender, EventArgs e)
        {
            using (var db = new Model.ITTRAINEntities1())
                ShowPage(new Reservation(db.Division.FirstOrDefault(x => x.d_no == comboBox1.SelectedIndex + 1)));
        }
    }

}
