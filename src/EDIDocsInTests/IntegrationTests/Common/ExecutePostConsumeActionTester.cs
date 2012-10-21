using System;
using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Core;
using EdiMessages;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Transports;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.Common
{
    [TestFixture]
    public class ExecutePostConsumeActionTester
    {
        private IPostActionSpecification _postActionSpecification;
        private IServiceBus _serviceBus;
        private const string BOL = "Bol";
        private const string ControlNumber = "ControlNumber";
        private IList<ShippedLine> Lines = new List<ShippedLine>();
        private OrderHasBeenShippedMessage DefaultOrderHasBeenShippedMessage;
        private IExecutePostConsumeAction _sut;

        [SetUp]
        public void Setup()
        {
            ObjectFactory.Configure(x =>
                                        {
                                            x.For<IServiceBus>().Singleton().Use<MockedServiceBus>();
                                            x.For<IPostActionSpecification>().Use<PublishEdiAsnSentSpecification>();
                                            x.For<IExecutePostConsumeAction>().Use<ExecutePostConsumeAction>();
                                        });

            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(ObjectFactory.Container));

            Lines = new List<ShippedLine>();
            Lines.Add(new ShippedLine() { LineNumber = "line1" });
            Lines.Add(new ShippedLine() { LineNumber = "line2" });
            Lines.Add(new ShippedLine() { LineNumber = "line3" });

            DefaultOrderHasBeenShippedMessage = new OrderHasBeenShippedMessage()
            {
                BOL = BOL,
                ControlNumber = ControlNumber,
                Lines = Lines
            };

            _sut = ServiceLocator.Current.GetInstance<IExecutePostConsumeAction>();
        }

        [Test, Explicit]
        public void should_execute_properly_for_OrderHasBeenShippedMessage()
        {
            var serviceBus = ServiceLocator.Current.GetInstance<IServiceBus>() as MockedServiceBus;
            Assert.That(serviceBus.PublishedMessage, Is.Null);

            _sut.Execute(DefaultOrderHasBeenShippedMessage);

            Assert.That(serviceBus.PublishedMessage, Is.Not.Null);
            Assert.That(serviceBus.PublishedMessage.BOL, Is.EqualTo(BOL));
            Assert.That(serviceBus.PublishedMessage.ControlNumber, Is.EqualTo(ControlNumber));
            Assert.That(serviceBus.PublishedMessage.LineNumbers.Count, Is.EqualTo(Lines.Count));
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Specifications are not Registered")]
        public void should_throw_exception_when_no_specifications_are_resolved()
        {
            ObjectFactory.Initialize(x =>
                                    {
                                        x.For<IExecutePostConsumeAction>().Use<ExecutePostConsumeAction>();
                                    });

            _sut = ServiceLocator.Current.GetInstance<IExecutePostConsumeAction>();
            _sut.Execute(DefaultOrderHasBeenShippedMessage);
        }

        #region MockedServiceBus
        private class MockedServiceBus : IServiceBus
        {
            public EdiAsnSentMessage PublishedMessage { get; set; }

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
                throw new NotImplementedException();
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
                return null;
            }

            public void Publish<T>(T message) where T : class
            {
                PublishedMessage = message as EdiAsnSentMessage;
            }

 

            public IEndpoint GetEndpoint(Uri address)
            {
                return new Endpoint(new EndpointAddress(address), null, null, null);
            }

            public TService GetService<TService>() where TService : IBusService
            {
                 throw new NotImplementedException();  
            }

            public IEndpoint Endpoint
            {
                get { return new Endpoint(null,null,null,null) ; }
            }

            public IEndpoint PoisonEndpoint
            {
                get { throw new NotImplementedException(); }
            }

            public IMessagePipeline OutboundPipeline
            {
                get { throw new NotImplementedException(); }
            }

            public IMessagePipeline InboundPipeline
            {
                get { throw new NotImplementedException(); }
            }

            public IServiceBus ControlBus
            {
                get { throw new NotImplementedException(); }
            }

            public IEndpointCache EndpointCache
            {
                get { throw new NotImplementedException(); }
            }

            public void Dispose()
            {
                
            }
        }

        #endregion
    }

    internal class FakeBusService :IBusService
    {
        public void Dispose()
        { 
        }

        public void Start(IServiceBus bus)
        { 
        }

        public void Stop()
        { 
        }
    }
}