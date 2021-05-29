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
    public partial class Admin_Login : Form
    {
        string username = "";
        string password = "";

        public Admin_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            username = textBox1.Text;
            password = textBox2.Text;
            if(username.Equals("admin") && password.Equals("admin") || username.Equals("Admin") && password.Equals("Admin"))
            {
                MessageBox.Show("Login Success");
                UserManagement am = new UserManagement();
                am.ShowDialog();
            }
            else if (username.Equals("user") && password.Equals("user123") || username.Equals("User") && password.Equals("user123"))
            {
                MessageBox.Show("Login Success");
                UserMenu am = new UserMenu();
                am.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid Credientials");
            }
        }
    }
}
