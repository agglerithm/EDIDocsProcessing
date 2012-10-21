using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Messages;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.impl;
using MassTransit;
using StructureMap.Configuration.DSL;

namespace EDIDocsIn.config
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            var config = new AFPSTConfiguration();

            
             

            For<IConsumer>().Use<OrderRequestReceivedMessageSubscriber>();

 
        }
    }
}