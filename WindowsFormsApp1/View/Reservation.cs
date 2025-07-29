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
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class Reservation : BF
    {
        Division division;
        DayLabel[] dayLabels;
        Label[] labels;
        bool click = false, click2 = false;
        int dx;

        private DateTime _selDate;

        public DateTime selDate
        {
            get { return _selDate; }
            set
            {
                _selDate = value;
                foreach (var item in dayLabels)
                {
                    if ((DateTime)item.Tag == _selDate)
                        item.BackColor = Color.Yellow;
                    else
                        item.BackColor = Color.White;
                }
                addLabels();
            }
        }

        private TimeSpan _selHour;

        public TimeSpan selHour
        {
            get { return _selHour; }
            set
            {
                _selHour = value;
                foreach (var item in labels)
                {
                    if ((TimeSpan)item.Tag == _selHour)
                        item.BackColor = Color.Yellow;
                    else
                        item.BackColor = Color.White;
                }
            }
        }

        public Reservation(Division division)
        {
            InitializeComponent();
            this.division = division;
        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            addDaylabels();

            selDate = DateTime.Now.Date;


            RouteSearch f = GetForm("노선검색") as RouteSearch;
            label2.Text = f.map.start.stationname + "→" + f.map.end.stationname;
            label3.Text = DateTime.Now.ToString("yyyy년 MM월 dd일(ddd) HH:mm");
        }

        private void addLabels()
        {
            panel2.Controls.Clear();
            bool isToday = selDate == DateTime.Now.Date;

            int start = isToday ? DateTime.Now.AddHours(1).Hour : division.d_sTime.Value.Hours, end = division.d_eTime.Value.Hours;
            labels = new Label[end - start + 1];

            for (int i = start; i <= end; i++)
            {
                labels[i - start] = new Label
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = i + "시",
                    Tag = TimeSpan.FromHours(i),
                    Size = new Size(50, panel2.Height),
                    Location = new Point(50 * (i - start), 0),
                };
                panel2.Controls.Add(labels[i - start]);

                AddEvent2(labels[i - start]);
                labels[i - start].Click += Reservation_Clickk;
            }


            selHour = TimeSpan.FromHours(start);
        }

        private void Reservation_Clickk(object sender, EventArgs e)
        {
            selHour = (TimeSpan)(sender as Label).Tag;
        }

        private void addDaylabels()
        {
            DateTime now = DateTime.Now.Date;
            DateTime max = DateTime.Now.Date.AddMonths(1);
            TimeSpan different = max - now;
            dayLabels = new DayLabel[different.Days];
            var current = now;
            var week = "일,월,화,수,목,금,토".Split(',');
            for (int i = 0; current != max; i++)
            {
                dayLabels[i] = new DayLabel()
                {
                    label1 = { Text = week[(int)current.DayOfWeek] },
                    label2 = { Text = current.Day.ToString() },
                    ForeColor = (int)current.DayOfWeek == 0 ? Color.Red : (int)current.DayOfWeek == 6 ? Color.SkyBlue : Color.Black,
                    Size = new Size(50, panel1.Height),
                    Location = new Point(50 * i, 0),
                    Tag = current,
                };
                panel1.Controls.Add(dayLabels[i]);
                current = current.AddDays(1);

                AddEvent(dayLabels[i].label1);
                AddEvent(dayLabels[i].label2);
                AddEvent(dayLabels[i]);
            }
        }

        private void AddEvent(Control c)
        {
            c.MouseDown += Reservation_MouseDown;
            c.MouseUp += Reservation_MouseUp;
            c.MouseMove += Reservation_MouseMove;
        }
        private void AddEvent2(Control c)
        {
            c.MouseDown += panel2_MouseDown;
            c.MouseUp += panel2_MouseUp;
            c.MouseMove += panel2_MouseMove;
        }

        private void Reservation_MouseMove(object sender, MouseEventArgs e)
        {
            if (click)
            {
                int dir = e.X - dx;
                if (IsOutOfRange(dir)) return;
                foreach (var item in dayLabels)
                {
                    item.Left += dir;
                }
            }
        }

        private bool IsOutOfRange(int dir)
        {
            return (dayLabels[0].Left + dir > 0 || dayLabels[dayLabels.Length - 1].Right + dir < panel1.Width);
        }
        private bool IsOutOfRange2(int dir)
        {
            return (labels[0].Left + dir > 0 || labels[labels.Length - 1].Right + dir < panel2.Width);
        }

        private void Reservation_MouseUp(object sender, MouseEventArgs e)
        {
            click = false;
        }

        private void Reservation_MouseDown(object sender, MouseEventArgs e)
        {
            dx = e.X;
            click = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var c = new Calendar();
            c.ShowDialog();
            selDate = c.Date;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dx = e.X;
            click2 = true;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            click2 = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = GetForm("노선검색") as RouteSearch;
            var list = new ScheduleList { route = division, startTime = selHour, start = f.map.start, end = f.map.end, selDate = selDate+selHour };
            ShowPage(list);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (click2)
            {
                int dir = e.X - dx;
                if (IsOutOfRange2(dir)) return;
                foreach (var item in labels)
                {
                    item.Left += dir;
                }
            }
        }
    }
}
