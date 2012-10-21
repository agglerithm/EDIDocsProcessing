using AFPST.Common.Infrastructure;
using AFPST.Common.Services.Logging;
using EDIDocsOut.config;
using EDIDocsProcessing.Core;

namespace EDIDocsOut
{
    [CoverageExclude("unsure how to test this", "Greg")]
    class Program
    {
        static void Main(string[] args)
        {
            Logger.EnsureInitialized();

            Logger.Info(typeof(Program), "Starting EdiDocsOut");

            BootstrapProgram();

            var runner =
                new StructureMapServiceRunner<EdiDocsOutSubscriberStarter>("AFP-EdiDocumentsOut",
                                                                           "a service for receiving Edi Files and Publishing the corresponding events on the Bus.");

            runner.Start( );
        }

        private static void BootstrapProgram()
        {
            StructureMapBootstrapper.Execute();
            MassTransitEdiDocsOutBootstrapper.Execute();
        }
    }
}