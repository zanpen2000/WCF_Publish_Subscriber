using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Listener
    {
        public string ClientMac { get; private set; }
        public string FromIP { get; private set; }
        public int FromPort { get; private set; }
        private IListenerCallback _innerListener;
        public Listener(string clientMac, string fromIP, int fromPort, IListenerCallback innerListener)
        {
            this.ClientMac = clientMac;
            this.FromIP = fromIP;
            this.FromPort = fromPort;
            this._innerListener = innerListener;
        }

        public void Notify(string message)
        {
            _innerListener.Publish(message);
        }

        public override bool Equals(object obj)
        {
            bool eq = base.Equals(obj);
            if (!eq)
            {
                Listener lstn = obj as Listener;
                if (lstn._innerListener.Equals(this._innerListener))
                {
                    eq = true;
                }
            }
            return eq;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}({2})", this.FromIP, this.FromPort, this.ClientMac);
        }
    }
}