using System;
using AFPST.Common.Services.Logging;
using EdiMessages;
using MassTransit;

namespace EDIDocsProcessing.Core
{
    public class IgnoreSubscriber<T> : Consumes<T>.Selected where T : class, IEdiMessage
    {
        private IAcceptMessages _acceptMessages;

        public IgnoreSubscriber(IAcceptMessages acceptMessages)
        {
            _acceptMessages = acceptMessages;
        }

        public void Consume(T message)
        {
            Logger.Info(this, "Ignoring Message {0}", message);
        }

        public bool Accept(T message)
        {
            var reject = _acceptMessages.Accept(message) == false; 
            return reject;
        }
    }
}