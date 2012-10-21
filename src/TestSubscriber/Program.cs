using System;
using System.IO;
using Castle.Windsor;
using log4net.Config;
using MassTransit.WindsorIntegration;
using Microsoft.Practices.ServiceLocation;
using Topshelf;
using Topshelf.Configuration;

namespace TestSubscriber
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
                        XmlConfigurator.Configure(new FileInfo("testsubscriber.log4net.xml"));
            
            var cfg = RunnerConfigurator.New(c =>
             {
                 c.SetServiceName("Test Subscriber");
                 c.SetDisplayName("Test Subscriber");
                 c.SetDescription("a Mass Transit test service for handling new customer orders.");

                 c.RunAsLocalSystem();
                 c.DependencyOnMsmq();

                 c.BeforeStart(a => { });

                 c.ConfigureService<TestSubscriberService>(s=>
                   {
						s.CreateServiceLocator(() =>
						{
                            IWindsorContainer container = new DefaultMassTransitContainer("TestSubscriber.Castle.xml");
                            container.AddComponent<TestSubscriberService>();
                            container.AddComponent<Subscriber>();
                            return ServiceLocator.Current;
                        });
                                                
                       s.WhenStarted(o => o.Start());
                       s.WhenStopped(o=>o.Stop());
                   });
             });
            Runner.Host(cfg, args);
        }
    }
}
