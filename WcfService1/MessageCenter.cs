using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WcfService1
{
    public class MessageCenter
    {
        #region 单例实现
        private static readonly object _syncLock = new object();
        private static MessageCenter _instance;

        public static MessageCenter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MessageCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private MessageCenter() { }
        #endregion

        public event EventHandler<MessageListenerEventArgs> ListenerAdded;
        public event EventHandler<MessageListenerEventArgs> ListenerRemoved;
        public event EventHandler<MessageNotifyErrorEventArgs> NotifyError;

        private List<Listener> _listeners = new List<Listener>(0);

        public void AddListener(Listener listener)
        {
            lock (_syncLock)
            {
                if (_listeners.Contains(listener))
                {
                    throw new InvalidOperationException("重复注册相同的监听器");
                }
                _listeners.Add(listener);
            }
            if (ListenerAdded != null)
            {
                this.ListenerAdded(this, new MessageListenerEventArgs(listener));
            }
        }

        public void RemoveListener(Listener listener)
        {
            lock (_syncLock)
            {
                if (_listeners.Contains(listener))
                {
                    this._listeners.Remove(listener);
                }
                else
                {
                    throw new InvalidOperationException("要移除的监听器不存在");
                }
            }
            if (ListenerRemoved != null)
            {
                this.ListenerRemoved(this, new MessageListenerEventArgs(listener));
            }
        }

        public void NotifyMessage(string message)
        {
            Listener[] listeners = _listeners.ToArray();
            foreach (Listener lstn in listeners)
            {
                try
                {
                    lstn.Notify(message);
                }
                catch (Exception ex)
                {
                    OnNotifyError(lstn, ex);
                }
            }
        }
        private void OnNotifyError(Listener listener, Exception ex)
        {
            if (this.NotifyError == null)
            {
                return;
            }

            MessageNotifyErrorEventArgs args = new MessageNotifyErrorEventArgs(listener, ex);
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                this.NotifyError(this, state as MessageNotifyErrorEventArgs);
            }, args);
        }
    }
}