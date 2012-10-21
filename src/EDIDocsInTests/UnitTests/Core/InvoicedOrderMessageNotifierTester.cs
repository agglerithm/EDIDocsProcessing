using System;
using System.Collections.Generic;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.impl;
using EdiMessages;
using EmailService.Messages;
using MassTransit;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core
{
    using Common;

    [TestFixture]
    public class InvoicedOrderMessageNotifierTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private Mock<INotificationSender> _notificationSender;
        private INotifier<InvoicedOrderMessage> _sut;

        private INotifier<InvoicedOrderMessage> CreateSUT()
        {
            _notificationSender = new Mock<INotificationSender>();
            return new InvoicedOrderMessageNotifier(_notificationSender.Object);
        }

        [Test]
        public void can_notify_failure_of_InvoicedOrderMessage()
        {
            var message = new InvoicedOrderMessage {BusinessPartnerCode = "WWT"};
            var exception = new Exception("Test Exception");
            var fault = new Fault<InvoicedOrderMessage>(message, exception);

            _sut.NotifyFailureOf(fault);

            _notificationSender.Verify(x => x.SendNotification(
                                                It.IsAny<string>(),
                                                It.Is<IList<EmailAddress>>(
                                                    a =>
                                                    a[0].Address ==
                                                    EmailAddressConstants.InformationtechnologygroupEmailAddress &&
                                                    a[1].Address == EmailAddressConstants.AccountsReceivableEmailAddress),
                                                It.Is<string>(body => body.Contains(message.ToString()) &&
                                                                      body.Contains(exception.ToString()))));
        }
    }
}