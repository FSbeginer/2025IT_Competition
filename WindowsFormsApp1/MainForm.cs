using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Templete;
using WindowsFormsApp1.View;

namespace WindowsFormsApp1
{
    public partial class MainForm : BF
    {
        private List<List<Image>> frames = new List<List<Image>>();
        private Timer animationTimer = new Timer();
        Boolean stop = false;
        List<Panel> colorPanel = new List<Panel>();
        int idx = 0;
        bool run = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //기본 UI생성
            Size size = new Size(flowLayoutPanel2.Height - 10, flowLayoutPanel2.Height - 10);
            flowLayoutPanel2.Controls.Clear();
            for (int i = 0; i < 5; i++)
            {
                var cp = new Panel()
                {
                    Margin = new Padding(3),
                };
                cp.Paint += ColorPanel_Paint;
                cp.Tag = i == 0 ? "blue" : "white";
                cp.Size = size;
                colorPanel.Add(cp);
                flowLayoutPanel2.Controls.Add(cp);
            }
            //GIF
            GetGifFrame();
            animationTimer.Interval = 100;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            lblPlus.Size = lblMinus.Size = label2.Size = size;
            flowLayoutPanel2.Controls.Add(label2);
            flowLayoutPanel2.Controls.Add(lblMinus);
            flowLayoutPanel2.Controls.Add(lblPlus);

            reload();
        }

        private void GetGifFrame()
        {
            frames.Clear();
            idx = 0;
            for (int i = 1; i <= 5; i++)
            {
                if (File.Exists($@"./datafiles/advertisement/{i}.gif"))
                {
                    var imglist = new List<Image>();
                    using (FileStream ms = new FileStream($@"./datafiles/advertisement/{i}.gif", FileMode.Open, FileAccess.Read))
                    {
                        Image gif = Image.FromStream(ms);
                        FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
                        int frameCnt = gif.GetFrameCount(dimension);
                        for (global::System.Int32 j = 0; j < frameCnt; j++)
                        {
                            gif.SelectActiveFrame(dimension, j);
                            imglist.Add(new Bitmap(gif));
                        }
                        frames.Add(imglist);    
                    }
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
                var lbl = sender as Label;
                var g = e.Graphics;
                g.DrawEllipse(Pens.White, 0, 0, lbl.Width - 1, lbl.Height - 1);
            }
        }


        private async void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if(frames.Count == 0) return;
            if (run) return;
            run = true;
            var img = frames[idx];
            int oricnt = frames.Count;
            foreach (var item in img)
            {
                if (oricnt != frames.Count)
                {
                    run = false;
                    return;
                }
                picGif.Image = item;
                await Task.Delay(30);
            }

            if(frames.Count == 0)
            {
                picGif.Image = null;
                idx = 0;
                return;
            }
            idx = ++idx % frames.Count;

            for (int i = 0; i < frames.Count; i++)
            {
                if (i == idx)
                    colorPanel[i].Tag = "blue";
                else
                    colorPanel[i].Tag = "white";
                colorPanel[i].Invalidate();
            }

            run = false;
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
                var rect = flowLayoutPanel2.ClientRectangle;

                int radius = rect.Height / 2;
                int diameter = rect.Height;
                path.StartFigure();
                path.AddArc(0, 0, diameter, diameter, -90, -180);
                path.AddArc(rect.Width - diameter, 0, diameter, diameter, 90, -180);
                path.CloseFigure();

                flowLayoutPanel2.Region = new Region(path);

            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (loginUser != null)
            {
                button1.Text = "로그아웃";
                button2.Text = "예매";
                button3.Text = "승차권확인";
                button3.Visible = true;
                button4.Text = "종료";
            }
            else
            { 
                button3.Visible = false;
                button4.Text = "종료";
            }
        }

        private void reload()
        {
            for (int i = 0; i < 5; i++)
            {
                colorPanel[i].Visible = i + 1 <= frames.Count;
            }
            label2.Visible = true;
            lblMinus.Visible = false;
            lblPlus.Visible = false;
            Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (loginUser != null || button1.Text == "로그아웃")
            {
                loginUser = null;
                button1.Text = "로그인";
                button2.Text = "노선검색";
                reload();
                this.Hide();
                this.Show();
            }
            else
            {
                ShowPage(new LoginForm());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "분석")
            {
                ShowPage(new Analysis());
            }
            else
            {
                ShowPage(new RouteSearch());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowPage(new TicketCertify());
        }

        public void AdminLoad()
        {
            for (int i = 0; i < 5; i++)
            {
                colorPanel[i].Visible = i+1 <= frames.Count;
            }


            lblPlus.Enabled = frames.Count != 5;
            lblMinus.Enabled = frames.Count != 0;
            lblPlus.Visible = true;
            lblMinus.Visible = true;
            label2.Visible = false;
            Refresh();
        }

        private void LblMinus_Click(object sender, EventArgs e)
        {
            if (frames.Count > 0)
            {
                File.Delete(@"./datafiles/advertisement/" + (idx + 1) + ".gif");
                var files = Directory.GetFiles(@"./datafiles/advertisement");
                for (global::System.Int32 i = idx + 1; i < files.Count(); i++)
                {
                    string path = @"./datafiles/advertisement/" + (i + 1) + ".gif";
                    if (File.Exists(path))
                        File.Move(path, @"./datafiles/advertisement/" + (i) + ".gif");
                }
                GetGifFrame();
            }
            AdminLoad();
        }

        private void Lblplus_Click(object sender, EventArgs e)
        {
            if (frames.Count < 5)
            {
                animationTimer.Stop();
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "GIF Files|*.gif";
                open.Multiselect = false;
                if (open.ShowDialog() == DialogResult.OK)
                {
                    string path = open.FileName;
                    for (global::System.Int32 i = 4; i >= idx+1; i--)
                    {
                        string p = @"./datafiles/advertisement/" + (i) + ".gif";
                        if (File.Exists(p))
                        {
                            File.Move(p, "./datafiles/advertisement/"+(i+1)+".gif");
                            Console.WriteLine(1);
                        }
                    }
                    File.Copy(path, $@"./datafiles/advertisement/{idx + 1}.gif");
                    GetGifFrame();
                    AdminLoad();
                }
                animationTimer.Start();
            }
        }

        private void PicGif_Paint(object sender, PaintEventArgs e)
        {
            if (frames.Count == 0)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(Pens.SkyBlue, 0, 0, picGif.Width - 1, picGif.Height - 1);
                g.DrawLine(Pens.SkyBlue, picGif.Width / 2 - 20, picGif.Height / 2, picGif.Width / 2 + 20, picGif.Height / 2);
                g.DrawLine(Pens.SkyBlue, picGif.Width / 2, picGif.Height / 2 - 20, picGif.Width / 2, picGif.Height / 2 + 20);
            }
        }
    }
}
