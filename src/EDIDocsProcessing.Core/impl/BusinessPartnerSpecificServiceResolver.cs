using System;
using System.Linq;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsOut;
using Microsoft.Practices.ServiceLocation;

namespace EDIDocsProcessing.Core.impl
{
    public class BusinessPartnerSpecificServiceResolver : IBusinessPartnerSpecificServiceResolver
    {
        public ISegmentFactory GetSegmentFactoryFor(BusinessPartner partner)
        {
            var segFactory = ServiceLocator.Current.GetInstance<ISegmentFactory>();

            segFactory.SetBuildValues(partner);

            return segFactory;
        }

        public IFileParser GetFileParserFor(BusinessPartner partner)
        { 
            return ServiceLocator.Current.GetInstance<IFileParser>(string.Format("{0}FILEPARSER", partner.Code));
        }

        public IFileParser GetFileParserBySenderId(string senderId)
        {
            var lst = ServiceLocator.Current.GetAllInstances(typeof (IFileParser)).Select(p => (IFileParser) p);
            return lst.Find(p => p.CanParseSenderId(senderId));
        }

        public bool TestMode
        {
            get
            {
                var config = ServiceLocator.Current.GetInstance<IAFPSTConfiguration>();
                return config.TestMode;
            }
        }

        public IDocumentParser GetDocumentParserFor(BusinessPartner partner, string docType)
        {
            return ServiceLocator.Current.GetInstance<IDocumentParser>(string.Format("{0}{1}DOCPARSER", partner.Code, docType.Trim()));
        }

        public IBuildValueFactory GetBuildValueFactoryFor(BusinessPartner partner)
        {
            var factories = ServiceLocator.Current.GetAllInstances<IBuildValueFactory>();
            return factories.Find(f => f.For(partner));
        }
    }
}
