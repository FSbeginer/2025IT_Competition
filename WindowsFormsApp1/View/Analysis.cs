using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Metadata.Edm;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class Analysis : BF
    {
        List<int> values = new List<int>();
        int current = 0, max = 0, now = 0;
        Timer gradientTimer = new Timer { Interval = 1 };
        Timer aniTimer = new Timer { Interval = 1 };
        Random rand = new Random();
        Color color;

        public Analysis()
        {
            InitializeComponent();
        }

        private void Analysis_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            gradientTimer.Tick += GradientTimer_Tick;
            aniTimer.Tick += AniTimer_Tick;
        }

        private void AniTimer_Tick(object sender, EventArgs e)
        {
            var invisible = chart1.Series[1].Points.Last();
            invisible.YValues[0]--;

            chart1.Series[1].Points[current].YValues[0]++;

            if (chart1.Series[1].Points[current].YValues[0] >= values[current])
                current++;

            if (current >= values.Count) aniTimer.Stop();

            chart1.Invalidate();
        }

        private void GradientTimer_Tick(object sender, EventArgs e)
        {
            now += 5;
            panel1.Invalidate();
            if (now >= max)
            {
                gradientTimer.Stop();
                aniTimer.Start();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            chart1.Series[0].Points.Clear();

            using (var db = new Model.ITTRAINEntities1())
            {
                var list = db.Station.Where(x => x.s_code / 10000 == comboBox1.SelectedIndex + 1).ToList();
                foreach (var item in list)
                {
                    comboBox2.Items.Add(item.Information.stationname);
                }
                var reser = db.Reservation.Where(x => x.Station1.s_code / 10000 == comboBox1.SelectedIndex + 1).GroupBy(x => x.r_end ).OrderBy(x=>x.Key).ToList();
                int sum = 0;
                foreach (var group in reser)
                {
                    var glist = group.ToList();
                    int cnt = glist.Count;
                    chart1.Series[0].Points.AddXY(glist.Select(x => x.Station1.Information.stationname).First() + "(" + cnt + ")", cnt);
                    chart1.Series[0].Points.Last().Color = RandomColor();
                    sum += cnt;
                }
                label3.Text = "예약건수: " + sum;
            }

        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;

            HitTestResult result = chart1.HitTest(e.X, e.Y);

            if (result.ChartArea == chart1.ChartAreas[0] && result.ChartElementType == ChartElementType.DataPoint)
            {
                color = chart1.Series[0].Points[result.PointIndex].Color;
                panel1.Invalidate();
                addValues();
                now = 0; current = 0;
                gradientTimer.Start();
            }
        }
        
        private void addValues()
        {
            values.Clear();
            chart1.Series[1].Points.Clear();
            
            using (var db = new Model.ITTRAINEntities1())
            {
                int no = db.Information.Where(x => x.stationname == comboBox2.Text).Select(x => x.i_no).FirstOrDefault();
                var reser = db.Reservation.AsEnumerable().Where(x => x.Station1.s_code / 10000 == comboBox1.SelectedIndex + 1 && x.r_end == no).ToList();
                for (global::System.Int32 i = 0; i < 8; i++)
                {
                    //var group = reser.Where(x=>getage(x.User)>=i*10&&getage(x.User)<=(i*10+9));
                    chart1.Series[1].Points.AddXY($"{i*10}~{i*10+9}({reser.Count()})", 0);
                    chart1.Series[1].Points.Last().Color = GradientColor(color,10*i);
                    values.Add(reser.Count());
                }

                chart1.Series[1].Points.AddXY("", values.Sum());
                chart1.Series[1].Points.Last().Color = Color.Transparent;
                chart1.Series[1].Points.Last().IsVisibleInLegend = false;
            }
        }
        private Rectangle GetRect(int v)
        {
            int x = (int)(chart1.ChartAreas[v].Position.X * (chart1.Width / 100));
            int y = (int)(chart1.ChartAreas[v].Position.Y * (chart1.Height / 100));
            int w = (int)(chart1.ChartAreas[v].Position.Width * (chart1.Width / 100));
            int h = (int)(chart1.ChartAreas[v].Position.Height * (chart1.Height / 100));
            return new Rectangle(x, y, w, h);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle chart1 = GetRect(0);
            Rectangle chart2 = GetRect(1);

            int stx = chart1.X + chart1.Width / 2 + 20;
            int enx = chart2.X + chart2.Width / 2 + 20;

            max = enx - stx;
            int 보정 = 10;

            using (var path = new GraphicsPath())
            {
                path.AddPolygon(new Point[]
                {
                    new Point(stx+보정, chart1.Y+보정),
                    new Point(enx + 보정, chart2.Y+보정),
                    new Point(enx + 보정, chart2.Y+chart2.Height),
                    new Point(stx + 보정, chart1.Y+chart1.Height),
                });
                using (var brush = new LinearGradientBrush(panel1.ClientRectangle, color, Color.White, LinearGradientMode.Horizontal))
                {
                    g.Clip = new Region(new Rectangle(stx + 보정, chart1.Y, now + 보정, chart1.Height + 보정));
                    g.FillPath(brush, path);
                }

            }
        }

        private Color GradientColor(Color color, int v)
        {
            int r = Math.Max(color.R - v, 0);
            int g = Math.Max(color.G - v, 0);
            int b = Math.Max(color.B - v, 0);

            return Color.FromArgb(r, g, b);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            using (var db = new Model.ITTRAINEntities1())
            {
                int no = db.Information.Where(x => x.stationname == comboBox2.Text).Select(x => x.i_no).FirstOrDefault();
                var reser = db.Reservation.AsEnumerable().Where(x => x.Station1.s_code / 10000 == comboBox1.SelectedIndex + 1 && x.r_end == no).GroupBy(x => x.r_date.Value.Month).ToList();
                
                int sum = 0;
                foreach (var group in reser)
                {
                    chart1.Series[0].Points.AddXY(group.Key + "월(" + group.Count() + ")", group.Count());
                    chart1.Series[0].Points.Last().Color = RandomColor();
                    sum += group.Count();
                }
                label3.Text = "예약건수 : " + sum;
            }

        }
        private Color RandomColor()
        {
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }
}
