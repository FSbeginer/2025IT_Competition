using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;
using WindowsFormsApp1.View;

namespace WindowsFormsApp1.Controls
{

    public partial class RouteMap : BC
    {
        ComboBox combo;

        public List<(StaInfo, StaInfo)> connection = new List<(StaInfo, StaInfo)>();

        List<Label> lblList = new List<Label>();
        List<Information> infos = new List<Information>();
        public Information start, end;

        public int[,] dist;
        public int[,] next;

        public Division division;

        int idx, current = 0;
        Timer timer = new Timer() { Interval = 1000 };

        public RouteMap()
        {
            InitializeComponent();
        }
        public RouteMap(ComboBox combo) : this()
        {
            this.combo = combo;
            combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
        }
        public RouteMap(Information start, Information end, Division division) : this()
        {
            this.start = start;
            this.end = end;
            this.division = division;
        }

        private void RouteMap_Load(object sender, EventArgs e)
        {
            if (combo != null)
            {
                labelAdd();
                getData();
                var form = FindForm() as RouteSearch;
                if (BF.loginUser != null)
                {
                    form.btnEnd.Visible = true;
                    form.btnStart.Visible = true;
                    form.btnReservation.Visible = true;

                    form.btnStart.Click += BtnStart_Click;
                    form.btnEnd.Click += BtnEnd_Click;
                }
                form.btnNext.Click += BtnNext_Click;
                form.btnPrev.Click += BtnPrev_Click;
                Paint += RouteMap_Paint;
            }
            else if (start != null && end != null)
            {
                Label lblstart = new Label
                {
                    Location = new Point(start.x.Value * 4, start.y.Value * 4 - 30),
                    BackColor = Color.Red,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = start.stationname,
                    AutoSize = true
                };
                Controls.Add(lblstart);
                Label lblEnd = new Label
                {
                    Location = new Point(end.x.Value * 4, end.y.Value * 4 - 30),
                    BackColor = Color.Yellow,
                    ForeColor = Color.Gray,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = end.stationname,
                    AutoSize = true
                };
                Controls.Add(lblEnd);

                getinfos();
                getData();
                Paint += DrawRoute;

                var list = GetRoute(start, end).Select(x => " " + infos[x].stationname).ToList();
                TimeSpan time = TimeSpan.FromHours(10);

                for (int i = 0; i < list.Count; i++)
                {
                    Label timeLabel = new Label()
                    {
                        Text = time.ToString("hh\\:mm") + list[i],
                        AutoSize = true,
                        ForeColor = i == 0 ? Color.Red : Color.Black
                    };
                    this.flowLayoutPanel1.Controls.Add(timeLabel);
                    time = time.Add(TimeSpan.FromMinutes(20));
                }

                timer.Tick += (s1, e1) =>
                {
                    current++;

                    var controls = this.flowLayoutPanel1.Controls.Cast<Label>().ToList();
                    for (int i = 0; i < current; i++)
                    {
                        controls[i].ForeColor = Color.SkyBlue;
                    }

                    if (current < controls.Count)
                        controls[current].ForeColor = Color.Red;

                    if (current == list.Count)
                    {
                        timer.Stop();
                    }
                    this.Invalidate();
                };
                timer.Start();
            }

        }

        private void getinfos()
        {
            infos.Clear();
            using (var db = new Model.ITTRAINEntities1())
            {
                foreach (var item in db.Station.Where(x => x.s_code / 10000 == division.d_no).GroupBy(x => x.i_no).Select(g => g.Select(x => x.Information).FirstOrDefault()))
                {
                    infos.Add(item);
                }
            }
        }

        private void DrawRoute(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var brush = new SolidBrush(colors[division.d_no]);

            var list = GetRoute(start, end);

            var points = list.Select(x =>
            {
                var info = infos[x];
                return new Point(info.x.Value * 4, info.y.Value * 4);
            }).ToList();

            g.DrawLines(new Pen(Color.Yellow, 2), points.ToArray());

            var array2 = points.GetRange(0, Math.Min(current + 1, list.Count - 1)).ToArray();
            if (array2.Length >= 2)
                g.DrawLines(new Pen(Color.Red, 2), array2);

            var array = points.GetRange(0, current).ToArray();
            if (array.Length >= 2)
                g.DrawLines(new Pen(Color.SkyBlue, 2), array);

            foreach (var p in points)
            {
                g.FillEllipse(Brushes.Red, p.X - 4, p.Y - 4, 8, 8);
            }
        }

