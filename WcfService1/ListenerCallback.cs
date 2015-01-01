using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfService1
{
    public class ListenerCallback:IListenerCallback
    {
        public event EventHandler<ListenerCallbackEventArgs> OnPublish = delegate { };

        public void Publish(string message)
        {
            if (this.OnPublish!=null)
            {
                OnPublish(this, new ListenerCallbackEventArgs(message));
            }
        }
    }
}
