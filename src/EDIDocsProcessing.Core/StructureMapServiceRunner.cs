using AFPST.Common.Infrastructure;
using MassTransit.EndpointConfigurators;
using MassTransit.Transports;
using Microsoft.Practices.ServiceLocation;
using Topshelf;
using Topshelf.Configuration.Dsl;

namespace EDIDocsProcessing.Core
{
    [CoverageExclude("unsure how to test this", "Greg")]
    public class StructureMapServiceRunner<TServiceType> where TServiceType : class, ISubscriptionService 
    {
        private readonly string _srvName;
        private readonly string _srvDescription;

        public StructureMapServiceRunner(string srvName, string srvDescription)
        {
            _srvName = srvName;
            _srvDescription = srvDescription;
        }

        public void Start()
        {

            var host = HostFactory.New(c =>
                                           {
                                               c.SetServiceName(_srvName);
                                               c.SetDisplayName(_srvName);
                                               c.SetDescription(_srvDescription);
                                               c.DependsOnMsmq();
                                               c.DependsOn("MassTransit.RuntimeServices");
                                               c.RunAsLocalSystem();
                                               c.Service<TServiceType>(s =>
                                                                           {
                                                                               s.ConstructUsing(
                                                                                   builder =>
                                                                                   ServiceLocator.Current.GetInstance
                                                                                       <TServiceType>());
                                                                               s.WhenStarted(o => o.Start(false));
                                                                               s.WhenStopped(o => o.Stop());
                                                                           });
                                           });
            host.Run();
        }
    }
}
