using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Logging;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Infrastructure;
using EdiMessages;
using FedexEDIDocsIn;

namespace MockShipMessagePublisher
{
    public class ShippingMessageService
    {
        private ILogger _logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public void Start()
        { 

            var orderShippedPublisher = Container.Resolve<IShippedOrderMessagePublisher>();

            IEnumerable<ShippedOrderMessage> shippedOrderMessages = get_list();

            orderShippedPublisher.PublishShippedOrderMessages(shippedOrderMessages);

            Logger.Info(shippedOrderMessages.Count() + " messages published."); 
        }

        private static IEnumerable<ShippedOrderMessage> get_list()
        {
            var lst = new List<ShippedOrderMessage>();
            lst.Add(get_shipped_order_msg());
            return lst;                                                         
        }

        private static ShippedOrderMessage get_shipped_order_msg()
        {
            var msg = new ShippedOrderMessage
            {
                BOL = "0005456",
                CustomerPO = "SRC009670",
                DateShipped = DateTime.Today,
                BusinessPartnerCode = BusinessPartner.FedEx.Code,
                BusinessPartnerNumber = BusinessPartner.FedEx.Number,
                ControlNumber = "60001"
            };
            msg.Add(new ShippedLine
            {
                CustomerPartNo = "900104",
                ItemID = "FIN23430I044",
                LineNumber = "1",
                QtyOrdered = 3,
                QtyShipped = 3,
                Status = "AC"
            });
            msg.AddAddress(new Address { AddressType = AddressTypeConstants.ShipFrom, Address1 = "2343 Grimes", City = "Austin", State = "TX", Zip = "78987", AddressName = "Austin Foam Plastics" });
            msg.AddAddress(new Address { AddressType = AddressTypeConstants.ShipTo, Address1 = "90 FEDEX PARKWAY", City = "COLLIERVILLE", State = "TN", Zip = "38017", AddressName = "FEDEX SUSIE CARNEY" });
             return msg;                                               
        }

        public void Stop()
        {
        }
    }
}