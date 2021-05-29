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
    public partial class YearReport : Form
    {
        SqlConnection con;
        public YearReport()
        {
            InitializeComponent();
            string st = @"Data Source=DESKTOP-PL5619I\SQLEXPRESS;Initial Catalog=gymdatabase;Integrated Security=True";
            con = new SqlConnection(st);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT te.member_name,te.member_age,te.member_cell,te.member_type,ta.today_date,ta.val from tbl_attendance ta JOIN tbl_member te ON te.Id=ta.memberid where ta.year='" + comboBox2.SelectedItem.ToString() + "'";

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
    }
}
