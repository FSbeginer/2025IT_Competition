using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.__테스트폼;
using WindowsFormsApp1._인증폼_검정선;
using WindowsFormsApp1._지도그리기_점연결;
using WindowsFormsApp1.View;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new Form1());
            //Application.Run(new GIF_Form());
            //Application.Run(new CancelLine());
            //Application.Run(new 지도());

            //using (var db = new Model.ITTRAINEntities1())
            //{
            //    Model.Information start = db.Information.Find(1);
            //    Model.Information end = db.Information.Find(14);
            //    Model.Division div = db.Division.Find(1);
            //    Application.Run(new ShowRoute() { start = start, end = end, division = div });
            //}
            //Application.Run(new _차트애니메이션.차트애니메이션());
            Application.Run(new MainForm());

        }
    }
}
