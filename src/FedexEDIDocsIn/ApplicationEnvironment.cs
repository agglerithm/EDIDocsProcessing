using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using Castle.Core;
using Castle.Windsor;
using EDIDocsProcessing.Common;


using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using FedexEDIDocsOut;
using FluentNHibernate; 

namespace FedexEDIDocsIn
{
    public static class ApplicationEnvironment
    {
        public static IWindsorContainer Setup()
        {
            //Container.GetContainer().AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net, "EdiDocsIn.log4net.xml"));
            CommonSetup.Setup();
            Container.Register<IBusinessPartnerSpecificServiceResolver, BusinessPartnerSpecificServiceResolver>(); 
            Container.Register<IFileUtilities, FileUtilities>("fileUtilities"); 
       //     Container.Register<IFileCreationService, FedexFileCreationService>("FEDEXFileService");
            Container.Register<IDocumentParser<OrderRequestReceivedMessage>, Fedex850Parser>();
            Container.Register<ISplitter, Splitter>();
            Container.Register<IAddressParser, AddressParser>();
            Container.Register<IPOLineParser, Fedex850LineParser>();
            Container.Register<ICreateOrderMessagePublisher, CreateOrderMessagePublisher>();
            Container.Register<IAFPSTConfiguration, AFPSTConfiguration>();  
            Container.Register<ICreateEdiContentFrom<OrderRequestReceivedMessage>, Fedex855Creator>();

            Container.Register<IBuildValueFactory, FedExBuildValueFactory>();
            Container.Register<ISegmentFactory, SegmentFactory>("segFactory") ;


            var configuration = Container.Resolve<IAFPSTConfiguration>();


            var connectionKey = configuration.TestMode == false ? "SQLConnection" : "SQLConnectionTest";
            var createnewTables = configuration.TestMode;

            Container.Register<ISessionSource, EdiSessionSource>(typeof(EdiSessionSource).Name)
                .WithDependencies(new { connectionKey,createnewTables });

            Container.Register<IControlNumberRepository, ControlNumberRepository>("controlNumRepo", LifestyleType.Transient);
            Container.Register<IIncomingDocumentsRepository, IncomingDocumentsRepository>("incomingDocsRepository", LifestyleType.Transient);
            Container.Register<IFileProcessingService,FedexFileProcessingService>();
            Container.Register<IEDIResponseReferenceRecorder, EDIResponseReferenceRecorder>();
            Container.Register<IFedexFileParser,
                FedexFileParser>("srv").WithDependencies(new
                {
                    elDelim = "~",
                    segDelim = @"\"
                });
 
            return Container.GetContainer();
        }


 
    }
}