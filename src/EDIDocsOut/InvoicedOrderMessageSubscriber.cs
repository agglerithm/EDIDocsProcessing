////////////////////////////////////Generic 'Subscriber' class makes this obsolete!//////////////////
//using EDIDocsProcessing.Core.DocsOut;
//using EDIDocsProcessing.Core.DocsOut.EDIStructures;
//using EdiMessages;
//using MassTransit;
//
//namespace EDIDocsOut
//{
//    public class InvoicedOrderMessageSubscriber : Consumes<InvoicedOrderMessage>.All
//    {
//        private readonly IBusinessPartnerResolver<InvoicedOrderMessage> _businessPartnerResolver;
//        private readonly ICreateEdiDocumentFrom<InvoicedOrderMessage> _createEdiDocumentFrom;
//        private readonly IEdiDocumentSaver _ediDocumentSaver;
//
//        public InvoicedOrderMessageSubscriber(IBusinessPartnerResolver<InvoicedOrderMessage> businessPartnerResolver,
//                                             ICreateEdiDocumentFrom<InvoicedOrderMessage> createEdiDocumentFrom,
//                                             IEdiDocumentSaver ediDocumentSaver)
//        {
//            _businessPartnerResolver = businessPartnerResolver;
//            _createEdiDocumentFrom = createEdiDocumentFrom;
//            _ediDocumentSaver = ediDocumentSaver;
//        }
//
//        public void Consume(InvoicedOrderMessage message)
//        {
//            var bp855Creator = _businessPartnerResolver.ResolveFrom(message);
//            EDIXmlInterchangeControl document = _createEdiDocumentFrom.CreateDocumentWith(bp855Creator, message);
//            _ediDocumentSaver.Save(document);
//        }
//    }
//}