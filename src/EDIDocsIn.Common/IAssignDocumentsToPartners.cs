namespace EDIDocsProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AFPST.Common.IO;
    using Extensions;

    public class DocumentFile
    {
        public DocumentFile(FileEntity f, EdiSegmentCollection segs)
        {
            Segments = segs;
            File = f;
        }
        public EdiSegmentCollection Segments { get; private set; }
        public FileEntity File { get; private set; }
    }
    public class DocumentAssignments
    {
        public DocumentAssignments(BusinessPartner partner)
        {
            Partner = partner;
            Documents = new List<DocumentFile>();
        }

        public BusinessPartner Partner
        {
            get; private set;
        }

        public IList<DocumentFile> Documents
        {
            get; private set;
        }
    }

    public interface IAssignDocumentsToPartners
    {
        IEnumerable<DocumentAssignments> MakeAssignments(IList<FileEntity> files);
    }

    public class DocumentPartnerAssigner : IAssignDocumentsToPartners
    {
        private readonly ISegmentSplitter _segmentSplitter;

        public DocumentPartnerAssigner(ISegmentSplitter segmentSplitter)
        {
            _segmentSplitter = segmentSplitter;
        }

        public IEnumerable<DocumentAssignments> MakeAssignments(IList<FileEntity> files)
        {
            var assignments = new List<DocumentAssignments>();

            files.ForEach(f =>
                              {
                                  var doc = get_segments(f);
                                  var partner = get_partner(doc);
                                  var assignment = assignments.Find(a => a.Partner == partner);
                                  if (assignment == null)
                                  {
                                      assignment = new DocumentAssignments(partner);
                                      assignments.Add(assignment);
                                  }
                                  assignment.Documents.Add(new DocumentFile(f,doc));
                              });
            return assignments;
        }

        private EdiSegmentCollection get_segments(FileEntity file)
        {
            var contents = FileUtilitiesExtensions.QuickFileText(file.FullPath);
            contents = contents.Replace("\r\n", "");
            return _segmentSplitter.Split(contents);
        }

        private BusinessPartner get_partner(EdiSegmentCollection doc)
        {
            var seg = doc.SegmentList.First();
            var els = seg.GetElements(seg.Contents.Substring(3, 1));
            return BusinessPartner.FromReceiverId(els[6]);
        }
    }
}