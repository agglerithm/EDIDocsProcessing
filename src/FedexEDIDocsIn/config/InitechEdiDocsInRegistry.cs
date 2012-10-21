using StructureMap.Configuration.DSL;

namespace InitechEDIDocsIn.config
{
    public class InitechEdiDocsInRegistry : Registry
    {
        public InitechEdiDocsInRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();
                     });
        }
    }
}