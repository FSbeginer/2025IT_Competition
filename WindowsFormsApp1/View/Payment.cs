using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class Payment : BF
    {
        public Division division;
        public Information from;
        public Information to;
        public int cnt;
        public TimeSpan start;
        public string seat;
        public int carno;
        public DateTime selDate;
        public bool special;

        int price, use;

        public Payment()
        {
            InitializeComponent();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            cnt = (GetForm("노선검색") as RouteSearch).map.GetRoute(from, to).Count;
            price = getPrice(cnt).Item1;
            lblDate.Text = selDate.ToString("yyyy년 MM월 dd일(ddd)");
            lblFrom.Text = from.stationname;
            lblTo.Text = to.stationname;
            lblStart.Text = start.ToString("hh\\:mm");
            lblEnd.Text = (start + TimeSpan.FromMinutes(20 * cnt)).ToString("hh\\:mm");
            lblSeat.Text = carno + "호차 " + seat;
            lblPrice.Text = price.ToString("N0") + "(" + (special ? "특실" : "일반") + "/" + (selDate.DayOfWeek == DayOfWeek.Sunday || selDate.DayOfWeek == DayOfWeek.Saturday ? "주말" : "평일") + ")";
            lblPrice2.Text = lblPrice.Text;
            lblMileage.Text = loginUser.u_mileage.Value.ToString("N0");
            lblPay.Text = price+"";
            dataGridView1.Rows.Add(carno+"호차", seat, special?"특실":"일반", Properties.Resources.qrcode);
            dataGridView1.Rows[0].Height = 90;
            using(var db = new Model.ITTRAINEntities1())
            {
                db.Information.Attach(from);
                label2.Text = "예약번호:" + selDate.ToString("yyyyMMdd") + "-" + from.Station.FirstOrDefault(x => x.s_code / 10000 == division.d_no).s_code + "-" + carno.ToString("D2")+GetNum(seat);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int idx = tabControl1.SelectedIndex;
            if (idx == 0)
            {
                tabControl1.SelectedIndex++;
            }
            else if (idx == 1)
            {
                List<TextBox> textBoxes = new List<TextBox> { textBox2, textBox3, textBox4, textBox5 };
                foreach (var item in textBoxes)
                {
                    if (item.TextLength != 4)
                    {
                        MsgErr("카드번호 형식을 확인하세요.");
                        return;
                    }
                }
                bool result = int.TryParse(textBox6.Text, out int mm);

                if (result)
                {

                    if (mm > 9999 || mm < 1000 || mm/100 > 12 || mm/100 < 1)
                    {
                        MsgErr("유효기간 형식을 확인하세요.");
                        return;
                    }
                }
                else
                {
                    MsgErr("유효기간 형식을 확인하세요.");
                    return;
                }

                if(textBox7.Text != loginUser.u_pw)
                {
                    MsgErr("카드 비밀번호를 확인해주세요.");
                    return;
                }
                tabControl1.SelectedIndex++;
                button2.Text = "확인";
            }
            else
            {
                using (var db = new Model.ITTRAINEntities1())
                {
                    Model.Reservation r = new Model.Reservation();
                    r.u_no = loginUser.u_no;
                    r.r_date = selDate;
                    r.r_start = from.i_no;
                    r.r_end = to.i_no;
                    r.r_car = carno;
                    r.r_seat = seat;
                    r.r_time = start;
                    db.Reservation.Add(r);
                    db.User.Attach(loginUser);
                    //마일리지
                    loginUser.u_mileage -= use;
                    loginUser.u_mileage += (int?)((price - use) * 0.1);
                    db.SaveChanges();
                    MsgInfo("예약이 완료되었습니다.");
                    ShowPage(new ShowRoute { start = from, end = to, division = division});
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = loginUser.u_mileage.ToString();
            use = loginUser.u_mileage.Value;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            var txt = (sender as TextBox);
            txt.BackColor = Color.Yellow;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            var txt = (sender as TextBox);
            txt.BackColor = Color.White;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            new KeyPad(sender as TextBox).ShowDialog();
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            label21.Visible = false;
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text))
                label21.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowPage(new SeatSelection() {selSeat =seat, carno = carno, division = division, start =from, end = to , special = special});
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool result = int.TryParse(textBox1.Text, out int mileage);
            if (result)
            {
                use = mileage;
                lblPay.Text = (price - mileage).ToString("N0");
            }
            else
            {
                MsgErr("숫자를 입력하세여.");
            }
        }
    }
}
