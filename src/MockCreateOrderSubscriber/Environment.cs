 
using Castle.Facilities.Logging;
using EDIDocsOut;
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EdiMessages;
using FedexEDIDocsOut;

namespace MockCreateOrderSubscriber
{
    public class Environment
    {
        public static void Setup( )
        { 
            Container.CommonSetup();

            Container.GetContainer().AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net, "MockOrder.log4net.xml"));
            Container.Register<ICreateEdiContentFrom<InvoicedOrderMessage>, Fedex810Creator>("CreateEdiContentFromFEDEX810");
            Container.Register<ICreateEdiContentFrom<CreateOrderMessage>, Fedex855Creator>("CreateEdiContentFromFEDEX850");
            Container.Register<ICreateEdiContentFrom<ShippedOrderMessage>, Fedex856Creator>("CreateEdiContentFromFEDEX856"); 

            Container.Register<ICreateEdiDocumentFrom<InvoicedOrderMessage>, CreateEdiDocumentFrom<InvoicedOrderMessage>>("EdiDocumentCreatorFEDEX810");
            Container.Register<ICreateEdiDocumentFrom<ShippedOrderMessage>, CreateEdiDocumentFrom<ShippedOrderMessage>>("EdiDocumentCreatorFEDEX856");
            Container.Register<ICreateEdiDocumentFrom<CreateOrderMessage>, CreateEdiDocumentFrom<CreateOrderMessage>>("EdiDocumentCreatorFEDEX850");

            Container.Register<IBusinessPartnerResolver<ShippedOrderMessage>,
                BusinessPartnerResolver<ShippedOrderMessage>>("856");
            Container.Register<IBusinessPartnerResolver<InvoicedOrderMessage>,
                BusinessPartnerResolver<InvoicedOrderMessage>>("810");
            Container.Register<IBusinessPartnerResolver<CreateOrderMessage>,
                BusinessPartnerResolver<CreateOrderMessage>>("850");
            Container.Register<IEdiDocumentSaver, EdiDocumentSaver>();

            Container.GetContainer().AddComponent<EdiDocsOutService>();

            //Container.GetContainer().AddComponent<ShippedOrderMessageSubscriber>();
            Container.GetContainer().AddComponent<Subscriber<ShippedOrderMessage>>();
            Container.GetContainer().AddComponent<Subscriber<InvoicedOrderMessage>>();


            var configuration = Container.Resolve<IEdiConfiguration>();

            var connectionKey = configuration.TestMode == false ? "SQLConnection" : "SQLConnectionTest";

  
            Container.Register<IFileCreationService, FedexFileCreationService>(); 


  
            
        }

    }
}