using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Controls;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class SeatSelection : BF
    {
        public Division division { get; set; }
        public bool special { get; set; }
        public DateTime selDate { get; set; }
        public TimeSpan selTime { get; set; }

        public Information start, end;
        public string selSeat;
        public int carno;
        public int idx;
        
        SeatControl prev;
        Label[] lbl;

        public SeatSelection()
        {
            InitializeComponent();
        }

        private void SeatSelection_Load(object sender, EventArgs e)
        {


            if (carno == 0)
                idx = special ? 1 : division.d_carR.Value - division.d_carS.Value;
            else
                idx = carno;
            labelLoad();
            labelChange();
        }

        private void labelChange()
        {
            foreach (var item in tableLayoutPanel1.Controls)
            {
                var cc = item as SeatControl;
                using (var con = new SqlConnection("data source=.\\sqlexpress;initial catalog=ITTRAIN;user id=sa;password=1234;"))
                {
                    con.Open();
                    using (var cmd = new SqlCommand($"select r_seat from reservation where r_start = {start.i_no} and r_end = {end.i_no} and r_date = '{selDate.ToShortDateString()}' and r_time = '{selTime}'", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && reader.GetString(0) == cc.label1.Text)
                        {
                            cc.BackgroundImage = Properties.Resources.seat0;
                            cc.label1.ForeColor = Color.Gray;
                        }

                    }
                }
                if ((item as SeatControl).label1.Text == selSeat && carno == idx)
                {
                    (item as SeatControl).label1.ForeColor = Color.Red;
                    (item as SeatControl).BackgroundImage = Properties.Resources.seat2;
                }
            }
            label1.Text = "좌석선택("+(special?"특실":"일반실")+")";
            label2.Text = idx + "호차";
            button2.Text = idx + 1 + "호차";
            button3.Text = idx - 1 + "호차";
            button2.Visible = idx != (special ? division.d_carS.Value : division.d_carR.Value);
            button3.Visible = idx != (special ? 1: division.d_carR.Value-division.d_carS.Value);
            

            if (prev != null)
            {
                prev.label1.ForeColor = Color.Yellow;
                prev.BackgroundImage = Properties.Resources.seat1;
            }
        }

        private void labelLoad()
        {
            tableLayoutPanel1.Controls.Clear();
            for (int i = 0; i < (special ? 21 : 40); i++)
            {
                SeatControl cc = new SeatControl() { Dock = DockStyle.Fill };
                int row = i / (special ? 7 : 10);
                int col = i % (special ? 7 : 10);
                string txt = $"{(char)(65 + row)}{col + 1:D2}";
                if (row > 1) row++;
                cc.label1.Text = txt;
                cc.BackgroundImage = Properties.Resources.seat1;
                cc.label1.Click += Cc_Click;
                tableLayoutPanel1.Controls.Add(cc, col, row);
            }
            if (special)
            {
                tableLayoutPanel1.ColumnCount = 7;
                tableLayoutPanel1.RowCount = 4;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            idx++;
            labelChange();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            idx--;
            labelChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowPage(new Payment() {selDate = selDate, seat = selSeat, start = selTime, from = start, to = end , special = special, carno = carno, division = division});
        }

        private void Cc_Click(object sender, EventArgs e)
        {
            var cc =  (sender as Label).Parent as SeatControl;

            if (cc.label1.ForeColor == Color.Gray) return;

            cc.label1.ForeColor = Color.Red;
            cc.BackgroundImage = Properties.Resources.seat2;
            selSeat = cc.label1.Text;
            carno = idx;
            textBox1.Text = idx+"호차: "+selSeat;

            if (prev != null)
            {
                prev.label1.ForeColor = Color.Yellow;
                prev.BackgroundImage = Properties.Resources.seat1;
            }

            prev = cc;
        }
    }
}
