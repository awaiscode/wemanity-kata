using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ServiceMultiProcess
{
    public partial class ClassMP: ServiceBase
    {
        //Global Variable
        Thread MyProcess;
        static string ConnString = "Your database connection string";        


        /// <summary>
        /// Class Constructor 
        /// </summary>
        public ClassMP()
        {
            try
            {
                InitializeComponent();
                MyProcess = new Thread(new ThreadStart(LaunchThreads));
            }
            catch (Exception ex)
            {
                Trace.WriteLog("Exception Occured at: " + DateTime.Now.ToString() + ex.ToString()); 
            }                 
        }

        protected override void OnStart(string[] array)
        {
            MyProcess.Start();
            Trace.WriteLog("Service is started at " + DateTime.Now.ToString());
        }

        protected override void OnStop()
        {
          
            MyProcess.Abort();
            Trace.WriteLog("Service is stopped at " + DateTime.Now.ToString());
        }


        /// <summary>
        /// To kill the processes according to desired timings
        /// </summary>
        private static void KillIdleProcesses()
        {            
            Trace.WriteLog("Kill Idle Processes started at: " + DateTime.Now.ToString()); 
            Process[] processes = Process.GetProcessesByName("EXCEL");
            for (int i = 0; i < processes.Count(); i++)
            {
                Process process = processes[i];
                try
                {
                    //If a process is running for more than three hours kill the process
                    if (process.StartTime < DateTime.Now.AddHours(-3))
                    {
                        process.Kill();
                    }
                }
                catch(Exception ex)
                {
                    Trace.WriteLog("Exception Details : " + DateTime.Now.ToString() + ex.ToString()); 
                }
            }
        }
        
        
        /// <summary>
        /// This method is responsible for launching threads of the same process
        /// Business Logic to launch a reporting application that servers reports for the users that demands reports from the web interface
        /// Oracle DB and a view have been used to fetch the data from Oracle if there are some report requests
        /// </summary>
        public static void LaunchThreads()
        {
         
            try
            {
                OracleConnection Conn = new OracleConnection(ConnString);
                OracleDataReader odr;
                int ProcessCounter;

                OracleCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.Text;                
                cmd.CommandText = "SELECT COUNT(*) AS CPT FROM VUserReportRequests";

                int Counter = 0;
                
                //Kind of infinitve loop until service is running
                while (Thread.CurrentThread.IsAlive)
                {
                    if (Counter == 0)
                    {
                        KillIdleProcesses();
                    }
                    try
                    {

                        if (!(Conn.State == ConnectionState.Open))
                        {
                            Conn.Open();
                        }
                        ProcessCounter = 0;                        
                        odr = cmd.ExecuteReader();
                        if (odr.Read())
                        {
                            ProcessCounter = Convert.ToInt32(odr.GetDecimal(0));
                        }
                        odr.Close();

                        if (ProcessCounter > 0)
                        {

                            Process[] processes = Process.GetProcessesByName("FILLREPORT");
                            
                            // To launch maximum 4 process at a time
                            if (processes.Count() < 4)
                            {
                                Process p = Process.Start(@"D:\\BusinessReportsApp\\CreateReport.exe");
                                Conn.Close();
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        Trace.WriteLog("Exception Details : " + DateTime.Now.ToString() + ex.ToString()); 
                    }
                    if (Counter == 180) Counter = 0; else Counter ++;
                }
                
                    Conn.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLog("Exception Details : " + DateTime.Now.ToString() + ex.ToString()); 
            }

        }

  
    }
}
