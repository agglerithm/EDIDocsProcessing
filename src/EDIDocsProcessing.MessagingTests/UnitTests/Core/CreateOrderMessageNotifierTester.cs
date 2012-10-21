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
    [TestFixture]
    public class CreateOrderMessageNotifierTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private Mock<INotificationSender> _notificationSender;
        private INotifier<OrderRequestReceivedMessage> _sut;

        private INotifier<OrderRequestReceivedMessage> CreateSUT()
        {
            _notificationSender = new Mock<INotificationSender>();

            return new CreateOrderMessageNotifier(_notificationSender.Object);
        }

        [Test]
        public void can_notify_failure_of_CreateOrderMessage()
        {
            var message = new OrderRequestReceivedMessage {BusinessPartnerCode = "WWT"};
            var exception = new Exception("Test Exception");
            var fault = new Fault<OrderRequestReceivedMessage>(message, exception);

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