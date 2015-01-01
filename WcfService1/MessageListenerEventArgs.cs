using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class MessageListenerEventArgs : EventArgs
    {
        public Listener Listener { get; private set; }

        public MessageListenerEventArgs(Listener listener)
        {
            this.Listener = listener;
        }
    }
}