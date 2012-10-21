using AFPST.Common.Config;
using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using EDIDocsProcessing.Common.configs;
using EDIDocsProcessing.Core.Config;
using InitechEDIDocsIn.config;
using InitechEDIDocsOut.config;
using MassTransitContrib;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace EDIDocsIn.config
{
    public static class StructureMapBootstrapper
    {
        public static void Execute()
        {
            var config = new AFPSTConfiguration();

            ObjectFactory.Initialize(x =>
            {
                                             x.AddRegistry(new MassTransitContribRegistry());
                                             x.AddRegistry(new EdiDocsCommonRegistry());
                                             x.AddRegistry(new EdiDocsInRegistry());
                                             x.AddRegistry(new EdiDocsProcessingCoreRegistry(config.TestMode));
                                             x.AddRegistry(new AfpstCommonCoreRegistryWithoutDataUtilities());
                                             x.AddRegistry(new InitechEdiDocsInRegistry());
                                             x.AddRegistry(new InitechEdiDocsOutRegistry()); 
                                         });

            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(ObjectFactory.Container));
        }
    }
}