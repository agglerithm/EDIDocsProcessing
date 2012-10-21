using System;

namespace EDIDocsOut.Infrastructure
{
    public class CoverageExcludeAttribute : Attribute
    {
        private readonly string m_author;
        private readonly string m_reason;

        public CoverageExcludeAttribute(string reason,
                                        string author)
        {
            m_reason = reason;
            m_author = author;
        }

        public string Author
        {
            get { return m_author; }
        }

        public string Reason
        {
            get { return m_reason; }
        }
    }
}