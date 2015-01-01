using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfService1;

namespace wsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceUri = "net.tcp://127.0.0.1:8888";
            if (args.Length > 0)
            {
                serviceUri = args[0];
            }
            Console.WriteLine("[{0}]连接服务…… {1}", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), serviceUri);
            try
            {
                ListenerCallback listener = new ListenerCallback();
                listener.OnPublish += listener_OnPublish;

                using (Subscriber sub = new Subscriber(serviceUri, listener))
                {
                    sub.Subscribe();
                    Console.WriteLine("[{0}]连接成功！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                    Console.WriteLine("输入 @exit 断开连接。");
                    string line;
                    while (true)
                    {
                        Console.Write(">>");
                        line = Console.ReadLine();
                        if ("@exit" == line.Trim())
                        {
                            Console.WriteLine("[{0}]准备断开连接……", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[{0}]发生错误！--{1}", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), ex.Message);
            }
            Console.WriteLine("[{0}]断开连接！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
            Console.ReadLine();
        }

        static void listener_OnPublish(object sender, ListenerCallbackEventArgs e)
        {
            Console.WriteLine("那个啥，收到了消息：{0}", e.Message);
        }
    }
}
