using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Common
{
    using AFPST.Common.IO;
    using EDIDocsProcessing.Common;
    using Microsoft.Practices.ServiceLocation;

    [TestFixture]
    public class AssignerTester
    {
        private IAssignDocumentsToPartners _assigner;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            EDIDocsIn.config.StructureMapBootstrapper.Execute();
            _assigner = ServiceLocator.Current.GetInstance<IAssignDocumentsToPartners>();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test, Explicit]
        public void can_assign_documents()
        {
            IList<FileEntity> files = get_files();
            var assignments = _assigner.MakeAssignments(files);
            assignments.Count().ShouldEqual(2);
        }

        private IList<FileEntity> get_files()
        {
            var lst = new List<FileEntity>();
            lst.Add(new FileEntity() { FullPath = @"\\automation\d$\Download\Test\edictIN_EDI20120223114443.dat" });
            lst.Add(new FileEntity() { FullPath = @"\\automation\d$\Download\Test\edictIN_EDI20120223012943.dat" });
            lst.Add(new FileEntity() { FullPath = @"\\automation\d$\Download\Test\edictIN_EDI20120012046.dat" });

            return lst;
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
