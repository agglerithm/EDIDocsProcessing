using System.Linq;
using EDIDocsProcessing.Common.Publishers;
using EdiMessages;
using MassTransit;
using MassTransitContrib;

namespace EDIDocsProcessing.Core
{
    public interface IPostActionSpecification
    {
        bool IsSatisfiedBy(IEdiMessage message);
        void Execute(IEdiMessage message);
    }

    public class PublishEdiAsnSentSpecification : IPostActionSpecification
    {
        private readonly IMessagePublisher _publisher;

        public PublishEdiAsnSentSpecification(IMessagePublisher serviceBus)
        {
            _publisher = serviceBus;
        }

        public bool IsSatisfiedBy(IEdiMessage message)
        {
            return message.GetType() == typeof(OrderHasBeenShippedMessage);
        }

        public void Execute(IEdiMessage message)
        {
            var orderHasBeenShippedMessage = (OrderHasBeenShippedMessage) message;

            var ediAsnSentMessageToBeSent = new EdiAsnSentMessage()
            {
                ControlNumber = orderHasBeenShippedMessage.ControlNumber,
                BOL = orderHasBeenShippedMessage.BOL,
                LineNumbers = orderHasBeenShippedMessage.GetLineNumbers().ToList()
            };

            _publisher.Publish(ediAsnSentMessageToBeSent);
        }

    }
}