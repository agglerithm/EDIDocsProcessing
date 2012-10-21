using System;
using System.Collections.Generic;
using AFPST.Common;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;

namespace FedexEDIDocsOut
{
    public class FedexOrderAckService:IOrderAckService
    {
        private readonly ICreateEdiContentFrom<CreateOrderMessage> _builder;
        private readonly IFileCreationService _fileSvc;

        public FedexOrderAckService(
                                    ICreateEdiContentFrom<CreateOrderMessage> builder,
                                    IFileCreationService fileSvc)
        { 
            _builder = builder;
            _fileSvc = fileSvc;
        }

        public void AcknowledgeAll(List<CreateOrderMessage> messages)
        {
            messages.ForEach(Acknowledge);
        }

        public void Acknowledge(CreateOrderMessage msg)
        { 
            var transaction = _builder.BuildFromMessage(msg);
            _fileSvc.SendFile(transaction.Value, msg.ControlNumber, 855);
        }
    }
}