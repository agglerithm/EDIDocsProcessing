using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IMessageGenerator
    {
        IEnumerable<IEdiMessage> GenerateMessages(EdiFileInfo fileInfo, BusinessPartner partner);
    }
}