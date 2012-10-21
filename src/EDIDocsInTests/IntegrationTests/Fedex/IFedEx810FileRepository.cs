namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    using System;
    using System.Collections.Generic;
    using AFPST.Common;
    using AFPST.Common.Services;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.Enumerations;
    using EDIDocsProcessing.Common.Extensions;
    using EdiMessages;

    public interface IInitech810FileRepository
    {
        IEnumerable<EdiInvoice> GetInvoicesFrom(string path);
    }

    public class EdiInvoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime SentDate { get; set; }
        public decimal Sales { get; set; }
        public decimal Tax { get; set; }
    }
    public class Initech810FileRepository : IInitech810FileRepository 
    {
        private readonly IFileUtilities _fileUtil;
        private IFileParser _parser;
        private ISegmentSplitter _splitter;

        public Initech810FileRepository(IFileUtilities fileUtil, ISegmentSplitter splitter)
        {
            _fileUtil = fileUtil;
            _splitter = splitter;
            _parser = BusinessPartner.Initech.Parser(); 
        }

        public IEnumerable<EdiInvoice> GetInvoicesFrom(string path)
        {
            var files = _fileUtil.GetListFromFolder(path, "IN*.out", DateTime.Today.AddYears(-1));

            var lst = new List<EdiInvoice>();
            files.ForEach(f =>
                              {
                                  var contents = Utilities.QuickFileText(f.FullPath);
                                  var segs = _splitter.Split(contents); 
                                  lst.Add(createInvoiceFrom(segs));
                              });

            return lst;
        }

        private EdiInvoice createInvoiceFrom(EdiSegmentCollection segs)
        {
            const string el = "~";
            var lst = segs.SegmentList;
            var begin = lst.Find(s => s.Label == SegmentLabel.InvoiceBegin);
            var beginEls = begin.GetElements(el);
            var groupSeg = lst.Find(s => s.Label == SegmentLabel.GroupLabel);
            var salesSeg = lst.Find(s => s.Label  == SegmentLabel.TDS);
            var taxSeg = lst.Find(s => s.Label  == SegmentLabel.TXI); 
            var inv = new EdiInvoice(); 
            inv.InvoiceDate = beginEls[1].DateFromEDIDate();
            inv.InvoiceNumber = beginEls[2];
            inv.SentDate = groupSeg.GetElements("~")[4].DateFromEDIDate();
            inv.Sales = decimal.Parse(salesSeg.GetElements(el)[1]) / 100;
            inv.Tax = decimal.Parse(taxSeg.GetElements(el)[2]);
            return inv;
        }
    }
}