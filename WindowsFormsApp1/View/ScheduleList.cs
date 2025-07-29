using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Controls;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class ScheduleList : BF
    {
        public DateTime selDate;
        public TimeSpan startTime;
        public Information start, end;
        public Division route;
        
        public ScheduleList()
        {
            InitializeComponent();
        }

        private void ScheduleList_Load(object sender, EventArgs e)
        {
            label2.Text = start.stationname + "→" + end.stationname;
            label3.Text = selDate.ToString("yyyy년 MM월 dd일(ddd) HH:mm");

            var map = (GetForm("노선검색") as RouteSearch).map;

            int cnt = map.GetRoute(start, end).Count;


            TimeSpan span = TimeSpan.FromMinutes(20 * cnt);

            while (startTime != route.d_eTime.Value)
            {
                using (var con = new SqlConnection("data source=.\\sqlexpress;initial catalog=ITTRAIN;user id=sa;password=1234;"))
                {
                    con.Open();
                    int seatCnt = 40 * (route.d_carR.Value-route.d_carS.Value);
                    int rSeat = 0;
                    string seat = "일반";
                    int price = getPrice(cnt).Item1;

                    if (selDate.DayOfWeek == DayOfWeek.Sunday || selDate.DayOfWeek == DayOfWeek.Saturday)
                        price = (int)(price * 1.2);
                    if (dataGridView1.Rows.Count % 2 == 1)
                    {
                        price = (int)(price * 1.3);
                        seat = "특실";
                        seatCnt = 21 * route.d_carS.Value;
                    }

                    using (var cmd = new SqlCommand($"select count(*) from reservation where r_date = '{selDate.ToShortDateString()}' and r_time = '{startTime}' and r_start = {start.i_no} and r_end = {end.i_no}", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read()) rSeat = reader.GetInt32(0);
                    }
                    dataGridView1.Rows.Add(startTime.ToString("hh\\:mm"), (startTime + span).ToString("hh\\:mm"), price.ToString("N0"), seat, $"{seatCnt-rSeat}/{seatCnt}");
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = startTime;
                    if (dataGridView1.Rows.Count % 2 == 0)
                        startTime += TimeSpan.FromMinutes(30);
                }
            }
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MsgErr("예약할 노선을 선택하세요.");
                return;
            }
            var time = (TimeSpan)dataGridView1.CurrentRow.Tag;
            ShowPage(new SeatSelection {selDate = selDate, selTime = time, special = dataGridView1.CurrentRow.Index % 2 == 1, start = start, end = end, division = route});
        }

    }
}
