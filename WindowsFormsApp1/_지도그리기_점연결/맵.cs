using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.Controls
{
    public partial class 맵 : BC
    {
        public List<(Location, Location)> connection = new List<(Location, Location)>();
        ComboBox combo;
        public 맵()
        {
            InitializeComponent();
        }
        public 맵(ComboBox combo) : this()
        {
            this.combo = combo;
            combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelAdd();
            getData();
            Refresh();
        }

        private void labelAdd()
        {
            Controls.Clear();
            using (var db = new Model.ITTRAINEntities1())
            {
                foreach (var item in db.Station.Where(x => x.s_code / 10000 == combo.SelectedIndex + 1).Select(x => x.Information))
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
                    Controls.Add(lbl);
                }
            }
        }

        private void RouteMap_Load(object sender, EventArgs e)
        {
            if (combo != null)
            {
                getData();
                labelAdd();
            }
        }

        private void getData()
        {
            connection.Clear();
            using (var db = new Model.ITTRAINEntities1())
            {
                var lineList = db.Station.Where(x => x.s_code / 10000 == combo.SelectedIndex + 1).OrderBy(x => x.s_code).Select(x => new Location { sta = x, info = x.Information }).ToList();
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
            }
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
                    (Location from, Location to) = item;
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
    public class Location
    {
        public Model.Station sta;
        public Model.Information info;
    }
}