using System;
using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using AFPST.Common.IO;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class IOTester
    {
        private IFileUtilities _fu;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Container.Reset(); 
            Container.Register<IFileUtilities, FileUtilities>();
            _fu = Container.Resolve<IFileUtilities>();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Container.Reset();
        }

        [Test]
        public void can_get_list_of_files()
        {
            IList<FileEntity> lst = _fu.GetListFromFolder(@"..\..\TestFiles\", "*.*", DateTime.Today.AddYears(-20));
            Assert.That(lst.Count, Is.GreaterThan(0));
        }

        [Test]
        public void file_entity_can_handle_full_path()
        {
            var fe = new FileEntity();
            Assert.That(fe.FileName == "");
            Assert.That(fe.ContainingFolder == "");
            fe.FullPath = @"c:\pagefile.sys";

            Assert.That(fe.FileName == "pagefile.sys");
            Assert.That(fe.ContainingFolder == @"c:\");
        }
    }
}