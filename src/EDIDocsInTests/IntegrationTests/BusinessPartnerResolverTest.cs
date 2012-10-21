using AFPST.Common.Infrastructure;
using EDIDocsOut;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;

using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using EdiMessages;
using InitechEDIDocsOut;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class BusinessPartnerResolverTest
    {
        private BusinessPartnerResolver<OrderHasBeenShippedMessage> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _sut = new BusinessPartnerResolver<OrderHasBeenShippedMessage>();
        }

        [Test]
        public void can_resolve_correct_business_partner_document_creator()
        {
            var msg = new OrderHasBeenShippedMessage
                          {
                              BusinessPartnerCode = BusinessPartner.Initech.Code,
                              BusinessPartnerNumber = BusinessPartner.Initech.Number
                          };
            ICreateEdiContentFrom<OrderHasBeenShippedMessage> creator = _sut.ResolveFrom(msg);
            Assert.That(creator, Is.TypeOf(typeof (Initech856Creator)),
                        "Type is actually " + creator.GetType());
        }
    }
}