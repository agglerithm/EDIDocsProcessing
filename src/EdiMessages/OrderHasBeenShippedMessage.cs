using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Enumerations;
using AFPST.Common.Structures;
using MassTransit;

namespace EdiMessages
{

    [Serializable]
    public class OrderHasBeenShippedMessage : OrderShippingInfo
    { 
        public override string BusinessPurpose
        {
            get { return "ASN"; }
        } 
    }
    
    [Serializable]
    public class OrderIsBackorderedMessage : OrderShippingInfo
    { 
        public override string BusinessPurpose
        {
            get { return "Backorder"; }
        } 
    }


    [Serializable]
    public class OrderShippingInfo : IEdiMessageWithAddress, CorrelatedBy<Guid>
    {

        public OrderShippingInfo()
        {
            Lines = new List<ShippedLine>();

        }

        public DateTime MaxDateShipped()
        {
            return Lines == null ? DateTime.MinValue : Lines.Max(x => x.DateShipped);
        }

        public string BOL
        {
            get; set;
        }

        public string OrderNumber
        {
            get;
            set;
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

        private Address _shipFromAddress;
        public Address ShipFromAddress
        {
            get { return _shipFromAddress; }
            set
            {
                if (value.IsShipFromAddress() == false)
                    throw new Exception("Expecting a Ship From Address");
                _shipFromAddress = value;
            }
        }

        public IEnumerable<ShippedLine> Lines
        {
            get; set;
        }

        public IEnumerable<string> GetLineNumbers()
        {
            return Lines.Select(x => x.LineNumber);
        }

        public void Add(ShippedLine line)
        {
             Lines.Add(line);   
        }

        public string CustomerPO
        {
            get; set;
        }

        public int DocumentId
        {
            get { return 856; }
        }

        public string ControlNumber
        {
            get; set;
        }

        public string BusinessPartnerCode
        {
            get; set;
        }

        public virtual string BusinessPurpose
        {
            get { return "ASN"; }
        }

        public int BusinessPartnerNumber
        {
            get;
            set;
        }

        public void AddAddress(Address address)
        {
            if (string.IsNullOrEmpty(address.AddressType))
                throw new ArgumentException("Address type is not specified");

            if (address.IsShipToAddress())
            {
                ShipToAddress = address;
                return;
            }
            if (address.IsShipFromAddress())
            {
                ShipFromAddress = address;
                return;
            }

            throw new ArgumentException(string.Format("Address type {0} is not useful here", address.AddressType));
        }


        public string GeographicLocation
        {
            get; set;
        }

        public Guid CorrelationId
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format("{7}[{8}]{7} DateShipped: {0}{7} BOL: {1}{7} ShipToAddress: {2}{7} ShipFromAddress: {2}{9} Lines: {3}{7} CustomerPO: {4}{7} ControlNumber: {5}{7} BusinessPartnerCode: {6}{7}, , GeographicLocation: {10}, CorrelationId: {11}, OrderNumber: {12}",
                MaxDateShipped(), BOL, ShipToAddress, Lines.PrintAll(), CustomerPO, ControlNumber, BusinessPartnerCode, Environment.NewLine, GetType().Name, ShipFromAddress, GeographicLocation, CorrelationId, OrderNumber);
        }


    }
}