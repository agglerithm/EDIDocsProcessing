using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EdiMessages;

namespace FlexEDIDocsIn
{
    public interface IMessageGenerator
    {
        IEnumerable<IEdiMessage> GenerateMessages(EdiFileInfo fileInfo, BusinessPartner partner);
    }
}