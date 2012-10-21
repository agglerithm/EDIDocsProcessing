using System.Collections.Generic;
using EDIDocsOut.config;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Foxconn
{
    [TestFixture]
    public class Foxconn846Tester
    {
        private ICreateEdiContentFrom<FinishedAndRawInventoryCountedList> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();


            _sut = ServiceLocator.Current.GetInstance<ICreateEdiContentFrom<FinishedAndRawInventoryCountedList>>();
 
        }

        [Test]
        public void can_create_846()
        {
            _sut.BuildFromMessage(get_msg());
        }

        private FinishedAndRawInventoryCountedList get_msg()
        {
            return new FinishedAndRawInventoryCountedList()
                       {
                           BusinessPartnerCode = "FOX",
                           BusinessPartnerNumber = 1000,
                           InventoryList = get_list()
                       };
        }

        private IList<FinishedAndRawInventoryCounted> get_list()
        {
            return new List<FinishedAndRawInventoryCounted>()
                       {
                           new FinishedAndRawInventoryCounted()
                               {
                                   AvailableFinishedQuantity = 21,
                                   AvailableRawQuantity = 431,
                                   CustomerPartNumber = "P3X234"
                               },
                           new FinishedAndRawInventoryCounted()
                               {
                                   AvailableFinishedQuantity = 331,
                                   AvailableRawQuantity = 23,
                                   CustomerPartNumber = "P3X284"
                               },
                       };
        }
    }
}
