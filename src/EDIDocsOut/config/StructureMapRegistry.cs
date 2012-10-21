using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Messages;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using MassTransit;
using MassTransit.Transports.Msmq;
using StructureMap.Configuration.DSL;

namespace EDIDocsOut.config
{
    using EDIDocsProcessing.Common;

    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            var config = new AFPSTConfiguration();
            //            _unsubscribeActions.Add(_bus.Subscribe<Subscriber<OrderHasBeenShippedMessage>>());
            //            _unsubscribeActions.Add(_bus.Subscribe<FaultSubscriber<OrderHasBeenShippedMessage>>());
            //            _unsubscribeActions.Add(_bus.Subscribe<Subscriber<OrderIsBackorderedMessage>>());
            //            _unsubscribeActions.Add(_bus.Subscribe<Subscriber<OrderRequestReceivedMessage>>());
            //            _unsubscribeActions.Add(_bus.Subscribe<Subscriber<InvoicedOrderMessage>>());
            //            _unsubscribeActions.Add(_bus.Subscribe<FaultSubscriber<InvoicedOrderMessage>>());
            //
            // Adding Ignore Subscriber to remove unwanted messages from queue
            //            _bus.Subscribe<IgnoreSubscriber<OrderHasBeenShippedMessage>>();
            //            _bus.Subscribe<IgnoreSubscriber<OrderRequestReceivedMessage>>();
            //            if(runOnce) Stop();
            

            For<INotificationSender>().Use<NotificationSender>()
                .Ctor<Queue>("notificationEndpoint").Is(config.GetQueue("EmailServiceHost", "EmailServiceEndpoint"))
                .Ctor<IEndpointCache>().Is(EndpointCacheFactory.New(ecf => ecf.AddTransportFactory<MsmqTransportFactory>()));


            For<IConsumer>().Use<WWTSubscriber<OrderRequestReceivedMessage>>();
            For<IConsumer>().Use<WWTSubscriber<OrderHasBeenShippedMessage>>();
            For<IConsumer>().Use<WWTSubscriber<InvoicedOrderMessage>>();
            For<IConsumer>().Use<Subscriber<OrderRequestReceivedMessage>>();
            For<IConsumer>().Use<Subscriber<OrderHasBeenShippedMessage>>(); 
            For<IConsumer>().Use<Subscriber<OrderChangeRequestReceivedMessage>>(); 
            For<IConsumer>().Use<Subscriber<OrderIsBackorderedMessage>>(); 
            For<IConsumer>().Use<Subscriber<InvoicedOrderMessage>>();
            For<IConsumer>().Use<FaultSubscriber<InvoicedOrderMessage>>();
            For<IConsumer>().Use<FaultSubscriber<OrderHasBeenShippedMessage>>(); 

            //For<IMessageSender<LabelPrintRequestMessage>>().Use<MessageSender<LabelPrintRequestMessage>>()
            // .Ctor<Queue>("queue").Is(config.GetQueue("reissueLabelQueueHost", "reissueLabelQueue"));
        }
    }
}