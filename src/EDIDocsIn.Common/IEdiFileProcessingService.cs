using System.Collections.Generic;
using EdiMessages;

namespace EDIDocsProcessing.Common
{
    public interface IEdiFileProcessingService 
    { 
        IEnumerable<IEdiMessage> ProcessFiles(TransmissionPath partner);
    }
}