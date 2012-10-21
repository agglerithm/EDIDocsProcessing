using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EdiMessages;
using MassTransit;
using StructureMap.Configuration.DSL;

namespace EDIDocsOut.config
{
    public class EdiDocsOutRegistry : Registry
    {
        public EdiDocsOutRegistry()
        {

            For<IPostActionSpecification>().Use<PublishEdiAsnSentSpecification>();
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();
                     });
            For<ISegmentFactory>().Use<SegmentFactory>().Named("segFact");
            For<ISubscriptionService>().Use<EdiDocsOutSubscriberStarter>();
            For<IControlNumberRepository>().Use<ControlNumberRepository>().Named("controlNumberRepository");
            ForConcreteType<EdiDocsOutSubscriberStarter>();

        }
    }
}