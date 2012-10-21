using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using InitechEDIDocsIn;
using MassTransit;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.StructureMap
{
    [TestFixture]
    public class EdiDocsInStructureMapRegistriesTester
    {
        private IContainer _container;

        [SetUp]
        public void Setup()
        {
            StructureMapBootstrapper.Execute();
            ObjectFactory.Configure(x => x.For<IServiceBus>().Use<FakeServiceBus>());
            _container = ObjectFactory.Container;
        }

        [Test]
        public void should_resolve_dependencies_correctly()
        {
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IBusinessPartnerSpecificServiceResolver, BusinessPartnerSpecificServiceResolver>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IFileUtilities, FileUtilities>(_container);
//            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IDocumentParser, Initech850Parser>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IHierarchySplitter, HierarchySplitter>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IAddressParser, AddressParser>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IInitech850LineParser, Initech850LineParser>(_container);
            //StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IMessagePublisher, CreateOrderMessagePublisher>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IAFPSTConfiguration, AFPSTConfiguration>(_container);
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ISegmentFactory, SegmentFactory>(_container, "segFactory");
            //StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<ICreateEdiContentFrom<OrderRequestReceivedMessage>, Initech855Creator>(_container);
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IControlNumberRepository, ControlNumberRepository>(_container, "controlNumRepo");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IIncomingDocumentsRepository, IncomingDocumentsRepository>(_container, "incomingDocsRepository");
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<INotificationSender, NotificationSender>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IEdiFileProcessingService, EdiFileProcessingService>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IEDIResponseReferenceRecorder, EDIResponseReferenceRecorder>(_container);
        }
    }
}