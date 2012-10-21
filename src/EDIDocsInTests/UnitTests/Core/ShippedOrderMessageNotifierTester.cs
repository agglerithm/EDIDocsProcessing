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
    public class ShippedOrderMessageNotifierTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private Mock<INotificationSender> _notificationSender;
        private INotifier<OrderHasBeenShippedMessage> _sut;

        private INotifier<OrderHasBeenShippedMessage> CreateSUT()
        {
            _notificationSender = new Mock<INotificationSender>();

            return new ShippedOrderMessageNotifier(_notificationSender.Object);
        }

        [Test]
        public void can_notify_failure_of_CreateOrderMessage()
        {
            var message = new OrderHasBeenShippedMessage {BusinessPartnerCode = "WWT"};
            message.Add(new ShippedLine());

            var exception = new Exception("Test Exception");
            var fault = new Fault<OrderHasBeenShippedMessage>(message, exception);

            _sut.NotifyFailureOf(fault);

            _notificationSender.Verify(x => x.SendNotification(
                                                It.IsAny<string>(),
                                                It.Is<IList<EmailAddress>>(
                                                    a =>
                                                    a[0].Address ==
                                                    EmailAddressConstants.InformationtechnologygroupEmailAddress &&
                                                    a[1].Address == EmailAddressConstants.LogisticsEmailAddress),
                                                It.Is<string>(body => body.Contains(message.ToString()) &&
                                                                      body.Contains(exception.ToString()))));
        }
    }
}