using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class MessagePublishService : IMessagePublishService
    {
        public void Regist(string clientMac)
        {
            RemoteEndpointMessageProperty remoteEndpointProp = 
                OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            IListenerCallback callback = OperationContext.Current.GetCallbackChannel<IListenerCallback>();
            MessageCenter.Instance.AddListener(new Listener(clientMac, remoteEndpointProp.Address, remoteEndpointProp.Port, callback));
        }

        public void Unregist(string clientMac)
        {
            RemoteEndpointMessageProperty remoteEndpointProp =
               OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            IListenerCallback callback = OperationContext.Current.GetCallbackChannel<IListenerCallback>();
            MessageCenter.Instance.RemoveListener(new Listener(clientMac,remoteEndpointProp.Address, remoteEndpointProp.Port, callback));
        }
    }
}
