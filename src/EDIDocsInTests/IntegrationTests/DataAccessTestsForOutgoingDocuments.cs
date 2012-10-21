using AFPST.Common.Infrastructure;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core.DocsOut;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class DataAccessTestsForOutgoingDocuments
    {
        private IControlNumberRepository _repo;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _repo = ServiceLocator.Current.GetInstance<IControlNumberRepository>();
        }

        [Test]
        public void can_add_document()
        {
            ISAEntity isa = _repo.GetNextISA("IN", BusinessPartner.Initech.Number);
            DocumentEntity doc = _repo.GetNextDocument(isa, 856);
            Assert.That(doc.ControlNumber != 0);
            Assert.That(doc.DocumentID == 856);
            Assert.That(doc.ISAEntity.ControlNumber == isa.ControlNumber);
        }

        [Test]
        public void can_create_new_ISA()
        {
            ISAEntity isa = _repo.GetNextISA("IN", BusinessPartner.Initech.Number);
            isa.Add(new DocumentEntity {DocumentID = 856, ERPID = "0083234"});
            isa.Add(new DocumentEntity {DocumentID = 856, ERPID = "0083235"});
            isa.Add(new DocumentEntity {DocumentID = 856, ERPID = "0083236"});
            isa.Add(new DocumentEntity {DocumentID = 856, ERPID = "0083237"});

            _repo.Save(isa);

            Assert.That(isa.ControlNumber != 0);
            Assert.That(isa.GroupControlNumber != 0);
            Assert.That(isa.Documents[0].ControlNumber != 0);
        }
    }
}