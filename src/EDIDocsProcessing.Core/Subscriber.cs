

namespace EDIDocsProcessing.Core
{
    using System;
    using AFPST.Common.Services.Logging;
    using Common;
    using Common.EDIStructures;
    using DocsOut;
    using EdiMessages;
    using MassTransit;
    using Microsoft.Practices.ServiceLocation;
//    public interface ISelector
//    {
//        bool Accept(IEdiMessage message);
//    }

    public class WWTSubscriber<T> : Consumes<T>.Selected where T : class, IEdiMessage
    {
        private readonly IAcceptMessages _acceptMessages;

        public WWTSubscriber(IAcceptMessages acceptMessages)
        {
            _acceptMessages = acceptMessages;
        }


        public void Consume(T message)
        {
            Logger.Info(this, "Skipping '" + message.BusinessPartnerCode + "' message. {0}", message);
        }

        public bool Accept(T message)
        {
            var accept = _acceptMessages.Accept(message); 
            return !accept;
        }
    }
    public class Subscriber<T> : Consumes<T>.Selected where T : class, IEdiMessage
    {

        private readonly IBusinessPartnerResolver<T> _businessPartnerResolver;
        private readonly ICreateEdiDocumentFrom<T> _createEdiDocumentFrom;
        private readonly IAcceptMessages _acceptMessages;
        private readonly IExecutePostConsumeAction _postConsumeAction;
        private readonly IEdiDocumentSaver _ediDocumentSaver;

        public Subscriber(IBusinessPartnerResolver<T> businessPartnerResolver,
                          ICreateEdiDocumentFrom<T> createEdiDocumentFrom,
                          IEdiDocumentSaver ediDocumentSaver, 
                          IAcceptMessages acceptMessages,
                          IExecutePostConsumeAction postConsumeAction)
        {
            _businessPartnerResolver = businessPartnerResolver;
            _acceptMessages = acceptMessages;
            _postConsumeAction = postConsumeAction;
            _createEdiDocumentFrom = createEdiDocumentFrom;
            _ediDocumentSaver = ediDocumentSaver; 
        }

        public bool Accept(T message)
        {
            var accept = _acceptMessages.Accept(message); 
            return accept;
        }

        public void Consume(T message)
        {
            try
            {
                ICreateEdiContentFrom<T> ediContentCreator = _businessPartnerResolver.ResolveFrom(message);

                if (ediContentCreator == null)
                    throw new InvalidOperationException("No matching document creator was found for business partner "
                                                        + message.BusinessPartnerCode + ", Document ID " +
                                                        message.DocumentId);

                EDITransmissionPackage package = _createEdiDocumentFrom.CreateDocumentWith(ediContentCreator, message);

                checkForProblems(message, package);
                 
                _ediDocumentSaver.Save(package);
                //_ediDocumentSaver.SaveAsXml(package);

                Logger.Info(this, "Document Saved: " + package.GetInterchangeControl().Value);

                _postConsumeAction.Execute(message);

            }
            catch (Exception ex)
            {
                Logger.Error(this, "Error processing message", ex);
                throw;
            }
        }

 

        private void checkForProblems(T message, EDITransmissionPackage package)
        {
            if (message.BusinessPartnerCode == BusinessPartner.Initech.Code)
            {
                if (BusinessPartner.Initech.Code != package.GetBusinessPartner().Code)
                {
                    var errormessage = string.Format("Business Partner Code should be Initech but it's {0}", package.GetBusinessPartner().Code);

                    Logger.Error(this, errormessage);

                    throw new InvalidOperationException(errormessage);
                }

                var valueFactory = ServiceLocator.Current.GetInstance<IBuildValueFactory>("InitechBuildValueFactory"); 

                if(valueFactory == null)
                    throw new InvalidOperationException("No appropriate factory found for InitechBuildValueFactory");

                var ediXmlBuildValues = valueFactory.GetValues();

                var interchangeControl = package.GetInterchangeControl().ToString();
                if (interchangeControl.Contains(ediXmlBuildValues.SegmentDelimiter) == false)
                {
                    var errormessage = string.Format("There's a problem with the SegmentDelimiter in the following interchange control: {0}", interchangeControl);

                    Logger.Error(this, errormessage);
                    
                    throw new InvalidOperationException(errormessage);
                }
               
            }
        }
    }


    public class FaultSubscriber<T> : Consumes<Fault<T>>.All where T : class 
    {
        private readonly INotifier<T> _notifier;

        public FaultSubscriber(INotifier<T> notifier)
        {
            _notifier = notifier;
        }

        public void Consume(Fault<T> message)
        {

            Logger.Error(this, "");
            Logger.Error(this, "The following Message failed to be Consumed:");
            Logger.Error(this, message.FailedMessage.ToString());
            Logger.Error(this, "Exception Details:");
            message.Messages.ForEach(m => Logger.Error(this, m));
            message.StackTrace.ForEach(m => Logger.Error(this, m));

            _notifier.NotifyFailureOf(message);
            
        }
    }
}