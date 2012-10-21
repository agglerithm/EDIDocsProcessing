using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class Segment
    {
        public string Contents
        {
            get; set;
        }

        public string Label
        {
            get; set;
        }

        public string Context
        {
            get; set;
        }

        public List<string> GetElements(string element_delimiter)
        {
            if (Contents == null)
                return null;
            return Contents.Split(element_delimiter.ToCharArray()).ToList();
        }
    }
}