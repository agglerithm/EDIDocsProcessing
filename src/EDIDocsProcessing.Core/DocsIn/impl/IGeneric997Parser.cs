namespace EDIDocsProcessing.Core.DocsIn.impl
{ 
    using System.Collections.Generic;
    using System.Linq;
    using AFPST.Common.Extensions; 
    using Common.EDIStructures;
    using EdiMessages;

    public interface IGeneric997Parser
    {
        Acknowledgements ProcessSegmentList(List<Segment> segList);
    }

    public class Generic997Parser : IGeneric997Parser
{
        private string _elementDelimiter;
 
        public Acknowledgements ProcessSegmentList(List<Segment> segList)
        {
            var st = segList.First();
            _elementDelimiter = get_element_delimiter(st);

            var header = st.GetElements(_elementDelimiter); 
            var acks = new Acknowledgements();
            acks.ControlNumber = header[2];
            var akList = getAK2List(segList);
            acks.Acks  = akList.Select(getAck).ToList(); 
            return acks;

        }

        private  ReceiptAcknowledgementMsg  getAck(Segment segment )
        {
            var els = segment.GetElements(_elementDelimiter);
            var ack = new ReceiptAcknowledgementMsg(); 
            ack.ControlNumber = els[2];
            ack.DocumentId = els[1].CastToInt();

            return ack;
        }

        private static string get_element_delimiter(Segment header)
        {
            return header.Contents.Substring(2, 1);
        }


        private IEnumerable<Segment> getAK2List(List<Segment> segList)
        {
            var segs = segList.Where(s => s.Label.Text == "AK2");
            if (segs.Count() == 0)
                throw new InvalidEDIDocumentException("AK2 Segment is missing!");
            return segs;
        }
}
}