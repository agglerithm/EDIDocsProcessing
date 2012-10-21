using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl; 
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class AddressParser : IAddressParser
    {
        #region IAddressParser Members

        public void ProcessAddresses(List<Segment> lst, IEdiMessage ediMessage)
        {
            lst.RemoveWhile(seg => seg.Label != EDIConstants.AddressNameLabel);
            var addr_loop = new List<Segment>();
            while (EDIUtilities.MoveSegmentByLabel(lst, addr_loop, EDIConstants.AddressNameLabel))
            {
                SegmentCount++;
                Segment next_seg = lst[0];
                while (next_seg.Label == EDIConstants.AddressLineLabel
                       || next_seg.Label == EDIConstants.GeographicLabel)
                {
                    SegmentCount++;
                    EDIUtilities.MoveSegment(lst, addr_loop, next_seg);
                    next_seg = lst[0];
                }
                ediMessage.AddAddress(process_address(addr_loop));
                addr_loop.Clear();
            }
        }

        public int SegmentCount { get; private set; }

        #endregion

        private static Address process_address(List<Segment> addressSegments)
        {
            var addrStruct = new Address();
            string elementDelimeter = addressSegments[0].Contents.Substring(2, 1);
            addressSegments.ForEach(line => process_address_line(line, addrStruct, elementDelimeter));
            return addrStruct;
        }

        private static void process_address_line(Segment line, Address address, string elementDelimeter)
        {
            List<string> arr = line.GetElements(elementDelimeter);
            if (line.Label == EDIConstants.AddressNameLabel)
            {
                address.AddressType = get_address_type(arr[1]);
                address.AddressName = arr[2];
                if (arr.Count >= 4)
                    address.AddressCode = arr[3];
            }
            if (line.Label == EDIConstants.AddressLineLabel)
                if (string.IsNullOrEmpty(address.Address1))
                    address.Address1 = arr[1];
                else
                {
                    if (string.IsNullOrEmpty(address.Address2))
                        address.Address2 = arr[1];
                }
            if (line.Label == EDIConstants.GeographicLabel)
            {
                address.City = arr[1];
                address.State = arr[2];
                address.Zip = arr[3];
            }
        }


        private static string get_address_type(string code)
        {
            if (code == "ST") return AddressTypeConstants.ShipTo;
            return AddressTypeConstants.BillTo;
        }
    }
}