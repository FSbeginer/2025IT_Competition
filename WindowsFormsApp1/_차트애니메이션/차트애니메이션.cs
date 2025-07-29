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
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1._차트애니메이션
{
    public partial class 차트애니메이션 : Form
    {
        List<int> values = new List<int>();
        int current = 0, max = 0, now = 0;
        Timer gradientTimer = new Timer { Interval = 1 };
        Timer aniTimer = new Timer { Interval = 1 };
        Random rand = new Random();
        Color color;

        public 차트애니메이션()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getData();
            gradientTimer.Tick += GradientTimer_Tick;
            aniTimer.Tick += AniTimer_Tick;
        }

        /// <summary>
        /// 그라데이션 애니메이션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 차트애니메이션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// 초기 데이터 세팅
        /// </summary>
        private void getData()
        {
            //초기화
            chart1.Series[0].Points.Clear();

            //시리즈1. 월별 데이터
            for (int i = 1; i <= 12; i++)
            {
                chart1.Series[0].Points.AddXY($"{i}월({i * 5}건)", i * 5);
                chart1.Series[0].Points.Last().Tag = i + "월";
                chart1.Series[0].Points.Last().Color = RandomColor();
            }
        }

        /// <summary>
        /// 랜덤 색상 가져오기
        /// </summary>
        /// <returns></returns>
        private Color RandomColor()
        {
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            // HitTest 차트에서 특정 좌표의 요소
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            // ex) chart1.HitTest(10, 10)의 ElementType이 DataPoint이다.
            if (result.ChartElementType == ChartElementType.DataPoint && result.ChartArea == chart1.ChartAreas[0])
            {
                color = chart1.Series[0].Points[result.PointIndex].Color;
                panel1.Invalidate();
                addValues();
                gradientTimer.Start();
            }
        }

        /// <summary>
        /// 시리즈2(작은 파이)의 요소값 추가하기
        /// </summary>
        private void addValues()
        {
            //초기화
            chart1.Series[1].Points.Clear();

            // 예시데이터 (문제에서는 나잇대)
            values = Enumerable.Range(0, 8).Select(x => rand.Next(80)).ToList();


            for (int i = 0; i < 8; i++)
            {
                chart1.Series[1].Points.AddXY($"{i * 10}~{i * 10 + 9}({values[i]}건)", 0);
                chart1.Series[1].Points.Last().Color = GradientColor(color, i * 10);
            }

            current = 0;
            chart1.Series[1].Points.AddXY("덮개", values.Sum());
            chart1.Series[1].Points.Last().IsVisibleInLegend = false;
            chart1.Series[1].Points.Last().Color = Color.Transparent;
        }

        private Color GradientColor(Color color, int v)
        {
            int r = Math.Max(color.R - v, 0);
            int g = Math.Max(color.G - v, 0);
            int b = Math.Max(color.B - v, 0);

            return Color.FromArgb(r, g, b);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle chart1 = GetRect(0);
            Rectangle chart2 = GetRect(1);

            int stx = chart1.X + chart1.Width / 2 + 20;
            int enx = chart2.X + chart2.Width / 2 + 20;

            max = enx - stx;

            using (var path = new GraphicsPath())
            {
                path.AddPolygon(new Point[]
                {
                    new Point(stx+20, chart1.Y+20),
                    new Point(enx + 20, chart2.Y+20),
                    new Point(enx + 20, chart2.Y+chart2.Height+20),
                    new Point(stx + 20, chart1.Y+chart1.Height+20),
                });
                using (var brush = new LinearGradientBrush(panel1.ClientRectangle, color, Color.White, LinearGradientMode.Horizontal))
                {
                    g.Clip = new Region(new Rectangle(stx + 20, chart1.Y, now + 20, chart1.Height + 20));
                    g.FillPath(brush, path);
                }

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
    }
}
