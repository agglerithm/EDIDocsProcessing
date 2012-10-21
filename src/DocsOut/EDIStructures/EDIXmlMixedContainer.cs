using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures; 

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlMixedContainer : EDIXmlContainer 
    { 
        protected EDIXmlSegment header;
        protected EDIXmlSegment footer;
        protected string control_no_element;
        protected List<string> expected_segment_names;
        protected List<string> expected_loop_names;
        protected int _segment_count;
        protected static string current_seg_name;
        protected int _control_number;

        public void AddSegment(EDIXmlSegment seg)
        {
            _segment_count++;
            base.Add(seg);
        }

        public void AddLoop(EDIXmlMixedContainer loop)
        {
            _segment_count += loop.SegmentCount;
            base.Add(loop);
        }

        public new void Add(object obj)
        {
            throw new NotImplementedException("Hidden at this level!");
        }

        public EDIXmlMixedContainer(string name):base(name)
        {
            
        }
 
        public int SegmentCount
        {
            get { return _segment_count;  }
        }
        public new EDIXmlContainer this[int ndx]
        {
            get { return (EDIXmlContainer)base[ndx]; }
        }

        protected  void initialize_expected_names()
        {
            expected_loop_names = new List<string>();
            expected_segment_names = new List<string>();
        }

 

        protected static bool compare_to_current(string s)
        {
            return s == current_seg_name;
        }

        protected bool check_name(string name, List<string> lst)
        {
            current_seg_name = name;
            return (lst.Find(compare_to_current) == name);
                    
        }

        public   string ControlNumber
        {
            get
            {
                return control_no_element == null ? "" : 
                                                           header.ElementByLabel(control_no_element).Value;
            }
            set
            {
                if (control_no_element != null)
                    header.ElementByLabel(control_no_element).Value = value;
            }
        }

        public EDIXmlSegment Segment(string seg_name)
        {
            return (EDIXmlSegment)((XElement)this).EDISegment(seg_name);
        }

        public EDIXmlMixedContainer Loop(string loop_name)
        {
            return (EDIXmlMixedContainer)((XElement)this).EDILoop(loop_name);
        }

    }
}