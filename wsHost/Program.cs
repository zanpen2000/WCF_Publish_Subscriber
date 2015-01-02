using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;


namespace wsHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            IPAddress ipaddr;
            if (args.Length > 0 && IPAddress.TryParse(args[0], out ipaddr))
            {
                ip = ipaddr.ToString();
            }

            int port = 8888;
            int tempport;
            if (args.Length > 1 && int.TryParse(args[1], out tempport))
            {
                port = tempport;
            }

            string uri = string.Format("net.tcp://{0}:{1}", ip, port);
            NetTcpBinding binding = new NetTcpBinding();
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            ServiceHost host = new ServiceHost(typeof(WcfService1.MessagePublishService));
            host.AddServiceEndpoint(typeof(WcfService1.IMessagePublishService), binding, uri);
            Console.WriteLine("启动消息发布服务器，地址:{0}", uri);

            WcfService1.MessageCenter.Instance.ListenerAdded += Instance_ListenerAdded;
            WcfService1.MessageCenter.Instance.ListenerRemoved += Instance_ListenerRemoved;
            WcfService1.MessageCenter.Instance.NotifyError += Instance_NotifyError;

            host.Open();
            Console.WriteLine("服务正在运行");
            EnterMessageInputMode();
            Console.WriteLine("正在关闭服务");
            host.Close();
            Console.WriteLine("服务已关闭");
            host = null;
            Console.ReadLine();
        }

        /// <summary>
        /// 消息发送模式
        /// </summary>
        private static void EnterMessageInputMode()
        {
            Console.WriteLine("输入要发送的消息，按回车键发送，输入@exit回车结束操作并关闭服务");
            string line;
            do
            {
                Console.Write(">>");
                line = Console.ReadLine();
                if ("@exit" == line)
                {
                    break;
                }
                if (string.Empty == line.Trim())
                {
                    Console.WriteLine("[{0}]不能发送空消息", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    continue;
                }
                WcfService1.MessageCenter.Instance.NotifyMessage(line);
                Console.WriteLine("[{0}]发送成功", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            } while (true);
        }

        static void Instance_NotifyError(object sender, WcfService1.MessageNotifyErrorEventArgs e)
        {
            Console.WriteLine("[{0}]消息发送失败！--IP:{1}; Port:{2}; Error:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort, e.Error.Message);
            Console.WriteLine("移除无效监听器……");
            WcfService1.MessageCenter.Instance.RemoveListener(e.Listener);
        }

        static void Instance_ListenerRemoved(object sender, WcfService1.MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]取消订阅-- From: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.ToString());
        }

        static void Instance_ListenerAdded(object sender, WcfService1.MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]订阅消息--From: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.ToString());
        }
    }
}
