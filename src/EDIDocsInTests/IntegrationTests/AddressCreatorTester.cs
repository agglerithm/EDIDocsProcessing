using System.Collections.Generic;
using AFPST.Common.Structures;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;

using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class AddressCreatorTester
    {
        private readonly IList<IAddressSegmentCreator> _addrSegCreator = new List<IAddressSegmentCreator>();
        private ISegmentFactory _segFactory;

        [TestFixtureSetUp]
        public void SetUp()
        { 
            StructureMapBootstrapper.Execute();

            _segFactory = ServiceLocator.Current.GetInstance<ISegmentFactory>();
            _segFactory.SetBuildValues(BusinessPartner.Initech);
        }

        private static void handle_addr(IAddressSegmentCreator segCreator, AddressLoop container,
                                        ISegmentFactory factory, Address address)
        {
            if (segCreator.CanProcess(address.AddressType))
            {
                segCreator.AddAddressSegmentsTo(container, factory, address);
            }
        }


        private Address get_shipfrom_address()
        {
            return new Address
                       {
                           Address1 = "234 Elm",
                           Address2 = "Suite 11",
                           AddressName = "Acme",
                           AddressType = AddressTypeConstants.ShipFrom,
                           City = "Austin",
                           State = "TX",
                           Zip = "23432"
                       };
        }

        private Address get_shipto_address()
        {
            return new Address
                       {
                           Address1 = "234 Elm",
                           Address2 = "Suite 11",
                           AddressName = "Acme",
                           AddressType = AddressTypeConstants.ShipTo,
                           City = "Austin",
                           State = "TX",
                           Zip = "23432"
                       };
        }

        private Address get_billto_address()
        {
            return new Address
                       {
                           Address1 = "234 Elm",
                           Address2 = "Suite 11",
                           AddressName = "Acme",
                           AddressType = AddressTypeConstants.BillTo,
                           City = "Austin",
                           State = "TX",
                           Zip = "23432"
                       };
        }


        [Test]
        public void can_create_address_segment()
        {
            var container = new AddressLoop(_segFactory);
            Address addr = get_shipfrom_address();
            _addrSegCreator.ForEach(a => handle_addr(a, container, _segFactory, addr));
            container.Count(EdiStructureNameConstants.Segment).ShouldEqual(6);

            addr = get_shipto_address();
            _addrSegCreator.ForEach(a => handle_addr(a, container, _segFactory, addr));
            container.Count(EdiStructureNameConstants.Segment).ShouldEqual(9);

            addr = get_billto_address();
            _addrSegCreator.ForEach(a => handle_addr(a, container, _segFactory, addr));
            container.Count(EdiStructureNameConstants.Segment).ShouldEqual(12);
        }
    }
}