namespace EDIDocsProcessing.Core.DocsOut.Impl
{ 
    using AFPST.Common.Extensions;
    using AFPST.Common.Services.Logging;
    using EdiMessages;
    using Microsoft.Practices.ServiceLocation;


    public class BusinessPartnerResolver<T> : IBusinessPartnerResolver<T> where T : IEdiMessage
    {
        public ICreateEdiContentFrom<T> ResolveFrom(T message)
        { 
            var documentCreators = ServiceLocator.Current.GetAllInstances<ICreateEdiContentFrom<T>>();
                
            
            var documentCreator = documentCreators.Find(c => c.CanProcess(message));

            if (documentCreator == null) Logger.Info(this, string.Format("No implementation for ICreateEdiContentFrom found for partner = {0}, doc ID = {1}, purpose = {2}", message.BusinessPartnerCode,message.DocumentId,
                            message.BusinessPurpose 
                          )  );

            return documentCreator;
        }


    }
}