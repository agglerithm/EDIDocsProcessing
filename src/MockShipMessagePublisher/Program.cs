using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Windsor;
using EDIDocsProcessing.Common.Infrastructure;
using log4net.Config;
using MassTransit.WindsorIntegration;
using Microsoft.Practices.ServiceLocation;
using Topshelf;
using Topshelf.Configuration;

namespace MockShipMessagePublisher
{
    static class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("MockShipMessagePublisher.log4net.xml"));

            var cfg = RunnerConfigurator.New(c =>
            {
                c.SetServiceName("Mock Shipping Msg Service");
                c.SetDisplayName("Mock Shipping Msg Service");
                c.SetDescription("a service for faking shipping messages and Publishing the corresponding events on the Bus.");

                c.DependencyOnMsmq();
                c.RunAsFromInteractive(); //Is this right?
                c.BeforeStart(a => { });

                c.ConfigureService<ShippingMessageService>(s =>
                {
                    s.CreateServiceLocator(() =>
                    {
                        IWindsorContainer container = new DefaultMassTransitContainer("MockShipMessagePublisher.Castle.xml");
                        Container.InitializeWith(container);
                       Environment.Setup();

                        container.AddComponent<ShippingMessageService>();
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
