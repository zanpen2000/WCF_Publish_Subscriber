using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService1
{
    public class Subscriber:IDisposable
    {
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
            _serviceProxy.Regist();

        }

        public void Dispose()
        {
            _serviceProxy.Unregist();
            (_serviceProxy as IDisposable).Dispose();
            _listener = null;
            _serviceProxy = null;
        }
    }
}