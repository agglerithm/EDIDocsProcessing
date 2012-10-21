using System.Collections.Generic;
using EdiMessages;
using EmailService.Messages;
using MassTransit;

namespace EDIDocsProcessing.Core.impl
{
    using Common;

    public class InvoicedOrderMessageNotifier : INotifier<InvoicedOrderMessage>
    {
        private readonly INotificationSender _notificationSender;
        

        public InvoicedOrderMessageNotifier(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public void NotifyFailureOf(Fault<InvoicedOrderMessage> fault)
        {
            IList<EmailAddress> recipients = new List<EmailAddress>
            {
                new EmailAddress {Address = EmailAddressConstants.InformationtechnologygroupEmailAddress},
                new EmailAddress {Address = EmailAddressConstants.AccountsReceivableEmailAddress}
            };

            string body = string.Format(
                         @"Failed Message:
                                {0}

                              Caught Execption
                                {1}
                            ", fault.FailedMessage, fault.CaughtException);

            _notificationSender.SendNotification("Failure occured processing this Invoiced Order Message", recipients, body);

        }
    
    }
}