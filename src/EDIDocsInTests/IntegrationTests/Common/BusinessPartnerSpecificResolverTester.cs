using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Common
{
    [TestFixture]
    public class BusinessPartnerSpecificResolverTester
    {
        private IBusinessPartnerSpecificServiceResolver _resolver;
        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _resolver = ServiceLocator.Current.GetInstance<IBusinessPartnerSpecificServiceResolver>();
        }

        [Test]
        public void can_get_different_build_values()
        {
            
            _resolver.GetSegmentFactoryFor(BusinessPartner.Initech);
           var segFactory = _resolver.GetSegmentFactoryFor(BusinessPartner.VandalayIndustries);
            var flexBuildValues = segFactory.BuildValues;
            segFactory = _resolver.GetSegmentFactoryFor(BusinessPartner.Initech);
            var InitechBuildValues = segFactory.BuildValues;

            InitechBuildValues.SegmentDelimiter.ShouldNotEqual(flexBuildValues.SegmentDelimiter);
        }
    }
}
