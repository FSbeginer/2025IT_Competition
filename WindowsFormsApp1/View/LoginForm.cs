using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class LoginForm : BF
    {

        private User user;
        private int cnt;
        private CheckBox prev;
        private Label cancelLine;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            prev = checkBox1;
            checkBox1.Checked = true;
            cancelLine = new Label()
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                Parent = textBox3,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            cancelLine.Paint += CancelLine_Paint;
        }

        private void CancelLine_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            Random random = new Random();
            g.DrawLine(Pens.Black, random.Next(20), random.Next(textBox3.Height - 5), random.Next(textBox3.Width - 40, textBox3.Width - 20), random.Next(textBox3.Height - 5));
            g.DrawLine(Pens.Black, random.Next(20), random.Next(textBox3.Height - 5), random.Next(textBox3.Width - 40, textBox3.Width - 20), random.Next(textBox3.Height - 5));

            cancelLine.Text = getRandom();
        }

        private string getRandom()
        {
            string s = "";
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                var c = (char)random.Next(65,91);
                if (random.Next(10) >= 5)
                    s += (c+"").ToUpper();
                else
                    s += (c + "").ToLower();
            }
            return s;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            prev.Checked = false;
            check.Checked = true;
            prev = check;
            label2.Text = "ID";

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            prev.Checked = false;
            check.Checked = true;
            prev = check;
            label2.Text = "전화번호";
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            prev.Checked = false;
            check.Checked = true;
            prev = check;
            label2.Text = "이메일";

        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            var id = textBox1.Text;
            var pw = textBox2.Text;


            if (id == "admin" && pw == "1234")
            {
                MsgInfo("관리자입니다.");
                (GetForm("메인") as MainForm).button1.Text = "로그아웃";
                (GetForm("메인") as MainForm).button2.Text = "분석";
                (GetForm("메인") as MainForm).AdminLoad();
                Close();
                return;
            }

            using (var db = new Model.ITTRAINEntities1())
            {
                user = null;

                switch (label2.Text)
                {
                    case "ID": user = db.User.SingleOrDefault(x => x.u_id == id); break;
                    case "전화번호": user = db.User.SingleOrDefault(x => x.u_tel == id); break;
                    case "이메일": user = db.User.SingleOrDefault(x => x.u_email == id); break;
                }
               
                if (user == null)
                {
                    MsgErr("회원정보가 일치하지 않습니다.");
                    return;
                }
                
                if (user.u_pw != pw)
                {
                    MsgErr("회원정보가 일치하지 않습니다.");
                    cnt++;
                    if (cnt == 3)
                    {
                        user.onoff = 0;
                        cnt = 0;
                        db.SaveChanges();
                        MsgErr("비밀번호 3회 오류로 잠금 처리합니다.");
                    }
                    return;
                }

                if (user.onoff == 0)
                {
                    MsgErr("잠긴 회원입니다.");
                    tabControl1.SelectedTab = tabPage2;
                }
                else
                {
                    MsgInfo(user.u_name + "님 환영합니다.");
                    loginUser = user;
                    Close();
                }
            }
        }

        private void textBox2_Enter_1(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;
            txt.BackColor = Color.Yellow;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;
            txt.BackColor = Color.White;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox3.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var txt = textBox4.Text;
            if(String.IsNullOrEmpty(txt)||txt != cancelLine.Text)
            {
                MsgErr("입력란이 비어있거나 인증 문구가 틀립니다.");
                return;
            }
            MsgInfo("잠금이 해제되었습니다.");
            using (var db = new Model.ITTRAINEntities1())
            {
                db.User.Attach(user);
                user.onoff = 1;
                db.SaveChanges();
            }
            loginUser = user;
            Close();
        }
    }
}
