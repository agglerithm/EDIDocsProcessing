using System;
using System.Collections.Generic;
using System.Configuration;
using AFPST.Common.Infrastructure;
using EDIDocsOut.config;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.impl;
using EmailService.Messages;
using MassTransit;
using MassTransit.Transports.Msmq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.MessagingTests
{
    using Common;

    [TestFixture]
    public class NotificationSenderTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {

            MassTransitEdiDocsOutBootstrapper.Execute();
            StructureMapBootstrapper.Execute();
            _endpointResolver = EndpointCacheFactory.New(ecf => ecf.AddTransportFactory<MsmqTransportFactory>());
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        private IEndpointCache _endpointResolver;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            RuntimeServicesRunner.EnsureRuntimeServicesAreRunning();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            RuntimeServicesRunner.Kill();
        }

        [Test]
        public void when_publish_SendNotificationMessage_it_will_go_to_email_service_endpoint()
        {
            var emailServiceEndpointUri = new Queue(ConfigurationManager.AppSettings["EmailServiceHost"], ConfigurationManager.AppSettings["EmailServiceEndpoint"]);

            IEndpoint emailServiceEndpoint = _endpointResolver.GetEndpoint(emailServiceEndpointUri.ToString());

            int startingNummberOfEmailMessages = 0;
            emailServiceEndpoint.Receive(message =>
                                             {
                                                 startingNummberOfEmailMessages++;
                                                 return null;
                                             }, new TimeSpan(0,1,0));

            INotificationSender notificationSender = new NotificationSender(emailServiceEndpointUri, _endpointResolver);

            IEnumerable<EmailAddress> emails = new List<EmailAddress>
                                                   {
                                                       new EmailAddress
                                                           {
                                                               Address = "gregbanister@hotmail.com",
                                                               DisplayName = "test"
                                                           }
                                                   };

            notificationSender.SendNotification("test", emails, "test");

            int finalNummberOfEmailMessages = 0;
            emailServiceEndpoint.Receive(message =>
                                             {
                                                 finalNummberOfEmailMessages++;
                                                 return null;
                                             }, new TimeSpan(0, 1, 0));

            Assert.That(finalNummberOfEmailMessages, Is.EqualTo(startingNummberOfEmailMessages + 1));
        }
    }
}