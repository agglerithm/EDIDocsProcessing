using Castle.Facilities.Logging;
using EDIDocsOut;
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Common.IO;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EdiMessages;
using FedexEDIDocsIn;
using FedexEDIDocsOut;
using FlexEDIDocsOut;

namespace MockShipMessagePublisher
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
            Container.Register<IShippedOrderMessagePublisher, ShippedOrderMessagePublisher>();
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

//            Container.Register<IFileCreationService, FedexFileCreationService>("FEDEXFileCreationService");
//            Container.Register<IFileCreationService, FlextronicsFileCreationService>("SOLE01FileCreationService"); 


  
            
        }

    }
}