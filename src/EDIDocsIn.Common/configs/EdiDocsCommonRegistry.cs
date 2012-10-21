using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using StructureMap.Configuration.DSL;

namespace EDIDocsProcessing.Common.configs
{
    public class EdiDocsCommonRegistry : Registry
    {
        public EdiDocsCommonRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });

            For<IAFPSTConfiguration>().Use<AFPSTConfiguration>().Named("config");
        }
    }
}