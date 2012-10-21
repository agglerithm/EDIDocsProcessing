using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Publishers;
using MassTransitContrib;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace EDIDocsProcessing.Tests.UnitTests.Initech
{
    internal class TestRegistry : Registry
    {
        internal TestRegistry()
        {
            For<IFileParser>().Use<TestFileParser>();
            For<IEdiMessagePublisher>().Use<TestPublisher>();
        }

        internal static    void SetUpTestRegistry()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(ObjectFactory.Container));
            ObjectFactory.Configure(x => x.AddRegistry(new TestRegistry()));
        }
    }
}