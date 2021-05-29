using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Enrollment
{
    public partial class MonthlyReport : Form
    {
        SqlConnection con;
        int  monthname;
        public MonthlyReport()
        {
            InitializeComponent();
            string st = @"Data Source=DESKTOP-PL5619I\SQLEXPRESS;Initial Catalog=gymdatabase;Integrated Security=True";
            con = new SqlConnection(st);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            monthname = getmonthNo(comboBox1.SelectedItem.ToString());
            string query = "SELECT te.member_name,te.member_age,te.member_cell,te.member_type,ta.today_date,ta.val from tbl_attendance ta JOIN tbl_member te ON te.Id=ta.memberid where ta.month= '" + monthname+ "' AND ta.year='"+comboBox2.SelectedItem.ToString()+"'";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            da.Update(dt);
            con.Close();
        }

        private void MonthlyReport_Load(object sender, EventArgs e)
        {

        }
        private int getmonthNo(string monthname)
        {
            int mno = 0;
            switch (monthname)
            {
                case "January":
                    mno = 1;
                    break;
                case "Feburary":
                    mno = 2;
                    break;
                case "March":
                    mno = 3;
                    break;
                case "April":
                    mno = 4;
                    break;
                case "May":
                    mno = 5;
                    break;
                case "June":
                    mno = 6;
                    break;
                case "July":
                    mno = 7;
                    break;
                case "August":
                    mno = 8;
                    break;
                case "September":
                    mno = 9;
                    break;
                case "October":
                    mno = 10;
                    break;
                case "November":
                    mno = 11;
                    break;
                case "December":
                    mno = 12;
                    break;
               
            }
            return mno;
        }
    }
}
