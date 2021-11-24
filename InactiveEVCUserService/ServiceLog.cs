using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace InactiveEVCUserService
{
    public class ServiceLog
    {
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        /// <summary>  
        /// this function write Message to log file.  
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public static void SendEmail()
        {
            Int32 BufferSize = 128;
            List<CorrectedUser> users = new List<CorrectedUser>();
            string FileExportPath = "";
            bool hasInactive = false;
            List<UserList> userLists = new List<UserList>();
            string htmlstring = "<p>Dear Colleagues,</p> <p>Please kindly check on Inactive users today.</p>";
            EVCInactiveMailService ms = new EVCInactiveMailService();
            ms.WriteToFile("I'm here in Send mail");
            //bind all incorrected user name to collection
            ms.WriteToFile("before worng user....");
            try
            {
                using (var fileStream = File.OpenRead(@"C:\Window_Application\ConnectionToOracle\DailyEVCUserInactiveInAD\bin\Debug\Map Users.csv"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    //get all worng user list in bin Alter User.csv
                    String l;
                    ms.WriteToFile("=============Wrong Name==============");
                    while ((l = streamReader.ReadLine()) != null)
                    {
                        //if (l == "")
                        //    break;
                        CorrectedUser obj = new CorrectedUser();
                        string[] usersList = l.Split(',');

                        obj.WrongName = usersList[0];
                        obj.CorrectedName = usersList[1];
                        users.Add(obj);

                        ms.WriteToFile(obj.WrongName);

                    }
                }
            }
            catch (Exception e)
            {
                ms.WriteToFile(e.ToString());
            }
            //get Inactive users
            //string constr = "User Id=KIN_CHANNARITH;Password=AAbb!@#20211028;Data Source=10.12.4.209:1526/ora11g";
            try
            {
                string constr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
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


                FileExportPath = AppDomain.CurrentDomain.BaseDirectory + "Reports/[Inactive]User reconcile " + DateTime.Now.ToString("dd_MM_yyy") + ".csv";



                StreamWriter sw = new StreamWriter(FileExportPath);

                sw.WriteLine("Username" + "," + "Remark");
                ms.WriteToFile("=============Done header=============");
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


                        bool DoesUserExist(string userName)
                        {
                            using (var domainContext = new PrincipalContext(ContextType.Domain, "SMART.LOCAL"))
                            {
                                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                                {
                                    return foundUser != null;
                                }
                            }
                        }
                        bool exist = DoesUserExist(line);


                        if (exist == false)
                        {
                            ms.WriteToFile("It's false");
                            if (hasInactive == false && exist == false)
                            {
                                hasInactive = true;
                            }
                            userLists.Add(new UserList { username = line, remark = (exist == true ? "Active" : "Inactive"), status = (exist == true ? "Active" : "Inactive") });

                            sw.WriteLine(line + "," + (exist == true ? "Active" : "Inactive"));
                            //TextLogAddText(DateTime.Now.ToString("ddMMyyyhhmmss") + " : " + line + " is " + exist.ToString());

                        }


                    }
                    // Process line
                }
                else
                {
                    //MessageBox.Show("No data in database");
                }

                //Close the file
                sw.Close();
                //

                // Display Version Number
                //MessageBox.Show("Connected to Oracle " + con.ServerVersion);

                // Close and Dispose OracleConnection
                con.Close();
                con.Dispose();
                //end get all inactive users
            }
            catch (Exception e)
            {
                ms.WriteToFile(e.ToString());
            }

            //send mail
            Email(htmlstring, FileExportPath, hasInactive, userLists);


        }
        public static void Email(string htmlString, string filePath, bool IsInactive, List<UserList> InactiveUsers)
        {
            try
            {
                //List<UserList> InactiveUsers = new List<UserList>();
                EVCInactiveMailService ms = new EVCInactiveMailService();
                var fromAddress = new MailAddress(ConfigurationManager.AppSettings["FromMail"].ToString(), "[Testing] Billing EVC User");
                var toAddress = new MailAddress(ConfigurationManager.AppSettings["ToMail"].ToString(), "To Name");
                string fromPassword = ConfigurationManager.AppSettings["Password"].ToString();
                const string subject = "Daily Report for EVC users Inactive in AD";
                string body = htmlString;
                ms.WriteToFile("almost send");
                if (IsInactive == false)
                {
                    body = "<p>Dear Colleagues,</p> <p>There's no EVC user inactive in AD today.</p>";
                }

                //Attachment attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["Host"].ToString(),
                    Port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"].ToString()),
                    //EnableSsl = true,
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
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
                body = body + "<p>***Note: This is a system generated email, please do not reply. If you have any concern, please feel free to contact billing.prepaid@smart.com.kh *** </p> <hr style='border: solid 1px lightgrey;' > <b><i>Smart Axiata, CIO - Billing Prepaid </i></b>";
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true


                })
                {
                    //if (IsInactive == true)
                    //    message.Attachments.Add(attachment);
                    smtp.Send(message);
                }

            }
            catch (Exception e)
            {
              
            }
        }
    }
}
