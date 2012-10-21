using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EdiMessages;

namespace EDIDocsProcessing.Common
{
    public class DocumentRecordPackage
    {
        public DocumentRecordPackage(IEdiMessage msg, IEnumerable<ResponseElementEntity> responseValues, IEnumerable<DocumentLineItemEntity> lines)
        {
            Message = msg;
            ResponseValues = responseValues;
            Lines = lines;
        }

        public IEdiMessage Message { get; private set; }
        public IEnumerable<ResponseElementEntity> ResponseValues { get; private set; }

        public IEnumerable<DocumentLineItemEntity> Lines
        {
            get; private set; 
        }
    }
}