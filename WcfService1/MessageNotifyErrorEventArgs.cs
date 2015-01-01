using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfService1
{
    public class MessageNotifyErrorEventArgs : EventArgs
    {
        public MessageNotifyErrorEventArgs(Listener listener, Exception error)
        {
            this.Listener = listener;
            this.Error = error;
        }

        public Listener Listener { get; private set; }

        public Exception Error { get; private set; }
    }
}
