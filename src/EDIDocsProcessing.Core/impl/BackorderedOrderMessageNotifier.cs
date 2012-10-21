using System.Collections.Generic;
using EdiMessages;
using EmailService.Messages;
using MassTransit;

namespace EDIDocsProcessing.Core.impl
{
    using Common;

    public class BackorderedOrderMessageNotifier : INotifier<OrderIsBackorderedMessage>
    {
        private readonly INotificationSender _notificationSender;

        public BackorderedOrderMessageNotifier(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public void NotifyFailureOf(Fault<OrderIsBackorderedMessage> fault)
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

            _notificationSender.SendNotification("Failure occured processing this Backordered Order Message", recipients, body);

        }

    }
}