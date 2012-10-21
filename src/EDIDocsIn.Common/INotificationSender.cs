namespace EDIDocsProcessing.Common
{
    using System.Collections.Generic;
    using EmailService.Messages;

    public interface INotificationSender
    {
        void SendNotification(string subject, IEnumerable<EmailAddress> recipients, string body);
    }
}