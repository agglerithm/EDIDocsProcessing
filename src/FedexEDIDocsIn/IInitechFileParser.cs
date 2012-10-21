using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsIn;
using EdiMessages;


namespace InitechEDIDocsIn
{
    using System;


    public class InitechFileParser : IFileParser
    {  
        private readonly ISegmentSplitter _segmentSplitter; 
        private readonly IEdiFileReader _fileReader;
        private readonly IMessageGenerator _generator;


        public InitechFileParser(   ISegmentSplitter segmentSplitter,
                                     IEdiFileReader fileReader, IMessageGenerator generator)
        { 
            _segmentSplitter = segmentSplitter; 
            _fileReader = fileReader;
            _generator = generator;
        }

        public IEnumerable<IEdiMessage> Parse(string contents)
        {
            var segments = _segmentSplitter.Split(contents);
            var fileInfo = _fileReader.Read(segments);
            return _generator.GenerateMessages(fileInfo, BusinessPartner.Initech);
        }

 

 

        public bool CanParseSenderId(string senderId)
        {
            return senderId == EdiConfig.GetSenderIdFor(BusinessPartner.Initech);
        }

        public bool CanParseFor(BusinessPartner partner)
        {
            return partner.Value == BusinessPartner.Initech.Value;
        }

 

        public IEnumerable<IEdiMessage> Parse( EdiSegmentCollection segList)
        {
            var fileInfo = _fileReader.Read(segList);
            return _generator.GenerateMessages(fileInfo, BusinessPartner.Initech);
        }
    }
}