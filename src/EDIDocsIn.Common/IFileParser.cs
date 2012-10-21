using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdiMessages;

namespace EDIDocsProcessing.Common
{
    public interface IFileParser 
    { 
        bool CanParseSenderId(string senderId);
        bool CanParseFor(BusinessPartner partner);
        IEnumerable<IEdiMessage> Parse( EdiSegmentCollection  segList); 
    }
}
