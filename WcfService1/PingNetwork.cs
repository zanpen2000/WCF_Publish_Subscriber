using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class PingNetwork
    {
        static string url = "www.baidu.com";

        public static bool Ping
        {
            get
            {
                return check(url);
            }
        }

        static System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        static System.Net.NetworkInformation.PingReply res;

        private static bool check(string url)
        {
            try
            {
                res = ping.Send(url);
                if (res.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}