using System;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Enumerations;
using EdiMessages;

namespace EDIDocsProcessing.Core
{
    public interface IAcceptMessages
    {
        bool Accept<T>(T message) where T : class, IEdiMessage;
    }

    public class AcceptMessages : IAcceptMessages
    {
        public bool Accept<T>(T message) where T : class, IEdiMessage
        {
            var isPurchaseOrder = message.DocumentId == EdiDocumentTypes.PurchaseOrder.DocumentNumber || message.DocumentId == EdiDocumentTypes.PurchaseOrderChangeRequest.DocumentNumber;
            var isASN = message.DocumentId == EdiDocumentTypes.AdvanceShipNotice.DocumentNumber;

            var isInitech = message.BusinessPartnerCode.StartsWith(BusinessPartner.VandalayIndustries.Code);

            var isInitechPurchaseOrder = isInitech && isPurchaseOrder;
            var isInitechAsn = isInitech && isASN;


            if (isInitechAsn) return false;
            if (isInitechPurchaseOrder) return false;

            return true;
        }
    }
}