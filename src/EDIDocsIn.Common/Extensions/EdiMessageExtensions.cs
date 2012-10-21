using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsProcessing.Common.Publishers;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class EdiMessageExtensions
    {
        public static IEdiMessagePublisher Publisher(this IEdiMessage ediMessage)
        {
            var publishers = ServiceLocator.Current.GetAllInstances(typeof(IEdiMessagePublisher)).Select(p => (IEdiMessagePublisher)p);
            return publishers.Find(p => p.CanPublish(ediMessage));
        }


        public static void Publish(this IEdiMessage obj)
        {
            var publisher = obj.Publisher();
            if (publisher == null)
                throw new InvalidOperationException(string.Format("No publisher found for message type {0}", obj.GetType().Name));
            publisher.PublishMessage(obj);
        } 
    }
}
