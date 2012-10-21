using System.Collections.Generic;
using System.Linq;
using EDIDocsOut.config;
using EDIDocsProcessing.Core.DocsOut;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class AddressSegmentCreatorResolverTester
    {
        private IEnumerable<IAddressSegmentCreator> _segCreators;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            _segCreators = ServiceLocator.Current.GetAllInstances<IAddressSegmentCreator>();
        }

        [Test]
        public void can_resolve_segment_creators()
        {
            _segCreators.Count().ShouldEqual(3);
        }
    }
}