using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IAckOrderMessageFactory
    {
        AcknowledgedOrderMessage GetAckFromOrderRequest(CreateOrderMessage msg);
    }

    public class AckOrderMessageFactory : IAckOrderMessageFactory
    {
        public AcknowledgedOrderMessage GetAckFromOrderRequest(CreateOrderMessage msg)
        {
            var returnMsg = new AcknowledgedOrderMessage
                                {
                                    ControlNumber = msg.ControlNumber,
                                    CustomerPO = msg.CustomerPO,
                                    BusinessPartnerCode = msg.BusinessPartnerCode,
                                    AckType = "AD"
                                };
            msg.LineItems.ToList().ForEach(i => returnMsg.Add(get_new_line(i)));
            return returnMsg;
        }

        private AcknowledgedOrderLine get_new_line(CustomerOrderLine i)
        {
            return new AcknowledgedOrderLine()
                       {
                           ActualPrice = i.RequestedPrice,
                           ActualQuantity = i.RequestedQuantity,
                           LineNumber = i.LineNumber.ToString(),
                           CustomerPartNumber = i.CustomerPartNumber
                       };
        }
    }
}
