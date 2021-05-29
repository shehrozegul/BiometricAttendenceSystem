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
    public partial class AddMember : Form
    {
        private gymdatabaseEntities2 memdatabase;
        private DPFP.Template Template;
        SqlConnection con;
        public AddMember()
        {
            InitializeComponent();
            string st = @"Data Source=DESKTOP-PL5619I\SQLEXPRESS;Initial Catalog=gymdatabase;Integrated Security=True";
            con = new SqlConnection(st);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnrollmentForm Enroller = new EnrollmentForm();
            Enroller.OnTemplate += this.OnTemplate;
            Enroller.ShowDialog();
        }
        private void OnTemplate(DPFP.Template template)
        {
            this.Invoke(new Function(delegate ()
            {
                Template = template;
                button2.Enabled= (Template != null);
                if (Template != null)
                {
                    MessageBox.Show("The fingerprint template is ready for fingerprint verification.", "Fingerprint Enrollment");
                    textBox6.Text = "FingerPrint Captured";
                }
                
                else
                    MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try { 

            byte[] fingerbytes = Template.Bytes;
                var temp = comboBox1.SelectedItem;
                var tabindex = comboBox1.SelectedIndex;
                tbl_member tblmem = new tbl_member
                {
                    member_name = textBox1.Text,
                    member_age = textBox2.Text,
                    member_cell = textBox3.Text,
                    member_uniqueId = textBox4.Text,
                    member_type = temp.ToString(),
                    member_fee = textBox5.Text,
                    on_vacc = 0,
                member_finger = fingerbytes,
                join_date=dateTimePicker2.Value

            };
           memdatabase.tbl_member.Add(tblmem);
            memdatabase.SaveChanges();
                var tempid = tblmem.Id;
                var memFee = new fee();
                memFee.member_fee =Convert.ToInt32( textBox5.Text);
                memFee.member_id =tempid;
                memFee.datesubmitted = dateTimePicker2.Value;
                memdatabase.fees.Add(memFee);
                memdatabase.SaveChanges();
                Initiliaze();
            Template = null;
            MessageBox.Show("Saved");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            /*
            string query = "insert into tbl_emp(member_name,member_age,member_cell,member_type,member_fee,member_finger)values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "',(SELECT convert(varbinary, '"+fingerbytes+"')))";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Member Added");
            }
            else
            {
                MessageBox.Show("Error");
            }
            con.Close();
            */
        }

        private void Initiliaze()
        {
            var member = from mem in memdatabase.tbl_member
                         select new
                         {
                             Id = mem.Id,
                             Name = mem.member_name,
                             Fee = mem.member_fee,
                             Type = mem.member_type,
                             Age = mem.member_age,
                             Cell = mem.member_cell,
                             
                             
                              
                          };
            if (member!=null)
            {
                dataGridView1.DataSource = member.ToList();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            VerificationForm vf = new VerificationForm();
            vf.ShowDialog();
        }

        private void AddMember_Load(object sender, EventArgs e)
        {
            button3.Hide();
            memdatabase = new gymdatabaseEntities2();
            Initiliaze();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var temp = comboBox1.SelectedItem;
            var tabindex = comboBox1.SelectedIndex;
            if (tabindex==0)
            {
                textBox5.Text = "800";
            }
            if (tabindex == 1)
            {
                textBox5.Text = "2000";
            }
            if (tabindex == 2)
            {
                textBox5.Text = "1500";
            }
            
        }
    }
}
