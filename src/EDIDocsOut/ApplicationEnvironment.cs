using System;
using AFPST.Common.Infrastructure;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using Castle.Core;
using Castle.Windsor;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using FedexEDIDocsOut;
using FlexEDIDocsOut;
using FluentNHibernate;
using FoxconnEDIDocsOut;

namespace EDIDocsOut
{
    public static class ApplicationEnvironment
    {
        public static IWindsorContainer Setup( )
        {

            
            //Container.GetContainer().AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net, "EdiDocsOut.log4net.xml"));

            //CommonSetup.Setup();
            //Container.Register<IFileUtilities, FileUtilities>();
            //Container.Register<IAddressSegmentCreator, FlexBillToAddressSegmentCreator>("FlexBillToAddressSegmentCreator");
            //Container.Register<IAddressSegmentCreator, FlexShipFromAddressSegmentCreator>("FlexShipFromAddressSegmentCreator");
            //Container.Register<IAddressSegmentCreator, FlexShipToAddressSegmentCreator>("FlexShipToAddressSegmentCreator");
            //Container.Register<IBusinessPartnerSpecificServiceResolver, BusinessPartnerSpecificServiceResolver>();
            //Container.Register<ICreateEdiContentFrom<InvoicedOrderMessage>, Fedex810Creator>("CreateEdiContentFromFEDEX810");
            //Container.Register<ICreateEdiContentFrom<OrderRequestReceivedMessage>, Fedex855Creator>("CreateEdiContentFromFEDEX850");
            
            //Container.Register<ICreateEdiContentFrom<OrderHasBeenShippedMessage>, Fedex856Creator>("CreateEdiContentFromFEDEXASN856");
            //Container.Register<ICreateEdiContentFrom<OrderIsBackorderedMessage>, Fedex856BackorderCreator>("CreateEdiContentFromFEDEXBackorder856");

            //Container.Register<ICreateEdiContentFrom<FinishedAndRawInventoryCountedList>, Foxconn846Creator>("CreateEdiContentFromFOX846");

            //Container.Register<ICreateEdiContentFrom<InvoicedOrderMessage>, Flextronics810Creator>("CreateEdiContentFromFLEX810");
            //Container.Register<ICreateEdiDocumentFrom<InvoicedOrderMessage>, CreateEdiDocumentFrom<InvoicedOrderMessage>>("EdiDocumentCreatorFLEX810");

            //Container.Register<ICreateEdiDocumentFrom<InvoicedOrderMessage>, CreateEdiDocumentFrom<InvoicedOrderMessage>>("EdiDocumentCreatorFEDEX810");
            //Container.Register<ICreateEdiDocumentFrom<OrderRequestReceivedMessage>, CreateEdiDocumentFrom<OrderRequestReceivedMessage>>("EdiDocumentCreatorFEDEX850");
            //Container.Register<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>, CreateEdiDocumentFrom<OrderHasBeenShippedMessage>>("EdiDocumentCreatorFEDEXASN856");
            //Container.Register<ICreateEdiDocumentFrom<OrderIsBackorderedMessage>, CreateEdiDocumentFrom<OrderIsBackorderedMessage>>("EdiDocumentCreatorFEDEXBackorder856");

            //Container.Register
            //    <ICreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>,
            //        CreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>>("EdiDocumentCreatorFOX846");

            //Container.Register<IBusinessPartnerResolver<OrderHasBeenShippedMessage>,
            //    BusinessPartnerResolver<OrderHasBeenShippedMessage>>("856");
            //Container.Register<IBusinessPartnerResolver<OrderIsBackorderedMessage>,
            //    BusinessPartnerResolver<OrderIsBackorderedMessage>>("856-Backorders");

            //Container.Register<IBusinessPartnerResolver<InvoicedOrderMessage>,
            //    BusinessPartnerResolver<InvoicedOrderMessage>>("810");
            //Container.Register<IBusinessPartnerResolver<OrderRequestReceivedMessage>,
            //    BusinessPartnerResolver<OrderRequestReceivedMessage>>("855");
            //Container.Register<IBusinessPartnerResolver<FinishedAndRawInventoryCountedList>,
            //    BusinessPartnerResolver<FinishedAndRawInventoryCountedList>>("846");
            //Container.Register<IEdiDocumentSaver, EdiDocumentSaver>();
            //Container.Register<IAcceptMessages, AcceptMessages>();
            

            ////Container.GetContainer().AddComponent<EdiDocsOutSubscriberStarter>();

            //Container.Register<IEDIResponseReferenceRecorder, IEDIResponseReferenceRecorder>();
            //Container.GetContainer().AddComponentLifeStyle<Subscriber<OrderHasBeenShippedMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<FaultSubscriber<OrderHasBeenShippedMessage>>(LifestyleType.Transient);
            //Container.Register<INotifier<OrderHasBeenShippedMessage>, ShippedOrderMessageNotifier>();

            //Container.GetContainer().AddComponentLifeStyle<Subscriber<OrderIsBackorderedMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<FaultSubscriber<OrderIsBackorderedMessage>>(LifestyleType.Transient);

            //Container.GetContainer().AddComponentLifeStyle<Subscriber<InvoicedOrderMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<FaultSubscriber<InvoicedOrderMessage>>(LifestyleType.Transient);
            //Container.Register<INotifier<InvoicedOrderMessage>, InvoicedOrderMessageNotifier>();

            //Container.GetContainer().AddComponentLifeStyle<Subscriber<OrderRequestReceivedMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<FaultSubscriber<OrderRequestReceivedMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<IgnoreSubscriber<OrderHasBeenShippedMessage>>(LifestyleType.Transient);
            //Container.GetContainer().AddComponentLifeStyle<IgnoreSubscriber<OrderRequestReceivedMessage>>(LifestyleType.Transient);

            //Container.Register<IExecutePostConsumeAction, ExecutePostConsumeAction>();
            //Container.RegisterWithImplementationAsKey<IPostActionSpecification, PublishEdiAsnSentSpecification>();

            //var configuration = Container.Resolve<IAFPSTConfiguration>();

            //Container.Register<INotificationSender, NotificationSender>(typeof(NotificationSender).Name)
            //    .WithDependencies(new { emailServiceEndpointUri = new Uri(configuration.EmailServiceEndpoint()) });   


//            var ediBuildValues = new EdiXmlBuildValues()
//                                     {
//                                         ElementDelimiter = "~",
//                                         SegmentDelimiter = "\n",
//                                         InterchangeQualifier = configuration.InterchangeQualifier,
//                                         FunctionGroupReceiverID = configuration.FunctionGroupReceiverID,
//                                         InterchangeReceiverID = configuration.InterchangeReceiverID,
//                                         Transport = TransportAgent.Fedex
//                                     };
            //Container.Register<IBuildValueFactory, FedExBuildValueFactory>("FEDEXBuildValueFactory", LifestyleType.Transient);
            //Container.Register<IBuildValueFactory, FlextronicsBuildValueFactory>("FLEXBuildValueFactory", LifestyleType.Transient);

            //Container.Register<ISegmentFactory, SegmentFactory>("segFact", LifestyleType.Transient);


            //var connectionKey = configuration.TestMode == false ? "SQLConnection" : "SQLConnectionTest";
            //var createnewTables = (configuration.RecreateTables()) && configuration.TestMode;

            //Container.Register<ISessionSource, EdiSessionSource>(typeof(EdiSessionSource).Name, LifestyleType.Transient)
            //    .WithDependencies(new { connectionKey, createnewTables });

            //Container.Register<IControlNumberRepository, ControlNumberRepository>("controlNumberRepository", LifestyleType.Transient);
            //Container.Register<IIncomingDocumentsRepository, IncomingDocumentsRepository>("incomingDocsRepository", LifestyleType.Transient);
//            Container.Register<IFileCreationService, FedexFileCreationService>("FEDEXFileService");
//            Container.Register<IFileCreationService, FlextronicsFileCreationService>("SOLE01FileService"); 


            return Container.GetContainer();
            
        }

    }
}