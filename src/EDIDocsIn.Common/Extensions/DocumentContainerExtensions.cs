using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class DocumentContainerExtensions
    {
        public static IDocumentParser ParserFor(this DocContainer doc, BusinessPartner partner)
        { 
            var parsers = ServiceLocator.Current.GetAllInstances(typeof(IDocumentParser)).Select(p => (IDocumentParser)p);
            return parsers.Find(p => p.CanProcess(partner,doc.DocType));
        }
 
    }
}
