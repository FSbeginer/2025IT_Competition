using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Controls;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.Templete;

namespace WindowsFormsApp1.View
{
    public partial class ShowRoute : BF
    {
        public Information start { get; set; }
        public Information end { get; set; }

        public Division division { get; set; }

        public ShowRoute()
        {
            InitializeComponent();
        }

        private void ShowRoute_Load(object sender, EventArgs e)
        {
            RouteMap map = new RouteMap(start, end, division);
            panel1.Controls.Add(map);
        }
    }
}
