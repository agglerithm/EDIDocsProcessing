using System;
using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.impl;
using EmailService.Messages;
using MassTransit;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core
{
    using Common;

    [TestFixture]
    public class NotificationSenderTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private Mock<IEndpointCache> _endpointResolver;
        private Queue _emailServiceEndpointUri;
        private INotificationSender _sut;


        private INotificationSender CreateSUT()
        {
            _endpointResolver = new Mock<IEndpointCache>();
            _emailServiceEndpointUri = new Queue("null","test/uri");
            return new NotificationSender(_emailServiceEndpointUri, _endpointResolver.Object);
        }

        [Test]
        public void can_send_notification()
        {
            //Arrange
            IEnumerable<EmailAddress> recipients = new List<EmailAddress>();
            var emailServiceEndpoint = new Mock<IEndpoint>();
            _endpointResolver.Setup(x => x.GetEndpoint(new Uri(_emailServiceEndpointUri.ToString()))).Returns(emailServiceEndpoint.Object);

            //Act
            string body = "test body";
            string subject = "test";
            _sut.SendNotification(subject, recipients, body);

            //Assert
            emailServiceEndpoint.Verify(ep => ep.Send(
                                                  It.Is<SendNotificationMessage>(m => m.Body == body &&
                                                                                      m.Subject == subject &&
                                                                                      m.From.Address ==
                                                                                      EmailAddressConstants.
                                                                                          InformationtechnologygroupEmailAddress &&
                                                                                      m.Recipients == recipients)));
            _endpointResolver.VerifyAll();
        }
    }
}