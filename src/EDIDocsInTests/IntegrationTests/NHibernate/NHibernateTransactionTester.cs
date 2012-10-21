using System;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsIn;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.NHibernate
{
    [TestFixture]
    public class NHibernateTransactionTester
    {
        private IIncomingDocumentsRepository _incomingDocumentsRepository;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var sessionSource = new EdiSessionSource("SQLConnection", false);
            _incomingDocumentsRepository = new IncomingDocumentsRepository(sessionSource);
        }

        [Test, Explicit]
        public void can_save_record()
        {
            var doc = new DocumentInDTO
                          {
                              ISAControlNumber = 332433,
                              ControlNumber = 253,
                              DateSent = DateTime.Today,
                              GroupID = "PO",
                              ID = new Guid(),
                              DocumentID = 850,
                              ERPID = "legacy",
                              PartnerNumber = BusinessPartner.Initech.Number
                          };

            _incomingDocumentsRepository.Save(doc);
        }
    }
}