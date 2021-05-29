using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment
{
    public partial class UserMenu : Form
    {
        public UserMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddMember ae = new AddMember();
            ae.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            VerificationForm vf = new VerificationForm();
            vf.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MonthlyReport mr = new MonthlyReport();
            mr.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            YearReport yr = new YearReport();
            yr.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VaccationMode vm = new VaccationMode();
            vm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
