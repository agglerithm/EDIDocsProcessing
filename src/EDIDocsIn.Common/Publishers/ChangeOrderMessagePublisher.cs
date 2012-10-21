namespace EDIDocsProcessing.Common.Publishers
{
    using System.Collections.Generic;
    using System.Linq;
    using EdiMessages;
    using Extensions;
    using MassTransit;

    public class ChangeOrderMessagePublisher : IEdiMessagePublisher
    {
        public void PublishCreateOrderMessages(IEnumerable<OrderChangeRequestReceivedMessage> customerOrders)
        {
            customerOrders.ForEach(publish);
        }

        private void publish(OrderChangeRequestReceivedMessage message)
        {
            Bus.Instance().Publish(message);
        }

        public void PublishMessages(IEnumerable<IEdiMessage> customerOrders)
        {
            PublishCreateOrderMessages(customerOrders.Select(o => (OrderChangeRequestReceivedMessage)o));
        }

        public bool CanPublish(IEdiMessage msg)
        {
            return msg.GetType() == typeof(OrderChangeRequestReceivedMessage);
        }

        public void PublishMessage(IEdiMessage ediMessage)
        {
            publish((OrderChangeRequestReceivedMessage)ediMessage);
        }
    }
}