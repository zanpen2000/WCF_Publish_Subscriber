using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class SubscriberOnlineChecker
    {
        #region 单例实现
        private static readonly object _syncLock = new object();
        private static SubscriberOnlineChecker _instance;

        public static SubscriberOnlineChecker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SubscriberOnlineChecker();
                        }
                    }
                }
                return _instance;
            }
        }


        #endregion

        public static int Interval
        {
            get
            {
                int interval = 6000;
                try
                {
                    string istr = System.Configuration.ConfigurationManager.AppSettings["OnlineCheckInterval"];
                    interval = int.Parse(istr);
                }
                catch { }
                return interval;
            }
        }

        private static BackgroundWorker backgroundThread = new BackgroundWorker();

        

        public void Check()
        {
            backgroundThread.RunWorkerAsync();
        }

        private SubscriberOnlineChecker()
        {
            backgroundThread.DoWork += backgroundThread_DoWork;
            backgroundThread.Disposed += backgroundThread_Disposed;
            backgroundThread.RunWorkerCompleted += backgroundThread_RunWorkerCompleted;
        }

        void backgroundThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(Interval);
            Check();
        }

        void backgroundThread_Disposed(object sender, EventArgs e)
        {
            
        }

        void backgroundThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //检查订阅者是否在线,不在线的订阅者取消其订阅

            MessageCenter.Instance.NotifyMessage("1");
        }
    }
}