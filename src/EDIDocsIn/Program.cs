using AFPST.Common.Infrastructure;
using AFPST.Common.Services.Logging;
using EDIDocsIn.config;
using EDIDocsProcessing.Core;

namespace EDIDocsIn
{
    [CoverageExclude("unsure how to test this", "Greg")]
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.EnsureInitialized();

            Logger.Info(typeof(Program), "Starting EdiDocsIn");
 

            BootstrapProgram();

            var runner =
                new StructureMapServiceRunner<EdiDocsInService>("AFP-EdiDocumentsIn",
                                "a service for receiving Edi Files and Publishing the corresponding events on the Bus.");
            
            runner.Start();
        }

        public static void BootstrapProgram()
        {
            StructureMapBootstrapper.Execute();
            MassTransitEdiDocsInBootstrapper.Execute();
        }
    }
}
