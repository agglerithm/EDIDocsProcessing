using System;
using System.Collections.Generic;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.Extensions;

using EDIDocsProcessing.Core.DataAccess;
using EDIDocsProcessing.Core.DocsIn;
using InitechEDIDocsIn;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class DataAccessTestsForIncomingDocuments
    {
        private IIncomingDocumentsRepository _repo;
        private DocumentInDTO _firstDoc;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            StructureMapBootstrapper.Execute();
            _repo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();

            IEnumerable<DocumentInDTO> isaList = get_doc_list();
            isaList.ForEach(_repo.Save);
            _firstDoc = _repo.GetByISAControlNumberAndPartnerID(1001, BusinessPartner.Initech.Number);
        }

        private static IEnumerable<DocumentInDTO> get_doc_list()
        {
            var list = new List<DocumentInDTO>();
            for (int i = 1000; i < 1100; i++)
                list.Add(get_doc(i, i + 5));
            return list;
        }

        private static DocumentInDTO get_doc(int isaControl, int docControl)
        {
            return new DocumentInDTO
                       {
                           ControlNumber = docControl,
                           DocumentID = 850,
                           ERPID = "legacy",
                           DateSent = DateTime.Today,
                           ISAControlNumber = isaControl,
                           PartnerNumber = BusinessPartner.Initech.Number
                       };
        }

        [Test]
        public void can_add_response_element()
        {
            _firstDoc.ResponseElements.Clear();
            _firstDoc.ResponseElements.Add(new ResponseElementEntity
                                               {ElementName = "REF02", Value = "CAC33", Qualifier = "ZZ"});
            _repo.Save(_firstDoc);
        }

        [Test]
        public void can_recover_response_element()
        {
            var sessionSource = new EdiSessionSource("SQLConnectionTest", false);

            var repo = new IncomingDocumentsRepository(sessionSource);
            DocumentInDTO orderDoc = repo.GetByDocumentControlNumberAndPartnerID(1006, BusinessPartner.Initech.Number);
            if (orderDoc == null)
                throw new Exception(string.Format("Control number {0} not found for business partner {1}.",
                                                  1006, BusinessPartner.Initech.Number));
            orderDoc.ResponseElements.Count.ShouldEqual(1);
            orderDoc.ResponseElements[0].ElementName.ShouldEqual("REF02");
            orderDoc.ResponseElements[0].Value.ShouldEqual("CAC33");
        }
    }
}