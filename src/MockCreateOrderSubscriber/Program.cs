using System.IO;
using Castle.Windsor; 
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Core;
using EdiMessages;
using log4net.Config;
using MassTransit.WindsorIntegration;
using Microsoft.Practices.ServiceLocation;
using Topshelf;
using Topshelf.Configuration;

namespace MockCreateOrderSubscriber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("MockOrder.log4net.xml")); 
            var cfg = RunnerConfigurator.New(c =>
            {
                c.SetServiceName("MockOrderSubscriber");
                c.SetDisplayName("MockOrderSubscriber");
                c.SetDescription("a service for mocking up order processing.");

                c.DependencyOnMsmq();
                c.RunAsLocalSystem();

                c.BeforeStart(a => { });

                c.ConfigureService<MockOrderService>(s =>
                {
                    s.CreateServiceLocator(() =>
                    {
                        IWindsorContainer container = new DefaultMassTransitContainer("MockOrder.Castle.xml");

                        Container.InitializeWith(container);
                        Environment.Setup();

                        container.AddComponent<MockOrderService>();
                        container.AddComponent<Subscriber<CreateOrderMessage>>(); 

                        //var builder = new WindsorObjectBuilder(container.Kernel);
                        //ServiceLocator.SetLocatorProvider(() => builder);

                        return ServiceLocator.Current;

                    });

                    s.WhenStarted(o => o.Start());
                    s.WhenStopped(o => o.Stop());
                });
            });
            Runner.Host(cfg, args);
        }
    }
}
