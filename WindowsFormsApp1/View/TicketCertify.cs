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
    public partial class TicketCertify : BF
    {
        public TicketCertify()
        {
            InitializeComponent();
        }

        private void TicketCertify_Load(object sender, EventArgs e)
        {
            label2.Text = "이름:" + loginUser.u_name + "(" + loginUser.u_id + ")";
            using (var db = new Model.ITTRAINEntities1())
            {
                var reservation = db.Reservation.AsEnumerable().Where(x => x.u_no == loginUser.u_no && x.r_date > DateTime.Now).ToList();
                foreach (var item in reservation)
                {
                    string rno = item.r_date.Value.ToString("yyyyMMdd") + "-" + item.Station.s_code + "-" + item.r_car.Value.ToString("D2") + GetNum(item.r_seat);

                    dataGridView1.Rows.Add(rno, item.r_date.Value.ToShortDateString(), item.Station.Information.stationname, item.Station1.Information.stationname, item.r_time.Value.ToString("hh\\:mm"), item.r_car, item.r_seat);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = item;
                }
            }
            label3.Text = "보유승차권(" + dataGridView1.RowCount + ")";
            dataGridView1.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MsgErr("예약 내역을 선택하세요.");
                return;
            }
            var reser = dataGridView1.CurrentRow.Tag as Model.Reservation;
            using (var db = new Model.ITTRAINEntities1())
            {
                db.Reservation.Attach(reser);
                db.Reservation.Remove(reser);
                db.SaveChanges();
            }
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            label3.Text = "보유승차권(" + dataGridView1.RowCount + ")";
            // 마일리지 차감 기능은 미구현
        }
    }
}
