using System.Xml.Linq;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class XElementExtensions
    { 
        public static void SafeRemove(this XElement el)
        {
            if(el != null && el.Parent != null) el.Remove();
        }

        public static void SafeRemove(this XAttribute attr)
        {
            if (attr != null) attr.Remove();
        }

        public static string GetSafeValue(this XElement el)
        {
            if (el == null) return null;
            return el.Value;
        }

        public static string GetSafeValue(this XAttribute attr)
        {
            if (attr == null) return null;
            return attr.Value;
        }
    }
}