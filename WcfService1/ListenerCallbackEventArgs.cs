using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class ListenerCallbackEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ListenerCallbackEventArgs(string msg)
        {
            this.Message = msg;
        }
    }
}