using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimedEvent);
            timer.Interval = 5000;//每5s执行一次。
            timer.Enabled = true;
        }

        public void TimedEvent(object sender,System.Timers.ElapsedEventArgs e)
        {
            //业务逻辑代码
        }

        protected override void OnStart(string[] args)
        {
            this.WriteLog("我的windows服务启动了");
            
        }

        protected override void OnStop()
        {
            this.WriteLog("我的windows服务停止了");
        }

        protected override void OnShutdown()
        {
            this.WriteLog("计算机关闭");
        }

        private void WriteLog(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
            FileInfo file = new FileInfo(path);
            if(!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw=new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + msg);

                }
            }
        }
    }
}
