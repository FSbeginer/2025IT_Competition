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

namespace WindowsFormsApp1.View
{
    public partial class Calendar : BF
    {
        public DateTime Date { get; private set; }

        Label[] dayLabels = new Label[42];
        DateTime current = DateTime.Now.Date;
        DateTime max = DateTime.Now.Date.AddMonths(1).AddDays(-1);

        public Calendar()
        {
            InitializeComponent();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            addWeek();
            addLabels();
            Date = current;
            changeValues();
        }

        private void changeValues()
        {
            var first = new DateTime(current.Year, current.Month, 1);
            label1.Text = first.ToString("yyyy년 MM월");

            for (int i = 0; i < 42; i++)
            {
                var date = first.AddDays(-(int)first.DayOfWeek+i);

                dayLabels[i].Text = date.Day.ToString();

                if (date.Month != first.Month) dayLabels[i].Text = "";

                if (date < DateTime.Now.Date || date > max) dayLabels[i].ForeColor = Color.LightGray;
                else if ((int)date.DayOfWeek == 0) dayLabels[i].ForeColor = Color.Red;
                else if ((int)date.DayOfWeek == 6) dayLabels[i].ForeColor = Color.Blue;
                else dayLabels[i].ForeColor = Color.Black;

                dayLabels[i].Tag = date;
                if (date == Date) dayLabels[i].BackColor = Color.Yellow;
                else dayLabels[i].BackColor = Color.White;
            }
        }

        private void addLabels()
        {
            for (int i = 0; i < 42; i++)
            {
                dayLabels[i] = new Label
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                };
                dayLabels[i].Click += Calendar_Click;
                tableLayoutPanel2.Controls.Add(dayLabels[i]);
            }
        }

        private void Calendar_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            Date = (DateTime)lbl.Tag;

            if (lbl.ForeColor == Color.LightGray || lbl.Text=="") return;

            foreach (var item in dayLabels)
                item.BackColor = Color.White;
            lbl.BackColor = Color.Yellow;
        }

        private void addWeek()
        {
            string[] week = "일,월,화,수,목,금,토".Split(',');
            for (int i = 0; i < 7; i++)
            {
                Label lbl = new Label { Text = week[i], ForeColor = i == 0 ? Color.Red : i == 6 ? Color.SkyBlue : Color.Black, Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0,0,0,1), TextAlign = ContentAlignment.MiddleCenter };
                tableLayoutPanel1.Controls.Add(lbl);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(DateTime.Now.Month != current.Month) 
                current = current.AddMonths(-1);
            button1.Visible = false;
            button2.Visible = true;
            changeValues();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Month == current.Month)
                current = current.AddMonths(1);
            button1.Visible = true;
            button2.Visible = false;
            changeValues();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
