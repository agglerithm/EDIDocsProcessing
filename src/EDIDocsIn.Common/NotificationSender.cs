namespace EDIDocsProcessing.Common
{
    using System.Collections.Generic;
    using AFPST.Common.Infrastructure;
    using EmailService.Messages;
    using MassTransit;

    public class NotificationSender : INotificationSender
    {
        private readonly IEndpointCache _endpointResolver;
        private readonly Queue _notificationEndpoint; 

        public NotificationSender(Queue notificationEndpoint, IEndpointCache endpointResolver)
        {
            _endpointResolver = endpointResolver;
            _notificationEndpoint = notificationEndpoint;
        }


        public void SendNotification(string subject, IEnumerable<EmailAddress> recipients, string body)
        {
            var message = new SendNotificationMessage
                              {
                                  Recipients = recipients,
                                  Subject = subject,
                                  Body = body,
                                  From =
                                      new EmailAddress
                                          {
                                              Address = EmailAddressConstants.InformationtechnologygroupEmailAddress,
                                              DisplayName = "Austin Foam Plastic IT"
                                          },
                                  IsBodyHtml = false
                              };

            IEndpoint endpoint = _endpointResolver.GetEndpoint(_notificationEndpoint.ToString());

            endpoint.Send(message);
            
        }

    }
}