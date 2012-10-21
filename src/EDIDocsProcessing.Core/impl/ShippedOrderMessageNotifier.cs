using System.Collections.Generic;
using EdiMessages;
using EmailService.Messages;
using MassTransit;

namespace EDIDocsProcessing.Core.impl
{
    using Common;

    public class ShippedOrderMessageNotifier : INotifier<OrderHasBeenShippedMessage>
    {
        private readonly INotificationSender _notificationSender;

        public ShippedOrderMessageNotifier(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public void NotifyFailureOf(Fault<OrderHasBeenShippedMessage> fault)
        {
            IList<EmailAddress> recipients = new List<EmailAddress>
            {
                new EmailAddress {Address = EmailAddressConstants.InformationtechnologygroupEmailAddress}
            };

            if (fault.FailedMessage.BusinessPartnerCode.Contains("WWT"))
            {
                recipients.Add(new EmailAddress { Address = EmailAddressConstants.LogisticsEmailAddress });
            }

            if (fault.FailedMessage.BusinessPartnerCode.Contains("FEDE"))
            {
                //TODO: email account manager???
            }

            string body = string.Format(
                          @"Failed Message:
                                {0}

                              Caught Execption
                                {1}
                            ", fault.FailedMessage, fault.CaughtException);

            _notificationSender.SendNotification("Failure occured processing this Shipped Order Message", recipients, body);
        
        }

    }


    public class OrderRequestReceivedMessageNotifier : INotifier<OrderRequestReceivedMessage>
    {
        private readonly INotificationSender _notificationSender;

        public OrderRequestReceivedMessageNotifier(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public void NotifyFailureOf(Fault<OrderRequestReceivedMessage> fault)
        {
            IList<EmailAddress> recipients = new List<EmailAddress>
            {
                new EmailAddress {Address = EmailAddressConstants.InformationtechnologygroupEmailAddress}
            };

            if (fault.FailedMessage.BusinessPartnerCode.Contains("WWT"))
            {
                recipients.Add(new EmailAddress { Address = EmailAddressConstants.LogisticsEmailAddress });
            }

            if (fault.FailedMessage.BusinessPartnerCode.Contains("FEDE"))
            {
                //TODO: email account manager???
            }

            string body = string.Format(
                          @"Failed Message:
                                {0}

                              Caught Execption
                                {1}
                            ", fault.FailedMessage, fault.CaughtException);

            _notificationSender.SendNotification("Failure occured processing this Order Request Received Message", recipients, body);

        }

    }
}