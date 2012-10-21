using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using FluentNHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core.DocsOut
{
    [TestFixture]
    public class ControlNumberRepositoryTester
    {
        private IControlNumberRepository _sut;
        private Mock<ISession> _session;
        private Mock<ISessionSource> _sessionSource;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _session = new Mock<ISession>();
            _sessionSource = new Mock<ISessionSource>();
            _session.SetupSet(s => s.FlushMode);
            _sessionSource.Setup(s => s.CreateSession()).Returns(_session.Object);
            _sut = new ControlNumberRepository(_sessionSource.Object);
        }


        private static ITransaction GetTrans()
        {
            var trans = new Mock<ITransaction>();
            trans.Setup(t => t.Commit());
            trans.Setup(t => t.Rollback());
            return trans.Object;
        }

        [Test]
        public void can_get_next_document()
        {
            var isa = new ISAEntity();
            _session.Setup(s => s.SaveOrUpdate(It.IsAny<ISAEntity>()));
            ITransaction trans = GetTrans();
            _session.Setup(s => s.BeginTransaction()).Returns(trans);
            _sut.GetNextDocument(isa, 810);
        }

        [Test]
        public void can_get_next_document_with_bol()
        {
            var isa = new ISAEntity();
            _session.Setup(s => s.SaveOrUpdate(It.IsAny<ISAEntity>()));
            ITransaction trans = GetTrans();
            _session.Setup(s => s.BeginTransaction()).Returns(trans);

            DocumentEntity doc = _sut.GetNextDocument(isa, 810, "83253");
            Assert.That(doc.ERPID == "83253");
        }

        [Test]
        public void can_get_next_isa()
        {
            _session.Setup(s => s.SaveOrUpdate(It.IsAny<ISAEntity>()));
            ITransaction trans = GetTrans();
            _session.Setup(s => s.BeginTransaction()).Returns(trans);
            ISAEntity isa = _sut.GetNextISA("NO", BusinessPartner.FedEx.Number);
            Assert.That(isa.GroupID == "NO");
        }
    }
}