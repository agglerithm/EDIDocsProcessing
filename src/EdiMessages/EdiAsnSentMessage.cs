using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMessages
{
    [Serializable]
    public class EdiAsnSentMessage
    {
        public string BOL { get; set; }
        public IList<string> LineNumbers { get; set; }
        public string ControlNumber { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var lineNumber in LineNumbers)
            {
                stringBuilder.AppendLine(string.Format("BOL: {0}, LineNumber: {1}, ControlNumber: {2}",
                                                       BOL, lineNumber, ControlNumber));
            }
            return stringBuilder.ToString();
        }
    }
}