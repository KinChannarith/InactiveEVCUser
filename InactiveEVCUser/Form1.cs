using InactiveEVCUser.models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InactiveEVCUser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public String constr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public String EVCUsersStatus = "";
        public String EVCUsersText = "";
        const Int32 BufferSize = 128;
        public List<CorrectedUser> users = new List<CorrectedUser>();
        public List<UserList> userLists = new List<UserList>();
        public string FileExportPath = "";
        public bool hasInactive = false;
        private void btnTest_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<ComboboxItem> items = new List<ComboboxItem>();
            items.Add(new ComboboxItem() { Text = "all", Value = "" });
            items.Add(new ComboboxItem() { Text = "active", Value = "true" });
            items.Add(new ComboboxItem() { Text = "Inactive", Value = "false" });
            this.cboStatus.DataSource = items;
            this.cboStatus.ValueMember = "Value";
            this.cboStatus.DisplayMember = "Text";

            using (var fileStream = File.OpenRead("Map Users.csv"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                //get all worng user list in bin Alter User.csv
                String l;
                while ((l = streamReader.ReadLine()) != null)
                {
                    if (l == "")
                        break;
                    CorrectedUser obj = new CorrectedUser();
                    string[] usersList = l.Split(',');

                    obj.WrongName = usersList[0];
                    obj.CorrectedName = usersList[1];
                    users.Add(obj);


                }
            }
        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
            ComboboxItem obj = cboStatus.SelectedItem as ComboboxItem;
            if (obj != null)
            {
                // MessageBox.Show(obj.Value);
                EVCUsersStatus = obj.Value;
                EVCUsersText = obj.Text;
            }
            //string constr = "User Id=KIN_CHANNARITH;Password=AAbb!@#20211028;Data Source=10.12.4.209:1526/ora11g";
            //getAllUsers(constr);


            //use worker
            //TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : " + "start");
            if (!EVCUserReconcileWorker.IsBusy)
            {
                //btnProcess.Enabled = false;
                EVCUserReconcileWorker.RunWorkerAsync();
            }
            //TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : " + "finish");
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            string htmlstring = "";
            htmlstring = "<b>Dear Colleagues,</b> <p>Please kindly check on Inactive users today.</p>";
            Email(htmlstring, FileExportPath, hasInactive, userLists);
        }
        public static void Email(string htmlString, string filePath, bool IsInactive, List<UserList> InactiveUsers)
        {
            try
            {
                //MailMessage message = new MailMessage();
                //SmtpClient smtp = new SmtpClient();

                //message.From = new MailAddress("mynameischannarith@gmail.com");
                //message.To.Add(new MailAddress("kin.channarith@smart.com.kh"));
                //message.Subject = "Test";
                //message.IsBodyHtml = true; //to make message body as html  
                //message.Body = htmlString;                
                //smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com"; //for gmail host  
                //smtp.EnableSsl = true;

                //smtp.Credentials = new NetworkCredential("mynameischannarith@gmail.com", "evacqyjyvbmtadvh");
                //smtp.UseDefaultCredentials = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Send(message);





                var fromAddress = new MailAddress("billing.prepaid@smart.com.kh", "Test Sending Inactive EVC Users");
                var toAddress = new MailAddress("billing.prepaid@smart.com.kh", "Billing Prepaid");
                //const string fromPassword = "Narithasdfghjkl@2021";
                const string fromPassword = "evacqyjyvbmtadvh";
                const string subject = "Daily Report for EVC users Inactive in AD";
                string body = htmlString;
                if (IsInactive == false)
                {
                    body = "<p>Dear Colleagues,</p> <p>There's no EVC user inactive in AD today.</p>";
                }

                //Attachment attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                //var smtp = new SmtpClient("10.0.11.15");
                //smtp.Port = 25;
                //smtp.UseDefaultCredentials = true;
                var smtp = new SmtpClient
                {

                    Host = "10.0.11.15",
                    //Host = "smart.com.kh",
                    Port = 25,
                    //Host = "smtp.gmail.com",
                    //Port = 587,
                    //EnableSsl = true,
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = false,
                    UseDefaultCredentials = true,
                    //Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                //add table
                if (InactiveUsers.Count > 0)
                {
                    body = body + "<table style='border: 1px solid lightblue;'> <tr style='border: 1px solid lightblue;background-color:green; color:white'><th>Username</th> <th>Status</th></tr>";

                    foreach (UserList user in InactiveUsers)
                    {
                        body = body + "<tr ><td style='border: 1px solid lightblue;'>" + user.username + "</td>" + "<td style='border: 1px solid lightblue;'>" + user.status + "</td>" + "</tr>";

                    }
                    body = body + "</table>";
                }
                body = body + "<p>***Note: This is a system generated email, please do not reply. If you have any concern, please feel free to contact billing.prepaid@smart.com.kh *** </p> <hr style='border: solid 1px lightgrey;' > <p><b><i>Smart Axiata, CIO - Billing Prepaid </i></b></p>";
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true


                })
                {
                    //if (IsInactive == true)
                        //message.Attachments.Add(attachment);
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void EVCUserReconcileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : Start");
            getAllUsers(constr);
            TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : Finish");
        }
        private void TextLogAddText(string Text)
        {
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke(new MethodInvoker(delegate { TextLogAddText(Text); }));

                return;
            }
            string logdetail;
            if (textBox2.Text.Length > 5000)
            {
                logdetail = textBox2.Text.Substring(0, 5000);
            }
            else
            {
                logdetail = textBox2.Text;
            }

            textBox2.Text = Text + "\r\n" + logdetail;
        }
        public bool DoesUserExist(string userName)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, "SMART.LOCAL"))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser != null;
                }
            }
        }
        private void getAllUsers(string constr)
        {
            OracleConnection con = new OracleConnection(constr);
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = "SELECT " +
                               "t1.operid,t1.name," +
                               "t1.createdate," +
                               "t4.LASTLOGINDATE," +
                               "t1.UPDATEDATE as LASTMODIFYDATE," +
                               "t3.NAME as ROLE " +
                               "FROM sysdb.T_BME_OPERATOR t1 " +
                               "left join sysdb.t_bme_user_role t2 " +
                               "on t1.operid = t2.operid " +
                               "left join sysdb.t_bme_role t3 " +
                               "on t3.roleid = t2.ROLEID " +
                               "left join sysdb.T_BME_LOGIN t4 " +
                               "on t1.name = t4.name " +
                               "where t1.EXPIREDTIME is null " +
                               "Order by t1.CREATEDATE desc ";
            cmd.Connection = con;
            con.Open();

            OracleDataReader dr = cmd.ExecuteReader();

            FileExportPath = "[" + EVCUsersText + "]User reconcile " + DateTime.Now.ToString("dd/MM/yyy hhmmss") + ".csv";
            StreamWriter sw = new StreamWriter(FileExportPath);
            sw.WriteLine("Username" + "," + "Remark");
            hasInactive = false;

            if (dr.HasRows)
            {

                //String line;
                while (dr.Read())
                {
                    bool IsWrongName = false;
                    String line = dr["name"].ToString();
                    int i = 0;
                    while (i < users.Count())
                    {
                        if (users[i].WrongName == line)
                        {
                            IsWrongName = true;
                            break;
                        }
                        i++;
                    }
                    if (IsWrongName == true)
                    {
                        if (users[i].CorrectedName == "admin")
                        {
                            continue;
                        }
                        line = users[i].CorrectedName;
                    }



                    bool exist = DoesUserExist(line);

                    if (EVCUsersText != "all")
                    {
                        if (exist == Convert.ToBoolean(EVCUsersStatus))
                        {
                            if (hasInactive == false && exist == false)
                            {
                                hasInactive = true;

                            }
                            userLists.Add(new UserList { username = line, remark = (exist == true ? "Active" : "Inactive"), status = (exist == true ? "Active" : "Inactive") });
                            sw.WriteLine(line + "," + (exist == true ? "Active" : "Inactive"));
                            TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : " + line + " is " + exist.ToString());

                        }
                    }
                    else if (EVCUsersText == "all")
                    {
                        if (hasInactive == false && exist == false)
                        {
                            hasInactive = true;
                        }
                        sw.WriteLine(line + "," + (exist == true ? "Active" : "Inactive"));
                        TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : " + line + " is " + exist.ToString());
                    }

                }
                // Process line
            }
            else
            {
                MessageBox.Show("No data in database");
            }

            //Close the file
            sw.Close();
            //




            // Display Version Number
            MessageBox.Show("Connected to Oracle " + con.ServerVersion);

            // Close and Dispose OracleConnection



            con.Close();
            con.Dispose();
        }


    }
}
