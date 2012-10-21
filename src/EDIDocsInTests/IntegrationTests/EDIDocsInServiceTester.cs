using System;
using EDIDocsIn;
using EDIDocsIn.config;
using MassTransit;
using MassTransit.Pipeline;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class EDIDocsInServiceTester
    {
        private EdiDocsInService _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            ObjectFactory.Configure(x => x.For<IServiceBus>().Use<FakeServiceBus>());

            _sut = new EdiDocsInService();
        }

        [Test]
        public void can_start_service()
        {
            _sut.Start(true);
        }
    }

    public class FakeServiceBus : IServiceBus
    {
        #region IServiceBus Members

        public void Dispose()
        {
        }

        public UnsubscribeAction Subscribe<T>(Action<T> callback) where T : class
        {
            throw new NotImplementedException();
        }

        public UnsubscribeAction Subscribe<T>(Action<T> callback, Predicate<T> condition) where T : class
        {
            throw new NotImplementedException();
        }

        public UnsubscribeAction Subscribe<T>(T consumer) where T : class
        {
            throw new NotImplementedException();
        }

        public UnsubscribeAction Subscribe<TConsumer>() where TConsumer : class
        {
            return unsubscribe;
        }

        public UnsubscribeAction Subscribe(Type consumerType)
        {
            throw new NotImplementedException();
        }

        public UnsubscribeAction SubscribeConsumer<T>(Func<T, Action<T>> getConsumerAction) where T : class
        {
            throw new NotImplementedException();
        }

        public UnsubscribeAction Configure(Func<IPipelineConfigurator, UnsubscribeAction> configure)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(T message) where T : class
        {
            Console.WriteLine(message.ToString());
        }

        TService IServiceBus.GetService<TService>()
        {
            throw new NotImplementedException();
        }

        public IEndpoint GetEndpoint(Uri address)
        {
            throw new NotImplementedException();
        }

        public TService GetService<TService>()
        {
            throw new NotImplementedException();
        }

        public IEndpoint Endpoint { get; private set; }
        public IEndpoint PoisonEndpoint { get; private set; }
        public IMessagePipeline OutboundPipeline { get; private set; }
        public IMessagePipeline InboundPipeline { get; private set; }

        public IServiceBus ControlBus
        {
            get { throw new NotImplementedException(); }
        }

        public IEndpointCache EndpointCache
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        private static bool unsubscribe()
        {
            return true;
        }
    }
}