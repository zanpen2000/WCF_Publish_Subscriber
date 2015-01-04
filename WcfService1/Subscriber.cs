using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Web;

namespace WcfService1
{
    public class Subscriber:IDisposable
    {
        public event EventHandler OnSubscribe = delegate { };

        private string _serviceURI;
        private ListenerCallback _listener = null;
        private IMessagePublishService _serviceProxy;

        public Subscriber(string serviceUrl, ListenerCallback callbacker)
        {
            this._serviceURI = serviceUrl;
            this._listener = callbacker;
        }

        public void  Subscribe()
        {
            NetTcpBinding binding = new NetTcpBinding();
            _serviceProxy = DuplexChannelFactory<IMessagePublishService>.CreateChannel(_listener, binding, new EndpointAddress(_serviceURI));
            _serviceProxy.Regist(GetMacAddressByNetworkInformation());

            if (OnSubscribe != null)
            {
                this.OnSubscribe(this, null);
            }

        }

        public void Dispose()
        {
            _serviceProxy.Unregist(GetMacAddressByNetworkInformation());
            (_serviceProxy as IDisposable).Dispose();
            _listener = null;
            _serviceProxy = null;
        }

        /// <summary>
        /// 通过网络适配器获取MAC地址
        /// </summary>
        /// <returns></returns>
        public string GetMacAddressByNetworkInformation()
        {
            string macAddress = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macAddress = adapter.GetPhysicalAddress().ToString();
                        for (int i = 1; i < 6; i++)
                        {
                            macAddress = macAddress.Insert(3 * i - 1, ":");
                        }
                        break;
                    }
                }

            }
            catch
            {
            }
            return macAddress;
        }
    }
}