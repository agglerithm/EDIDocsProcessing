namespace EDIDocsProcessing.Common.EDIStructures
{
    public enum TransportAgent
    {
        Edict,
        Initech,
        MicroCenter
    } ;
    public class EdiXmlBuildValues
    {
        public string ElementDelimiter { get; set; }
        public string SegmentDelimiter { get; set; }
        public string InterchangeReceiverID { get; set; } 
        public string FunctionGroupReceiverID { get; set; }
        public TransportAgent Transport { get; set; }

        public string InterchangeReceiverQualifier {get; set;}

        public string InterchangeSenderQualifier { get; set; }
        public string InterchangeSenderID { get; set; } 
    }
}