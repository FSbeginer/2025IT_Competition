using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1
{
    public partial class GIF_Form : BF
    {
        private List<Image> frames = new List<Image>();
        private int currentFrame = 0;
        private Timer animationTimer = new Timer();
        Boolean stop = false;
        List<int> Ints = new List<int>();
        Panel[] colorPanel = new Panel[5];

        public GIF_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //기본 UI생성
            for (int i = 0; i < 5; i++)
            {
                colorPanel[i] = new Panel();
                colorPanel[i].Paint += ColorPanel_Paint;
                colorPanel[i].Tag = i == 0 ? "blue" : "white";
                colorPanel[i].Dock = DockStyle.Fill;
                tableLayoutPanel1.Controls.Add(colorPanel[i], i + 1, 0);
            }

            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            Panel stopButton = new Panel();
            stopButton.Paint += StopButton_Paint;
            stopButton.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(stopButton, 6, 0);

            stopButton.Click += StopButton_Click;

            //GIF
            GetGifFrame();
            animationTimer.Start();
        }

        private void GetGifFrame()
        {
            for (int i = 1; i <= 5; i++)
            {
                Ints.Add(frames.Count);
                Image gif = Image.FromFile($@"./datafiles/advertisement/{i}.gif");
                FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
                int frameCnt = gif.GetFrameCount(dimension);
                for (global::System.Int32 j = 0; j < frameCnt; j++)
                {
                    gif.SelectActiveFrame(dimension, j);
                    frames.Add(new Bitmap(gif));
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            stop = !stop;
            if (stop)
                animationTimer.Start();
            else
                animationTimer.Stop();
        }

        private void StopButton_Paint(object sender, PaintEventArgs e)
        {
            using (var matrix = new Matrix())
            {
                var panel = sender as Panel;
                var g = e.Graphics;
                g.DrawEllipse(Pens.White, 0, 0, panel.Width - 1, panel.Height - 1);
                g.DrawString(" | |", Font, Brushes.White, new PointF(0, 0));
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            picGif.Image = frames[currentFrame];
            currentFrame = ++currentFrame % frames.Count;
            int idx = Ints.IndexOf(currentFrame);

            if (idx != -1)
            {
                foreach (var c in colorPanel)
                {
                    c.Tag = "white";
                }
                colorPanel[idx].Tag = "blue";
                Refresh();
            }
        }

        private void ColorPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            Graphics g = e.Graphics;
            if (p.Tag.ToString() == "white")
                g.FillEllipse(Brushes.White, p.ClientRectangle);
            else
                g.FillEllipse(Brushes.Blue, p.ClientRectangle);
            g.DrawEllipse(Pens.White, 0, 0, p.ClientRectangle.Width - 1, p.ClientRectangle.Height - 1);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            using (var path = new GraphicsPath())
            {
                var rect = panel1.ClientRectangle;

                int radius = rect.Height / 2;
                int diameter = rect.Height;
                path.StartFigure();
                path.AddArc(0, 0, diameter, diameter, -90, -180);
                path.AddArc(rect.Width - diameter, 0, diameter, diameter, 90, -180);
                path.CloseFigure();


                int gap = 5, size = diameter - 10;
                for (global::System.Int32 i = 0; i < 6; i++)
                {
                    g.FillEllipse(Brushes.Blue, diameter + (size + gap) * i, 2, size, size);
                    g.DrawEllipse(Pens.White, diameter + (size + gap) * i, 2, size, size);
                }

                panel1.Region = new Region(path);

            }
        }
    }
}
