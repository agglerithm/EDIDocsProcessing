using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Core.DocsOut;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.build
{
    [TestFixture]
    public class complete_810_records
    {
        private IControlNumberRepository _repo;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            EDIDocsOut.ApplicationEnvironment.Setup();
            _repo = Container.Resolve<IControlNumberRepository>();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_repopulate_fields()
        { 

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
