using System.IO;
using System.Threading;
using AFPST.Common;
using Castle.Windsor;
using EDIDocsProcessing.Common;
using EdiMessages;
using log4net.Config;
using MassTransit;
using MassTransit.WindsorIntegration;
using Microsoft.Practices.ServiceLocation;

namespace TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("TestPublisher.log4net.xml"));

            IWindsorContainer container = new DefaultMassTransitContainer("TestPublisher.Castle.xml");
            IServiceBus bus = ServiceLocator.Current.GetInstance<IServiceBus>();


            var messaage = new CreateOrderMessage()
            {
                BusinessPartnerCode = BusinessPartner.FedEx.Code,
                BusinessPartnerNumber = BusinessPartner.FedEx.Number,
                ControlNumber = "1",
                CustomerPO = "PO-100",
                Customer = new Customer { CustomerID = "100", CustomerName = "test co." },
                BusinessProcessName = "business process name",
                RequestDate = SystemTime.Now().ToString()
            };

            messaage.Customer.AddAddress(new Address
            {
                Address1 = "test addr1",
                Address2 = "addr2",
                AddressCode = "TEST",
                AddressName = "office",
                AddressType = AddressTypeConstants.ShipTo,
                City = "austin",
                State = "TX",
                Zip = "88888"
            });

            messaage.Add(new CustomerOrderLine
            {
                CustID = "100",
                CustomerPartNumber = "222",
                CustomerPO = "333",
                ItemDescription = "desc",
                ItemID = "444",
                LineNumber = 1,
                Notes = "note",
                OrderMultiple = 1,
                OrderNumber = "555",
                RequestedPrice = 6.0,
                RequestedQuantity = 2,
                RequestNumber = "1",
                TestMode = true
            });
            messaage.LineItems.Add(new CustomerOrderLine
            {
                CustID = "100",
                CustomerPartNumber = "222b",
                CustomerPO = "333b",
                ItemDescription = "descb",
                ItemID = "444b",
                LineNumber = 2,
                Notes = "noteb",
                OrderMultiple = 2,
                OrderNumber = "555b",
                RequestedPrice = 7.0,
                RequestedQuantity = 3,
                RequestNumber = "1b",
                TestMode = true
            });

            Thread.Sleep(4000);

            bus.Publish(messaage);

            bus.Dispose();
        }
    }
}
