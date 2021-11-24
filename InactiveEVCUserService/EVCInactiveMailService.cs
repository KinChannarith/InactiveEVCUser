using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
namespace InactiveEVCUserService
{
    public partial class EVCInactiveMailService : ServiceBase
    {
        Timer timer = new Timer();
        int getCallType = Convert.ToInt32(ConfigurationManager.AppSettings["CallType"]);
        int strTime = Convert.ToInt32(ConfigurationManager.AppSettings["callDuration"]);
        public EVCInactiveMailService()
        {

            InitializeComponent();

            timer = new Timer();
            getCallType = Convert.ToInt32(ConfigurationManager.AppSettings["CallType"]);
            strTime = Convert.ToInt32(ConfigurationManager.AppSettings["callDuration"]);

            if (getCallType == 1)
            {
                timer = new System.Timers.Timer();

                double inter = (double)GetNextInterval();
                WriteToFile(inter.ToString());
                timer.Interval = inter;
                timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
            else
            {
                timer = new System.Timers.Timer();
                timer.Interval = strTime * 1000;
                timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }




        }
        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //string Msg = "Hi ! This is DailyMailSchedulerService mail.";//whatever msg u want to send write here.  
            //    
            WriteToFile("I'm here");
            // Here you can write the   
            //ServiceLog.SendEmail();
            //Test Mail
            List<UserList> tests = new List<UserList>();
            ServiceLog.Email("Test Kok", "", false, tests);

            if (getCallType == 1)
            {
                timer.Stop();
                System.Threading.Thread.Sleep(1000000);
                SetTimer();
            }
        }
        private double GetNextInterval()
        {
            String timeString = ConfigurationManager.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            int x;
            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddDays(1) - System.DateTime.Now;//Here you can increase the timer interval based on your requirments.   
            }
            return ts.TotalMilliseconds;
        }
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                timer.Interval = inter;
                timer.Start();
            }
            catch (Exception ex)
            {
            }
        }


        protected override void OnStart(string[] args)
        {
            //WriteToFile("Service is started at " + DateTime.Now);
            timer.AutoReset = true;
            timer.Enabled = true;
            WriteToFile("Daily Reporting service started " + DateTime.Now);
        }

        protected override void OnStop()
        {
            timer.AutoReset = false;
            timer.Enabled = false;
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
