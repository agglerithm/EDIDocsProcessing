using System.Collections.Generic;
using EdiMessages;

namespace EDIDocsProcessing.Common.Publishers
{
    public interface IEdiMessagePublisher
    {
        void PublishMessages(IEnumerable<IEdiMessage> customerOrders);
        bool CanPublish(IEdiMessage msg);
        void PublishMessage(IEdiMessage ediMessage);
    }
}