namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    using System;
    using AFPST.Common.Extensions;
    using AFPST.Common.Services;
    using EDIDocsIn.config;
    using EDIDocsProcessing.Common;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;

    [TestFixture]
    public class Initech810FileRepositoryTester
    {
        private IInitech810FileRepository _invoiceFileRepo;

        [TestFixtureSetUp]
        public void setUp()
        {
            StructureMapBootstrapper.Execute();
            _invoiceFileRepo = new Initech810FileRepository(ServiceLocator.Current.GetInstance<IFileUtilities>(), 
                ServiceLocator.Current.GetInstance<ISegmentSplitter>());
        }

        [Test]
        public void can_parse_file()
        {
            var invoices = _invoiceFileRepo.GetInvoicesFrom(@"\\automation\d$\Upload\Initech\archive\");
            invoices.ForEach(i => Console.WriteLine("{0};{1};{2};{3};{4}", i.InvoiceNumber, i.InvoiceDate, i.SentDate, i.Sales, i.Tax));
        }
    }
}