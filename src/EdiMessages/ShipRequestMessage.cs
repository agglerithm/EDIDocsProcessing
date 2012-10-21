using System;
using System.Collections.Generic;
using AFPST.Common.Enumerations;
using AFPST.Common.Structures;

namespace EdiMessages
{
    public enum ServiceLevel
    {
        Overnight = 1, Saturday = 4, TwoDay = 3, Ground = 5, StandardOvernight = 2
    }

    public enum PaymentMethod
    {
        Sender, Receiver, ThirdParty
    }

    [Serializable]
    public class ShipRequestMessage : IEdiMessageWithAddress 
    {
        public ShipRequestMessage()
        {
            Packages = new List<Package>();
        }
//        public void Load(CreateOrderMessage ord)
//        {
//            ControlNumber = ord.ControlNumber;
//            BusinessPartnerCode = ord.BusinessPartnerCode;
//
//            ord.LineItems.ForEach(loadPackage);
//        }

//        private void loadPackage(CustomerOrderLine line)
//        {
//            var pack = new Package
//                               {
//                                   Specifications =
//                                       new PackageSpecification {Length = 1, Width = 1, Height = 1, Weight = 1},
//                                       Description = line.ItemDescription, IndexNumber = Packages.Count()
//                               };
//            Packages.Add(pack);
//        }
        public int DocumentId
        {
            get; private set;
        }

        public string ControlNumber
        {
            get;  set;
        }

        public string CustomerOrderNumber
        {
            get; set;
        }

        public int LineNumber
        {
            get; set;
        }

        public string CustomerPONumber
        {
            get; set;
        }
        public string BusinessPartnerCode
        {
            get; set;
        }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber
        {
            get;
            set;
        }

        public PaymentMethod MethodOfPayment
        {
            get; set;
        }

        public ServiceLevel LevelOfService
        {
            get; set;
        }

        private Address _shipToAddress;

        public Address ShipToAddress
        {
            get { return _shipToAddress; }
            set
            {
                if (value.IsShipToAddress() == false)
                    throw new Exception("Expecting a Ship To Address");
                _shipToAddress = value;
            }
        }

        public Address ShipFromAddress
        {
            get; set;
        }

        public void AddAddress(Address address)
        {
            if (string.IsNullOrEmpty(address.AddressType))
                throw new ArgumentException("Address type is not specified");
            
            if (address.AddressType == AddressTypeConstants.ShipTo)
            {
                ShipToAddress = address;
                return;
            }
             if (address.AddressType == AddressTypeConstants.ShipFrom)
             {
                 ShipFromAddress = address;
                 return;
             }
            
            throw new ArgumentException(string.Format("Address type {0} is not useful here", address.AddressType));
        }

        public void AddPackage(Package pkg)
        {
            Packages.Add(pkg);
        }

        public IList<Package> Packages
        {
            get; set;
        }

        public int PaymentType
        {
            get; set;
        }

        public string AccountNumberToUse
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format("{10}[{11}]{10} Packages: {0}{10} DocumentID: {1}{10} ControlNumber: {2}{10} BusinessPartnerCode: {3}{10} MethodOfPayment: {4}{10} LevelOfService: {5}{10} ShipToAddress: {6}{10} ShipFromAddress: {7}{10} PaymentType: {8}{10} AccountNumberToUse: {9}{10}", 
                Packages.PrintAll(), DocumentId, ControlNumber, BusinessPartnerCode, MethodOfPayment, LevelOfService, ShipToAddress, ShipFromAddress, PaymentType, AccountNumberToUse, Environment.NewLine, GetType().Name);
        }
    }


}
