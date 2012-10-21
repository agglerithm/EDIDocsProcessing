using System;
using System.Reflection;
using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using MassTransit;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.StructureMap
{
    [TestFixture]
    public class EdiDocsOutStructureMapRegistryTester
    {
        private IContainer _container;
        private IAFPSTConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            StructureMapBootstrapper.Execute();
            ObjectFactory.Configure(x =>
                                        {
                                            x.For<IServiceBus>().Use<FakeServiceBus>();
                                            x.For<IEndpointCache>().Use<FakeEndpointResolver>();
                                        });

            _container = ObjectFactory.Container;
            _configuration = new AFPSTConfiguration();
        }

        [Test]
        public void should_resolve_structuremap_dependices_correctly()
        {
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IFileUtilities, FileUtilities>(_container);
         
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IBusinessPartnerSpecificServiceResolver, BusinessPartnerSpecificServiceResolver>(_container);

            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiContentFrom<InvoicedOrderMessage>, Initech810Creator>(_container, "CreateEdiContentFromInitech810");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiContentFrom<OrderHasBeenShippedMessage>, Initech856Creator>(_container, "CreateEdiContentFromInitechASN856");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiContentFrom<OrderIsBackorderedMessage>, Initech856BackorderCreator>(_container, "CreateEdiContentFromInitechBackorder856");

            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiContentFrom<FinishedAndRawInventoryCountedList>, Foxconn846Creator>(_container, "CreateEdiContentFromFOX846");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiContentFrom<InvoicedOrderMessage>, Flextronics810Creator>(_container, "CreateEdiContentFromFLEX810");

            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<InvoicedOrderMessage>, CreateEdiDocumentFrom<InvoicedOrderMessage>>(_container, "EdiDocumentCreatorFLEX810");

           // StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<InvoicedOrderMessage>, CreateEdiDocumentFrom<InvoicedOrderMessage>>(_container, "EdiDocumentCreatorInitech810");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<OrderRequestReceivedMessage>, CreateEdiDocumentFrom<OrderRequestReceivedMessage>>(_container, "EdiDocumentCreatorInitech850");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>, CreateEdiDocumentFrom<OrderHasBeenShippedMessage>>(_container, "EdiDocumentCreatorInitechASN856");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<OrderIsBackorderedMessage>, CreateEdiDocumentFrom<OrderIsBackorderedMessage>>(_container, "EdiDocumentCreatorInitechBackorder856");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ICreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>, CreateEdiDocumentFrom<FinishedAndRawInventoryCountedList>>(_container, "EdiDocumentCreatorFOX846");
            
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBusinessPartnerResolver<OrderHasBeenShippedMessage>, BusinessPartnerResolver<OrderHasBeenShippedMessage>>(_container, "856");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBusinessPartnerResolver<OrderIsBackorderedMessage>, BusinessPartnerResolver<OrderIsBackorderedMessage>>(_container, "856-Backorders");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBusinessPartnerResolver<InvoicedOrderMessage>, BusinessPartnerResolver<InvoicedOrderMessage>>(_container, "810");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBusinessPartnerResolver<OrderRequestReceivedMessage>, BusinessPartnerResolver<OrderRequestReceivedMessage>>(_container, "855");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBusinessPartnerResolver<FinishedAndRawInventoryCountedList>, BusinessPartnerResolver<FinishedAndRawInventoryCountedList>>(_container, "846");


            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IEdiDocumentSaver, EdiDocumentSaver>(_container);
            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IAcceptMessages, AcceptMessages>(_container);

            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IEDIResponseReferenceRecorder, EDIResponseReferenceRecorder>(_container);

//            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<INotificationSender, NotificationSender>(_container, typeof(NotificationSender).Name);

//            var result = ObjectFactory.Container.GetInstance<INotificationSender>();
//
//            var uri = result.GetPrivateFieldsValue<Uri>("_emailServiceEndpointUri");
//            Assert.That(uri.AbsoluteUri, Is.EqualTo(_configuration.EmailServiceEndpoint()));
//            
//            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<INotifier<OrderHasBeenShippedMessage>, ShippedOrderMessageNotifier>(_container);
//            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<INotifier<OrderIsBackorderedMessage>, BackorderedOrderMessageNotifier>(_container);
//            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<INotifier<InvoicedOrderMessage>, InvoicedOrderMessageNotifier>(_container);

            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<Subscriber<OrderHasBeenShippedMessage>>(_container);
            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<FaultSubscriber<OrderHasBeenShippedMessage>>(_container);

            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<Subscriber<OrderIsBackorderedMessage>>(_container);
            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<FaultSubscriber<OrderIsBackorderedMessage>>(_container);

            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<Subscriber<InvoicedOrderMessage>>(_container);
            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<FaultSubscriber<InvoicedOrderMessage>>(_container);

            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<Subscriber<OrderRequestReceivedMessage>>(_container);
            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<FaultSubscriber<OrderRequestReceivedMessage>>(_container);

            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<IgnoreSubscriber<OrderHasBeenShippedMessage>>(_container);
            StructureMapTestHelper.AssertConcreteTypeResolvesCorretly<IgnoreSubscriber<OrderRequestReceivedMessage>>(_container);

            StructureMapTestHelper.AssertDefaultConcreteTypeResolvesCorrectly<IExecutePostConsumeAction, ExecutePostConsumeAction>(_container);

            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IPostActionSpecification, PublishEdiAsnSentSpecification>(_container, typeof(PublishEdiAsnSentSpecification).Name);

            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBuildValueFactory, InitechBuildValueFactory>(_container, "InitechBuildValueFactory");
            //StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IBuildValueFactory, FlextronicsBuildValueFactory>(_container, "FLEXBuildValueFactory");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<ISegmentFactory, SegmentFactory>(_container, "segFact");

            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IControlNumberRepository, ControlNumberRepository>(_container, "controlNumberRepository");
            StructureMapTestHelper.AssertResolvesCorrectlyWithKey<IIncomingDocumentsRepository, IncomingDocumentsRepository>(_container, "incomingDocsRepository");
        }
    }

    public static class TestHelperExtensions
    {
        public static T GetPrivateFieldsValue<T>(this object obj, string fieldName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) throw new ArgumentOutOfRangeException("fieldName", string.Format("Field {0} was not found in Type {1}", fieldName, obj.GetType().FullName));
            return (T)field.GetValue(obj);
        }
    }

    public class FakeEndpointResolver : IEndpointCache
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEndpoint GetEndpoint(string uriString)
        {
            throw new NotImplementedException();
        }

        public IEndpoint GetEndpoint(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}