using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;
using StructureMap.Configuration.DSL;

namespace InitechEDIDocsOut.config
{
    public class InitechEdiDocsOutRegistry : Registry
    {
        public InitechEdiDocsOutRegistry()
        {
            For<IBuildValueFactory>().Use<InitechBuildValueFactory>().Named("InitechBuildValueFactory");
            For<ICreateEdiContentFrom<OrderRequestReceivedMessage>>().Use<Initech855Creator>();
            For<ICreateEdiContentFrom<InvoicedOrderMessage>>().Use<Initech810Creator>();
            For<ICreateEdiContentFrom<OrderHasBeenShippedMessage>>().Use<Initech856Creator>();
            For<ICreateEdiContentFrom<OrderIsBackorderedMessage>>().Use<Initech856BackorderCreator>();

        }
    }
}