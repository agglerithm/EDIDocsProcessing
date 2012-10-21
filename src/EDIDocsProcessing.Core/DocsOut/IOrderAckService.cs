using System.Collections.Generic;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IOrderAckService
    {
        void AcknowledgeAll(List<CreateOrderMessage> messages);
        void Acknowledge(CreateOrderMessage msg);
    }
}