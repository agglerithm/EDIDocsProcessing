using System;
using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using FluentNHibernate;
using StructureMap.Configuration.DSL;

namespace EDIDocsProcessing.Core.Config
{
    public class EdiDocsProcessingCoreRegistry : Registry
    {
        public EdiDocsProcessingCoreRegistry(bool testMode)
        {
            IAFPSTConfiguration config = new AFPSTConfiguration();

            var connectionKey = testMode == false ? "SQLConnection" : "SQLConnectionTest";
            var createnewTables = testMode;

            For<ISessionSource>().Singleton().Use<EdiSessionSource>()
              .Ctor<string>("connectionKey").Is(connectionKey)
              .Ctor<bool>("createnewTables").Is(createnewTables);

            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();
                     });


            For<IIncomingDocumentsRepository>().Use<IncomingDocumentsRepository>().Named("incomingDocsRepository");

            For<ICreateEdiDocumentFrom<InvoicedOrderMessage>>().Use<CreateEdiDocumentFrom<InvoicedOrderMessage>>();
            For<ICreateEdiDocumentFrom<InvoicedOrderMessage>>().Use<CreateEdiDocumentFrom<InvoicedOrderMessage>>();
            For<ICreateEdiDocumentFrom<OrderRequestReceivedMessage>>().Use<CreateEdiDocumentFrom<OrderRequestReceivedMessage>>();
            For<ICreateEdiDocumentFrom<OrderChangeRequestReceivedMessage>>().Use<CreateEdiDocumentFrom<OrderChangeRequestReceivedMessage>>();
            For<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>>().Use<CreateEdiDocumentFrom<OrderHasBeenShippedMessage>>();
            For<ICreateEdiDocumentFrom<OrderIsBackorderedMessage>>().Use<CreateEdiDocumentFrom<OrderIsBackorderedMessage>>();
            For<ICreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>>().Use<CreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>>();

            For<IBusinessPartnerResolver<OrderHasBeenShippedMessage>>().Use<BusinessPartnerResolver<OrderHasBeenShippedMessage>>().Named("856");
            For<IBusinessPartnerResolver<OrderIsBackorderedMessage>>().Use<BusinessPartnerResolver<OrderIsBackorderedMessage>>().Named("856-Backorders");
            For<IBusinessPartnerResolver<InvoicedOrderMessage>>().Use<BusinessPartnerResolver<InvoicedOrderMessage>>().Named("810");
            For<IBusinessPartnerResolver<OrderRequestReceivedMessage>>().Use<BusinessPartnerResolver<OrderRequestReceivedMessage>>().Named("855");
            For<IBusinessPartnerResolver<FinishedAndRawInventoryCountedList>>().Use<BusinessPartnerResolver<FinishedAndRawInventoryCountedList>>().Named("846");
            For<IBusinessPartnerResolver<OrderChangeRequestReceivedMessage>>().Use<BusinessPartnerResolver<OrderChangeRequestReceivedMessage>>().Named("855-Change");
            For<INotifier<OrderHasBeenShippedMessage>>().Use<ShippedOrderMessageNotifier>();
            For<INotifier<OrderIsBackorderedMessage>>().Use<BackorderedOrderMessageNotifier>();
            For<INotifier<InvoicedOrderMessage>>().Use<InvoicedOrderMessageNotifier>();
            For<INotifier<OrderRequestReceivedMessage>>().Use<OrderRequestReceivedMessageNotifier>();

            For<IPostActionSpecification>().Use<PublishEdiAsnSentSpecification>().Named(typeof(PublishEdiAsnSentSpecification).Name);


 

        }
    }
}