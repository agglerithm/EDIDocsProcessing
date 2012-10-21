
namespace EDIDocsProcessing.MessagingTests
{
    using AFPST.Common.Extensions;
    using Core;
    using Core.DocsOut;
    using Common;
    using EdiMessages;
    using MassTransit;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;
    using EDIDocsOut.config;
    using Tests;

    [TestFixture]
    public class BusinessPartnerResolverTester
    {
        private IBusinessPartnerResolver<OrderChangeRequestReceivedMessage> _sut;
        private IConsumer _consumer;
        [TestFixtureSetUp]
        public void SetUpForAllTests()
        { 
            StructureMapBootstrapper.Execute();
            MassTransitEdiDocsOutBootstrapper.Execute();
            _sut = ServiceLocator.Current.GetInstance<IBusinessPartnerResolver<OrderChangeRequestReceivedMessage>>();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

 

        [Test]
        public void can_run_consumer()
        {

            var lst = ServiceLocator.Current.GetAllInstances<IConsumer>();
              _consumer =   lst.Find(
                    s => s.GetType() == typeof (Subscriber<OrderChangeRequestReceivedMessage>));
            ((Subscriber<OrderChangeRequestReceivedMessage>)_consumer).Consume(buildMsg());
        }
        private OrderChangeRequestReceivedMessage buildMsg()
        {
            var msg = new OrderChangeRequestReceivedMessage() {BusinessPartnerCode = BusinessPartner.Initech.Code};
            msg.Add(new CustomerOrderChangeLine());
            return msg;
        }
        [TearDown]
        public void TearDownForEachTest()
        {

        }

        [TestFixtureTearDown]
        public void TearDownAfterAllTests()
        {

        }
    }
}
