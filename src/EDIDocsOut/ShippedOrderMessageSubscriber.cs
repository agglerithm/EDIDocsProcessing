////////////////////////////////////Generic 'Subscriber' class makes this obsolete!//////////////////
//using System;
//using System.Transactions;
//using Castle.Core.Logging;
//using EDIDocsProcessing.Core.DocsOut;
//using EDIDocsProcessing.Core.DocsOut.EDIStructures;
//using EdiMessages;
//using MassTransit;
//
//namespace EDIDocsOut
//{
//    public class ShippedOrderMessageSubscriber : Consumes<ShippedOrderMessage>.All
//    {
//
//        private ILogger _logger = NullLogger.Instance;
//        public ILogger Logger
//        {
//            get { return _logger; }
//            set { _logger = value; }
//        }
//
//        private readonly IBusinessPartnerResolver<ShippedOrderMessage> _businessPartnerResolver;
//        private readonly ICreateEdiDocumentFrom<ShippedOrderMessage> _ediDocumentCreator;
//        private readonly IEdiDocumentSaver _ediDocumentSaver;
//
//        public ShippedOrderMessageSubscriber(IBusinessPartnerResolver<ShippedOrderMessage> businessPartnerResolver,
//                                             ICreateEdiDocumentFrom<ShippedOrderMessage> ediDocumentCreator,
//                                             IEdiDocumentSaver ediDocumentSaver)
//        {
//            _businessPartnerResolver = businessPartnerResolver;
//            _ediDocumentCreator = ediDocumentCreator;
//            _ediDocumentSaver = ediDocumentSaver;
//        }
//
//        #region All Members
//
//        public void Consume(ShippedOrderMessage message)
//        {
//                try
//                {
//                    ICreateEdiContentFrom<ShippedOrderMessage> bp856Creator =
//                        _businessPartnerResolver.ResolveFrom(message);
//
//                    EDIXmlInterchangeControl document = _ediDocumentCreator.CreateDocumentWith(bp856Creator, message);
//
//                    _ediDocumentSaver.Save(document);
//
//                    Logger.Info("Document Saved: " + document.Value);
//                }
//                catch (Exception exception)
//                {
//                    if (CurrentMessage.Headers.RetryCount < 5)
//                    {
//                        CurrentMessage.RetryLater();
//                        Logger.ErrorFormat(exception, "Error processing message {0}", message);
//                    }
//                    else
//                    {
//                        Logger.ErrorFormat(exception, "Message could not be processed and will be thrown out. {0}", message);
//                    }
//                }
//        }
//
//        #endregion
//    }
//}