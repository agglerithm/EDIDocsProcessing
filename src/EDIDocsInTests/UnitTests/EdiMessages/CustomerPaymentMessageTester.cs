using System;
using AFPST.Common;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class CustomerPaymentMessageTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private CustomerPaymentRecievedMessage _sut;

        private CustomerPaymentRecievedMessage CreateSUT()
        {
            return new CustomerPaymentRecievedMessage();
        }


        [Test]
        public void can_add_same_invoice_payment_only_once()
        {
            var invoicePayment1 = new InvoicePayment();
            _sut.Add(invoicePayment1);
            _sut.Add(invoicePayment1);

            _sut.InvoicePayments.Length.ShouldEqual(1);
        }

        [Test]
        public void can_get_invoice_payments()
        {
            _sut.InvoicePayments.Length.ShouldEqual(0);

            var invoicePayment1 = new InvoicePayment();
            var invoicePayment2 = new InvoicePayment();
            _sut.Add(invoicePayment1);
            _sut.Add(invoicePayment2);

            _sut.InvoicePayments[0].ShouldEqual(invoicePayment1);
            _sut.InvoicePayments[1].ShouldEqual(invoicePayment2);
        }

        [Test]
        public void can_get_set_business_partner_code()
        {
            _sut.BusinessPartnerCode = "99";
            _sut.BusinessPartnerCode.ShouldEqual("99");
        }

        [Test]
        public void can_get_set_BusinessPartnerNumber()
        {
            _sut.BusinessPartnerNumber = 99;
            _sut.BusinessPartnerNumber.ShouldEqual(99);
        }

        [Test]
        public void can_get_set_CheckDate()
        {
            SystemTime.Now = () => DateTime.Parse("8/10/2009");
            _sut.CheckDate = SystemTime.Now();
            _sut.CheckDate.ShouldEqual(SystemTime.Now());
        }

        [Test]
        public void can_get_set_CheckNumber()
        {
            _sut.CheckNumber = "99";
            _sut.CheckNumber.ShouldEqual("99");
        }

        [Test]
        public void can_get_set_ControlNumber()
        {
            _sut.ControlNumber = "99";
            _sut.ControlNumber.ShouldEqual("99");
        }

        [Test]
        public void can_get_total_of_all_invoice_payents()
        {
            _sut.Add(new InvoicePayment {Amount = 12});
            _sut.Add(new InvoicePayment {Amount = (decimal) 15.33});
            _sut.Add(new InvoicePayment { Amount = (decimal)1700.34 });
            _sut.GetTotalAmount().ShouldEqual((decimal)(12 + 15.33 + 1700.34));
        }

        [Test]
        public void CustomerName()
        {
            _sut.CustomerName = "99";
            _sut.CustomerName.ShouldEqual("99");
        }

        [Test]
        public void document_id_is_always_0()
        {
            _sut.DocumentId.ShouldEqual(0);
        }
    }
}