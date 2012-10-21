using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Publishers;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using InitechEDIDocsIn;
using InitechEDIDocsOut;
using StructureMap.Configuration.DSL; 

namespace EDIDocsIn.config
{
    using AFPST.Common.Infrastructure;
    using AFPST.Common.Infrastructure.impl;
    using MassTransit;
    using MassTransit.Transports.Msmq;

    public class EdiDocsInRegistry : Registry
    {
        public EdiDocsInRegistry()
        {
            var config = new AFPSTConfiguration();
 


            For<INotificationSender>().Use<NotificationSender>()
                .Ctor<Queue>("notificationEndpoint").Is(config.GetQueue("EmailServiceHost", "EmailServiceEndpoint"))
                .Ctor<IEndpointCache>().Is(EndpointCacheFactory.New(ecf => ecf.AddTransportFactory<MsmqTransportFactory>()));
            For<IGenericDocumentParser>().Use<GenericDocParser<OrderRequestReceivedMessage>>();
            For<IInitech850LineParser>().Use<Initech850LineParser>();
            For<ICreateEdiContentFrom<OrderRequestReceivedMessage>>().Use<Initech855Creator>();
            For<ISegmentFactory>().Use<SegmentFactory>().Named("segFactory");
            For<IControlNumberRepository>().Use<ControlNumberRepository>().Named("controlNumRepo");
            For<IAssignDocumentsToPartners>().Use<DocumentPartnerAssigner>();
            For<IBusinessPartnerSpecificServiceResolver>().Use<BusinessPartnerSpecificServiceResolver>(); 
            For<IFileParser>().Use<InitechFileParser>() ; 
            For<IDocumentParser>().Use<Initech850Parser>(); 
            For<IDocumentParser>().Use<Initech997Parser>();  
            For<IEdiMessagePublisher>().Use<CreateOrderMessagePublisher>();
            For<IEdiMessagePublisher>().Use<ChangeOrderMessagePublisher>();
            For<IEdiMessagePublisher>().Use<ShippedOrderMessagePublisher>();
             
        }
    }
}