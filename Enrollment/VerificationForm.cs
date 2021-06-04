using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace Enrollment
{
	/* NOTE: This form is inherited from the CaptureForm,
		so the VisualStudio Form Designer may not load it properly
		(at least until you build the project).
		If you want to make changes in the form layout - do it in the base CaptureForm.
		All changes in the CaptureForm will be reflected in all derived forms 
		(i.e. in the EnrollmentForm and in the VerificationForm)
	*/
	public class VerificationForm : CaptureForm
	{
        SqlConnection con;
        MemoryStream stream;
        byte[] mbyte;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private string status;
        private Button button1;
        private Label label4;
        private Label label3;
        private PrintDialog printDialog1;
        private gymdatabaseEntities1 memdatabase;
        
        public void Verify(DPFP.Template template)
		{
			Template = template;
			ShowDialog();
		}

		protected override void Init()
		{
			base.Init();
			base.Text = "Fingerprint Verification";
			Verificator = new DPFP.Verification.Verification();		// Create a fingerprint template verificator
			UpdateStatus(0);
		}

		protected override void Process(DPFP.Sample Sample)
		{
			base.Process(Sample);
            //Control.CheckForIllegalCrossThreadCalls = false;
            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

			// Check quality of the sample and start verification if it's good
			// TODO: move to a separate task
			if (features != null)
			{
                try {
                    // Compare the feature set with our template
                    DPFP.Verification.Verification ver = new DPFP.Verification.Verification();
                    DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                    /*
                    string st = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PC_Programmer\Documents\visual studio 2015\Projects\Enrollment\Enrollment\empdatabase.mdf;Integrated Security=True";
                con = new SqlConnection(st);
                string query = "select emp_finger from tbl_emp";
                SqlCommand cmd = new SqlCommand(query,con);
                con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    mbyte = ConvertDataSetToByteArray(dt);
                    */
                    
                    foreach (var mem in memdatabase.tbl_member)
                    {
                        stream = new MemoryStream(mem.member_finger);
                        Template = new DPFP.Template(stream);

                        Verificator.Verify(features, Template, ref result);
                        UpdateStatus(result.FARAchieved);
                        if (result.Verified)
                        {
                            
                            MakeReport("The member fingerprint was VERIFIED." + mem.member_name);
                            
                           
                            string todaydate = DateTime.Now.Day.ToString();
                            string todaydate1 = DateTime.Now.Date.ToShortDateString();
                            DateTime longTodayDate = DateTime.Now;
                            string thismonth = DateTime.Now.Month.ToString();
                            string year = DateTime.Now.Year.ToString();
                            string val = "1";
                            int b = ifalreadytoday(mem.member_name, todaydate1);
                            int c = ifOnVaccation(mem.member_name);
                            
                            if (b>0)
                            {
                                status ="Already Marked";
                                MakeReport("Attendance marked already of todayDate");
                            }else if (c>0)
                            {
                                MakeReport("Member On Vaccation");
                                status="On Vaccation";
                            }
                            else
                            {

                                var count = memdatabase.tbl_attendence.Where(x=>x.memberid==mem.Id&&x.day.ToString()==todaydate&&x.month.ToString()==thismonth&&x.year.ToString()==year).FirstOrDefault();
                                var getMember = memdatabase.tbl_member.Where(x => x.Id == mem.Id).FirstOrDefault();
                                var getfee = memdatabase.fees.Where(x => x.member_id == mem.Id).FirstOrDefault();
                                var check_fee=CheckFeeStatus(getMember.member_name,Convert.ToString(DateTime.Now),mem.Id);
                                //var insert = new tbl_attendence();
                                //insert.memberid = memdatabase.tbl_emp.Where(x => x.Id == mem.Id).Select(x=>x.Id).FirstOrDefault();
                                //insert.val = Convert.ToInt32(val);
                                //insert.today_date = longTodayDate;
                                //insert.month =Convert.ToInt32( thismonth);//first was string
                                //insert.year =Convert.ToInt32(year) ;//first was string
                                //insert.fee = 2500;//it is string;
                                //insert.fee_paid = true;
                                //memdatabase.tbl_attendence.Add(insert);
                                //int a=memdatabase.SaveChanges();
                                //LoadAttendenceList();
                                //if (count==0)
                                //{
                                //    count++;
                                //}
                                //else
                                //{
                                //    count++;
                                //}
                                var feedate = DateTime.Now;
                                if (count != null)
                                {
                                    if (check_fee==0)
                                    {
                                        
                                        if (getfee==null)
                                        {
                                            
                                        }
                                        else
                                        {
                                            feedate = getfee.datesubmitted.Value.AddMonths(1);
                                        }
                                       status="Already Marked\nFee Status :\t\t*Unpaid*\nFee Submission Date :\t"+ feedate + "\nMem Id :\t\t\t"+mem.Id+"\nMem Name :\t\t"+mem.member_name;
                                    }
                                    else
                                    {
                                        if (getfee == null)
                                        {

                                        }
                                        else
                                        {
                                            feedate = getfee.datesubmitted.Value.AddMonths(1);
                                        }
                                        status = "Already Marked\nFee Status :\t\t*Paid*\nFee Submission Date :\t" + feedate + "\nMem Id :\t\t\t" + mem.Id + "\nMem Name :\t\t" + mem.member_name;

                                    }
                                    MessageBox.Show(status);
                                }
                                else
                                {
                                    if (check_fee == 0)
                                    {

                                        if (getfee == null)
                                        {

                                        }
                                        else
                                        {
                                            feedate = getfee.datesubmitted.Value.AddMonths(1);
                                        }
                                        status = "Marked\nFee Status :\t\t*Unpaid*\nFee Submission Date :\t" + feedate + "\nMem Id :\t\t\t" + mem.Id + "\nMem Name :\t\t" + mem.member_name;
                                    }
                                    else
                                    {
                                        if (getfee == null)
                                        {

                                        }
                                        else
                                        {
                                            feedate = getfee.datesubmitted.Value.AddMonths(1);
                                        }
                                        status = "Marked\nFee Status :\t\t*Paid*\nFee Submission Date :\t" + feedate + "\nMem Id :\t\t\t" + mem.Id + "\nMem Name :\t\t" + mem.member_name;

                                    }
                                    MessageBox.Show(status);
                                    bool min = true;
                                    if (check_fee==0)
                                    {
                                        min = false;
                                    }
                                    else
                                    {
                                        min = true;
                                    }
                                    string query = "insert into tbl_attendence(memberid,uniqueId,val,today_date,month,year,fee,fee_paid,day)values((select Id from tbl_member where Id='" + mem.Id + "'),'" + mem.member_uniqueId + "','" + Convert.ToInt32(val) + "','" + longTodayDate + "','" + Convert.ToInt32(thismonth) + "','" + Convert.ToInt32(year) + "','" + Convert.ToInt32(mem.member_fee) + "','" + min + "','" + Convert.ToInt32(todaydate) + "')";
                                    
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    con.Open();
                                    int a = cmd.ExecuteNonQuery();
                                    if (a > 0)
                                    {
                                        MakeReport("Attendance is marked");
                                    }
                                    con.Close();
                                    LoadAttendenceList();
                                    
                                }
                               
                            }
                          
                            
                        }
                            
                            
                    else
                            MakeReport("The fingerprint not VERIFIED.");
                        

                    }



                }
                catch(Exception ex)
                {
                    MakeReport(ex.Message);
                }
                }
        }
        
        private int CheckFeeStatus(string mamname,string date,int memid)
        {
            var userfee =memdatabase.fees.Where(x=>x.member_id==memid).FirstOrDefault();
            if (userfee==null)
            {
                MessageBox.Show("Fee Not Added or Submitted");
                button1.Invoke(new Action(() => { button1.Show(); }));
                return 0;
            }
            var diff =( userfee.datesubmitted.Value.AddMonths(1) - DateTime.Now).Days;
            if (diff==0)
            {
                var tempfee = memdatabase.tbl_attendence.Where(x => x.Id == memid).FirstOrDefault();
                tempfee.fee_paid ="Unpaid";
                memdatabase.SaveChanges();
                button1.Show();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private int AddFee()
        {
            return 1;
        }
        private int ifalreadytoday(string memname,string todaydate)
        {
            int a = 0;
            string query = "select * from tbl_attendence where memberid=(select Id from tbl_member where member_name='" + memname + "' AND today_date='" + todaydate + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                a = 1;
            else
                a = 0;
            con.Close();
            return a;
            
        }

        private int ifOnVaccation(string memname)
        {
            int a = 0;
            
            var query =memdatabase.tbl_member.Where(x=>x.member_name==memname).FirstOrDefault();
           
            
               var  onvacc = query.on_vacc;
            if (onvacc!=null)
            {

            
                if (onvacc > 0)
                {
                    a = 1;
                }
                else
                {
                    a = 0;
                }
            }
        

       
            return a;

        }
        private void UpdateStatus(int FAR)
		{
			// Show "False accept rate" value
			SetStatus(String.Format("False Accept Rate (FAR) = {0}", FAR));
		}
        public VerificationForm()
        {
            memdatabase = new gymdatabaseEntities1();
            InitializeComponent();
        }
		private DPFP.Template Template;
		private DPFP.Verification.Verification Verificator;

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(536, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 372);
            this.panel1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(152, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Member Count";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(485, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add Fee";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(687, 261);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Member Attendence";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Verification Type";
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // VerificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1251, 532);
            this.Controls.Add(this.panel1);
            this.Name = "VerificationForm";
            this.Load += new System.EventHandler(this.VerificationForm_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void LoadAttendenceList()
        {
            try
            {

                string todaydate = DateTime.Now.Day.ToString();
                DateTime longTodayDate = DateTime.Now;
                string thismonth = DateTime.Now.Month.ToString();
                string year = DateTime.Now.Year.ToString();
                int countquery = memdatabase.tbl_attendence.Where(x => x.day.ToString() == todaydate && x.month.ToString() == thismonth && x.year.ToString() == year).ToList().Count();
                var member = memdatabase.tbl_attendence.Where(x=>x.day.ToString() == todaydate && x.month.ToString() == thismonth && x.year.ToString() == year).OrderByDescending(x => x.today_date).ToList();
                var member1 = from mem in memdatabase.tbl_attendence
                              where mem.day.ToString() == todaydate && mem.month.ToString() == thismonth && mem.year.ToString() == year orderby mem.today_date descending
                              select new {
                              ID=mem.Id,
                              MemberId=mem.memberid,
                              Name=mem.name,
                              UniqueId=mem.uniqueId,
                              Fee=mem.fee,
                              Fee_Paid=mem.fee_paid,
                              NexFeeDate=mem.fee_date
                              };

                label4.Invoke(new Action(() => { label4.Text = Convert.ToString(countquery); }));
                if (member != null)
                {
                    dataGridView1.Invoke(new Action(() => { dataGridView1.DataSource = member1.ToList(); }));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void VerificationForm_Load(object sender, EventArgs e)
        {
            string st = @"Data Source=DESKTOP-PL5619I\SQLEXPRESS;Initial Catalog=gymdatabase;Integrated Security=True";
            con = new SqlConnection(st);
            button1.Hide();
            LoadAttendenceList();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}