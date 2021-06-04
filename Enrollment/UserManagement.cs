using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment
{
    public partial class UserManagement : Form
    {
        private gymdatabaseEntities1 memdatabase;
        public int memId { get; set; }
        SqlConnection con;
        public UserManagement()
        {
            InitializeComponent();
            string st = @"Data Source=DESKTOP-PL5619I\SQLEXPRESS;Initial Catalog=gymdatabase;Integrated Security=True";
            con = new SqlConnection(st);
        }
        private void LoadUserList()
        {
            try
            {
                var member = from mem in memdatabase.users
                             select new
                             {
                                 Id = mem.Id,
                                 Name = mem.Name,
                                 Password=mem.Password

                             };
                if (member != null)
                {
                    dataGridView1.DataSource = member.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "insert into [user](Name,Password)values('" + textBox1.Text + "','" + textBox2.Text + "')";
               
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show("User Added");
                    LoadUserList();
                }
                else
                {
                    MessageBox.Show("Error");
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void update()
        {
            var updatequery = memdatabase.users.Where(x => x.Id == memId).FirstOrDefault();
            updatequery.Name = textBox1.Text;
            updatequery.Password = textBox2.Text;
            memdatabase.SaveChanges();
        }
        private void delete()
        {
            var deletequery = memdatabase.users.Where(x => x.Id == memId).FirstOrDefault();
            memdatabase.users.Remove(deletequery);
            memdatabase.SaveChanges();
        }
        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            memdatabase = new gymdatabaseEntities1();
            LoadUserList();
        }

        private void userManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Hide();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            memId = id;
            var findmem = memdatabase.users.Where(x => x.Id == id).FirstOrDefault();
            textBox1.Text = findmem.Name;
            textBox2.Text = findmem.Password;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            update();
            clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            delete();
            clear();
        }
    }
}
