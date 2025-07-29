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

namespace WindowsFormsApp1.Templete
{
    public partial class BF : Form
    {
        public static User loginUser;
        public Color[] colors = { Color.FromArgb(255, 0,0), Color.FromArgb(0, 100, 200), Color.FromArgb(150, 200,230), Color.FromArgb(170, 200, 150), Color.FromArgb(0, 255, 0), Color.FromArgb(255, 165, 0), Color.FromArgb(190, 190, 190) };

        public BF()
        {
            InitializeComponent();
        }

        private void BF_Load(object sender, EventArgs e)
        {
            using(var bi = new Bitmap(Properties.Resources.logo))
            {
                Icon = Icon.FromHandle(bi.GetHicon());
            }
        }

        public void MsgInfo(string msg) { MessageBox.Show(msg,"정보", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        public void MsgErr(string msg) { MessageBox.Show(msg,"정보", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        public void ShowPage(Form form)
        {
            this.Hide();

            form.FormClosed += Form_FormClosed; 

            form.Show();

        }

        public void ShowPage(string title)
        {
            Stack<Form> forms = new Stack<Form>(Application.OpenForms.Cast<Form>());
            foreach (Form item in forms)
            {
                if (item.Text == title)
                {
                    item.Show();
                    break;
                }     
                item.Dispose();
            }
        }

        public Form GetForm(string title)
        {
            Stack<Form> forms = new Stack<Form>(Application.OpenForms.Cast<Form>());
            foreach (Form item in forms)
            {
                if (item.Text == title)
                {
                    return item;
                }
            }
            return null;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        public string GetNum(string seat)
        {
            char c = seat[0];
            c -= (char)64;
            return (c + seat[1] + seat[2]).ToString();
        }


        public int getage(User u)
        {
            DateTime birth = u.u_birth.Value;
            int age = birth.Year - DateTime.Now.Year;
            if (birth > DateTime.Now.AddYears(age))
                age--;
            return age;
        }

        public (int, int) getPrice(int cnt)
        {
            int age = getage(loginUser);
            double percent = age < 12 || age >= 65 ? 0.5 : age < 19 ? 0.8 : 1;
            int price = (int)(5000 * cnt * percent);
            return (price, (int)(price * 0.1));
        }
    }
}
