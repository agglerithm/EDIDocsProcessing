using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;
using MassTransit;

namespace EDIDocsProcessing.Common.Publishers
{
    public class CreateOrderMessagePublisher : IEdiMessagePublisher
    {
        public void PublishCreateOrderMessages(IEnumerable<OrderRequestReceivedMessage> customerOrders)
        {
            customerOrders.ForEach(publish);
        }

        private void publish(OrderRequestReceivedMessage message)
        { 
            Bus.Instance().Publish(message);
        }

        public void PublishMessages(IEnumerable<IEdiMessage> customerOrders)
        {
            PublishCreateOrderMessages(customerOrders.Select(o => (OrderRequestReceivedMessage)o));
        }

        public bool CanPublish(IEdiMessage msg)
        {
            return msg.GetType() == typeof (OrderRequestReceivedMessage);
        }

        public void PublishMessage(IEdiMessage ediMessage)
        {
            publish((OrderRequestReceivedMessage) ediMessage);
        }
    }
}