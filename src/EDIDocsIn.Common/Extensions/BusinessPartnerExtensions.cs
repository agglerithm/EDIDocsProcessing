using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class BusinessPartnerExtensions
    {
        public static IFileParser Parser(this BusinessPartner partner)
        {
            var parsers = ServiceLocator.Current.GetAllInstances(typeof(IFileParser)).Select(p => (IFileParser)p);
            return parsers.Find(p => p.CanParseFor(partner));
        }
    }
}
