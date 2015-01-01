using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WcfService1
{
    [ServiceContract(CallbackContract=typeof(IListenerCallback))]
    public interface IMessagePublishService
    {
        [OperationContract]
        void Regist();

        [OperationContract]
        void Unregist();
    }
}