        private void ColorReset()
        {
            foreach (var item in lblList)
            {
                if (item.Tag == start)
                {
                    item.ForeColor = Color.White;
                    item.BackColor = Color.Red;
                    item.ContextMenuStrip = contextMenuStrip1;
                }
                else if (item.Tag == end)
                {
                    item.ForeColor = Color.Gray;
                    item.BackColor = Color.Yellow;
                    item.ContextMenuStrip = contextMenuStrip2;
                }
                else
                {
                    item.ForeColor = lblList[idx] == item ? Color.Red : Color.Gray;
                    item.BackColor = Color.White;
                    item.ContextMenuStrip = null;
                }
            }
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {

            end = lblList[idx].Tag as Information;
            ColorReset();
            btnActivate();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            start = lblList[idx].Tag as Information;
            ColorReset();
            btnActivate();
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            start = end = null;
            idx = 0;
            (FindForm() as RouteSearch).btnPrev.Enabled = false;

            labelAdd();
            getData();
            btnActivate();
            Refresh();
        }

        private void labelAdd()
        {
            Controls.Clear();
            lblList.Clear();
            infos.Clear();
            using (var db = new Model.ITTRAINEntities1())
            {
                foreach (var item in db.Station.Where(x => x.s_code / 10000 == combo.SelectedIndex + 1).GroupBy(x => x.i_no).Select(g => g.Select(x => x.Information).FirstOrDefault()))
                {
                    Label lbl = new Label
                    {
                        Text = item.stationname,
                        AutoSize = true,
                        ForeColor = Color.Gray,
                        Location = new Point(item.x.Value * 4, item.y.Value * 4 - 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = item
                    };
                    lbl.Click += Lbl_Click;
                    lblList.Add(lbl);
                    Controls.Add(lbl);
                    infos.Add(item);
                }
            }
            var form = (FindForm() as RouteSearch);
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            chageView(lblList[idx - 1]);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            chageView(lblList[idx + 1]);
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            chageView(lbl);
        }
        private async void chageView(Label lbl)
        {

            idx = lblList.IndexOf(lbl);

            ColorReset();

            var form = (FindForm() as RouteSearch);
            var information = lbl.Tag as Information;

            form.lblName.Text = information.stationname;
            form.lblLocation.Text = information.address;
            form.pictureBox1.Image = Image.FromFile(new[] { "jpg", "jpeg" }.Select(ext => $"./datafiles/station/{information.i_no}.{ext}").FirstOrDefault(x => File.Exists(x)));


            form.btnPrev.Enabled = idx != 0;
            form.btnNext.Enabled = idx != lblList.Count - 1;


            if (form.Size == form.ori)
            {
                int max = form.ori.Width * 2;
                while (form.Size.Width != max)
                {
                    form.Size = new Size(form.Size.Width + 1, form.Size.Height);
                    await Task.Delay(form.Size.Width % 2);
                }
                form.panel2.Visible = true;
            }

        }


        private void getData()
        {
            connection.Clear();
            using (var db = new Model.ITTRAINEntities1())
            {
                var lineList = db.Station.AsEnumerable().Where(x => x.s_code / 10000 == (combo != null ? combo.SelectedIndex + 1 : division.d_no)).OrderBy(x => x.s_code).Select(x => new StaInfo { sta = x, info = x.Information }).ToList();
                var zeroList = lineList.Where(x => GetCode(x.sta.s_code, 2) == 00).ToList();
                for (global::System.Int32 i = 0; i < zeroList.Count - 1; i++)
                {
                    var now = zeroList[i];
                    var next = zeroList[i + 1];

                    var nextList = lineList.Where(x =>
                    {
                        int scode = x.sta.s_code.Value;
                        int nowScode = now.sta.s_code.Value;
                        int nextScode = next.sta.s_code.Value;

                        return scode >= nowScode && scode <= nextScode;
                    }).GroupBy(x => GetCode(x.sta.s_code, 2) / 10).ToList();

                    foreach (var item in nextList)
                    {
                        var group = item.ToList();
                        connection.Add((now, group[0]));
                        for (global::System.Int32 idx = 0; idx < group.Count - 1; idx++)
                        {
                            connection.Add((group[idx], group[idx + 1]));
                        }
                        connection.Add((group.Last(), next));
                    }
                    connection.Add((now, next));
                }

                int n = infos.Count();
                dist = new int[n, n];
                next = new int[n, n];


                for (int i = 0; i < n; i++)
                {
                    for (global::System.Int32 j = 0; j < n; j++)
                    {
                        dist[i, j] = i == j ? 0 : 999999;
                        next[i, j] = -1;
                    }
                }

                foreach (var item in connection)
                {
                    (StaInfo from, StaInfo to) = item;

                    var fi = from.info;
                    var ti = to.info;
                    int distance = GetDistance(new Point(fi.x.Value, fi.y.Value), new Point(ti.x.Value, ti.y.Value));
                    int fIdx = infos.IndexOf(infos.FirstOrDefault(x => x.i_no == fi.i_no));
                    int tIdx = infos.IndexOf(infos.FirstOrDefault(x => x.i_no == ti.i_no));

                    dist[fIdx, tIdx] = distance;
                    dist[tIdx, fIdx] = distance;
                    next[fIdx, tIdx] = tIdx;
                    next[tIdx, fIdx] = fIdx;
                }

                for (int k = 0; k < n; k++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (global::System.Int32 j = 0; j < n; j++)
                        {
                            int newDist = dist[i, k] + dist[k, j];
                            if (newDist < dist[i, j])
                            {
                                dist[i, j] = newDist;
                                next[i, j] = next[i, k];
                            }
                        }
                    }
                }
            }
        }
        public List<int> GetRoute(Information start, Information end)
        {
            int from = infos.IndexOf(infos.FirstOrDefault(x => x.i_no == start.i_no));
            int to = infos.IndexOf(infos.FirstOrDefault(x => x.i_no == end.i_no));

            List<int> result = new List<int>();

            result.Add(from);
            while (from != to && from != -1)
            {
                from = next[from, to];
                result.Add(from);
            }

            return result;
        }

        private int GetDistance(Point p1, Point p2)
        {
            int x = p1.X - p2.X;
            int y = p1.Y - p2.Y;
            return (int)Math.Sqrt(x * x + y * y);
        }

        private void RouteMap_Paint(object sender, PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(colors[combo.SelectedIndex]))
            using (Pen pen = new Pen(colors[combo.SelectedIndex], 2))
            {
                var g = e.Graphics;

                g.FillPolygon(brush, new[] { new Point(0, 0), new Point(100, 0), new Point(0, 100) });
                using (var matrix = new Matrix())
                {
                    matrix.Translate(25, 25);
                    matrix.Rotate(-45);
                    var font = new Font("맑은 고딕", 15);
                    var size = g.MeasureString(combo.Text, font);
                    g.Transform = matrix;
                    g.DrawString(combo.Text, font, Brushes.White, -size.Width / 2, 0);
                }
                g.ResetTransform();

                foreach (var item in connection)
                {
                    (StaInfo from, StaInfo to) = item;
                    int fx = from.info.x.Value * 4;
                    int fy = from.info.y.Value * 4;
                    int tx = to.info.x.Value * 4;
                    int ty = to.info.y.Value * 4;
                    g.DrawLine(pen, fx, fy, tx, ty);
                    g.FillEllipse(brush, tx - 4, ty - 4, 8, 8);
                    g.FillEllipse(brush, fx - 4, fy - 4, 8, 8);
                }
            }
        }

        private void 취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start = null;
            ColorReset();
            btnActivate();
        }

        private void btnActivate()
        {
            (FindForm() as RouteSearch).btnReservation.Enabled = start != null && end != null;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            end = null;
            ColorReset();
            btnActivate();
        }

        public int GetCode(int? scode, int n)
        {
            if (n == 0)
                return scode.Value / 10000;
            else if (n == 1)
                return scode.Value / 100 % 100;
            else
                return scode.Value % 100;
        }


    }
    public class StaInfo
    {
        public Model.Station sta;
        public Model.Information info;
    }
}