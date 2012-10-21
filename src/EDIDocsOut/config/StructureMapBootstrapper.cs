namespace EDIDocsOut.config
{ 
    using AFPST.Common.Config;
    using AFPST.Common.Infrastructure;
    using AFPST.Common.Infrastructure.impl;
    using EDIDocsProcessing.Common.configs;
    using EDIDocsProcessing.Core.Config;
    using InitechEDIDocsOut.config;
    using MassTransitContrib;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap;


    public static class StructureMapBootstrapper
    {
        public static void Execute()
        {
            var config = new AFPSTConfiguration();

            ObjectFactory.Initialize(x =>
            {
                                             x.AddRegistry(new MassTransitContribRegistry());
                                             x.AddRegistry(new EdiDocsOutRegistry());
                                             x.AddRegistry(new EdiDocsCommonRegistry());
                                             x.AddRegistry(new AfpstCommonCoreRegistry()); 
                                             x.AddRegistry(new EdiDocsProcessingCoreRegistry(config.TestMode));   
                                             x.AddRegistry(new InitechEdiDocsOutRegistry()); 
                                             x.AddRegistry(new StructureMapRegistry());
                                         });

            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(ObjectFactory.Container));
        }
    }
}