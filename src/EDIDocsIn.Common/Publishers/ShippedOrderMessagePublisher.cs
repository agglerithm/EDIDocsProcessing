using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;
using MassTransit;

namespace EDIDocsProcessing.Common.Publishers
{
    public class ShippedOrderMessagePublisher : IEdiMessagePublisher
    { 

        public ShippedOrderMessagePublisher( )
        { 
        }

        public void PublishShippedOrderMessages(IEnumerable<OrderHasBeenShippedMessage> shippedOrders)
        {
            shippedOrders.ForEach(publish);
        }

        private void publish(OrderHasBeenShippedMessage message)
        {
            Bus.Instance().Publish(message);
        }

        public void PublishMessages(IEnumerable<IEdiMessage> customerOrders)
        {
            PublishShippedOrderMessages(customerOrders.Select(o => (OrderHasBeenShippedMessage) o));
        }

        public bool CanPublish(IEdiMessage msg)
        {
            return msg.GetType() == typeof(OrderHasBeenShippedMessage);
        }

        public void PublishMessage(IEdiMessage ediMessage)
        {
            publish((OrderHasBeenShippedMessage) ediMessage);
        }
    }
}