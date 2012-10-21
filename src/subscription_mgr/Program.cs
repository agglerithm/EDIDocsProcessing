
    using System;
    using MassTransit.Configuration;
    using MassTransit.Saga;
    using MassTransit.Services.Subscriptions.Configuration; 
    using MassTransit.Transports.Msmq;
    using log4net;
        using MassTransit;
        using MassTransit.Services.HealthMonitoring;
    using MassTransit.Services.Subscriptions.Server;
        using MassTransit.Services.Timeout;
        using MassTransit.WindsorIntegration;
        using Microsoft.Practices.ServiceLocation;
    using Topshelf;
    using Topshelf.Configuration;

namespace subscription_mgr
{
    // Copyright 2007-2008 The Apache Software Foundation.
    //  
    // Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
    // this file except in compliance with the License. You may obtain a copy of the 
    // License at 
    // 
    //     http://www.apache.org/licenses/LICENSE-2.0 
    // 
    // Unless required by applicable law or agreed to in writing, software distributed 
    // under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
    // CONDITIONS OF ANY KIND, either express or implied. See the License for the 
    // specific language governing permissions and limitations under the License.

        internal static class Program
        {
            private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
            private static WindsorObjectBuilder _wob;

            private static void Main(string[] args)
            {
                _log.Info("subscription_mgr Loading...");
 
                Console.WriteLine("MSMQ Subscription Service");
                var cfg = RunnerConfigurator.New(c =>
                {
                    c.SetServiceName("subscription_mgr");
                    c.SetDisplayName("Subscription Service");
                    c.SetDescription("Coordinates subscriptions between multiple systems"); 
                    c.DependencyOnMsmq();
                    c.RunAsLocalSystem(); 

                    c.BeforeStartingServices(s =>
                    {
                        var container = new DefaultMassTransitContainer();

                        IEndpointFactory endpointFactory = EndpointFactoryConfigurator.New(e =>
                        {
                            e.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                            e.RegisterTransport<MsmqEndpoint>();
                        });
                        container.Kernel.AddComponentInstance("endpointFactory", typeof(IEndpointFactory), endpointFactory);
 

                        _wob = new WindsorObjectBuilder(container.Kernel); 
                        ServiceLocator.SetLocatorProvider(() => _wob);
                        
                    });

                    c.ConfigureService<SubscriptionService>(ConfigureSubscriptionService);

                    c.ConfigureService<TimeoutService>(ConfigureTimeoutService);

                    c.ConfigureService<HealthService>(ConfigureHealthService);
 
                });
                try
                { 
                    Runner.Host(cfg, args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying to run service: " + ex);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(); 
                } 
 
            }

 

            private static void ConfigureSubscriptionService(IServiceConfigurator<SubscriptionService> configurator)
            { 
                configurator.CreateServiceLocator(() =>
                {
                    var container = new DefaultMassTransitContainer();

                    container.AddComponent("sagaRepository", typeof(ISagaRepository<>), typeof(InMemorySagaRepository<>));

                    container.AddComponent<ISubscriptionRepository, InMemorySubscriptionRepository>();
                    container.AddComponent<SubscriptionService, SubscriptionService>(typeof(SubscriptionService).Name);

                    var endpointFactory = EndpointFactoryConfigurator.New(x =>
                    {
                        // the default
                        x.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        x.RegisterTransport<MsmqEndpoint>();
                    });

                    container.Kernel.AddComponentInstance("endpointFactory", typeof(IEndpointFactory), endpointFactory);

                    var bus = ServiceBusConfigurator.New(servicesBus =>
                    {
                        servicesBus.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        servicesBus.ReceiveFrom("msmq://localhost/mt_subscriptions");
                    });

                    container.Kernel.AddComponentInstance("bus", typeof(IServiceBus), bus);

                    return container.ObjectBuilder;
                });

                configurator.WhenStarted(service => service.Start());

                configurator.WhenStopped(service =>
                {
                    service.Stop();
                    service.Dispose();
                });
            }

            private static void ConfigureTimeoutService(IServiceConfigurator<TimeoutService> configurator)
            {
                configurator.CreateServiceLocator(() =>
                {
                    var container = new DefaultMassTransitContainer();

                    container.AddComponent<ITimeoutRepository, InMemoryTimeoutRepository>();
                    container.AddComponent<TimeoutService>(typeof(TimeoutService).Name);

                    var endpointFactory = EndpointFactoryConfigurator.New(x =>
                    {
                        // the default
                        x.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        x.RegisterTransport<MsmqEndpoint>();
                    });

                    container.Kernel.AddComponentInstance("endpointFactory", typeof(IEndpointFactory), endpointFactory);

                    var bus = ServiceBusConfigurator.New(x =>
                    {
                        x.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        x.ReceiveFrom("msmq://localhost/mt_timeout");
                        x.ConfigureService<SubscriptionClientConfigurator>(client =>
                        {
                            // need to add the ability to read from configuratino settings somehow
                            client.SetSubscriptionServiceEndpoint("msmq://localhost/mt_subscriptions");
                        });
                    });
                    container.Kernel.AddComponentInstance("bus", typeof(IServiceBus), bus);

                    return container.Resolve<IObjectBuilder>();
                });

                configurator.WhenStarted(service => { service.Start(); });

                configurator.WhenStopped(service =>
                {
                    service.Stop();
                    service.Dispose();
                });
            }

            private static void ConfigureHealthService(IServiceConfigurator<HealthService> configurator)
            {
                configurator.CreateServiceLocator(() =>
                {
                    var container = new DefaultMassTransitContainer();

                    container.AddComponent<HealthService>(typeof(HealthService).Name);
                    container.AddComponent("sagaRepository", typeof(ISagaRepository<>), typeof(InMemorySagaRepository<>));

                    var endpointFactory = EndpointFactoryConfigurator.New(x =>
                    {
                        // the default
                        x.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        x.RegisterTransport<MsmqEndpoint>();
                    });

                    container.Kernel.AddComponentInstance("endpointFactory", typeof(IEndpointFactory), endpointFactory);

                    var bus = ServiceBusConfigurator.New(x =>
                    {
                        x.SetObjectBuilder(container.Resolve<IObjectBuilder>());
                        x.ReceiveFrom("msmq://localhost/mt_health");
                        x.ConfigureService<SubscriptionClientConfigurator>(client =>
                        {
                            // need to add the ability to read from configuratino settings somehow
                            client.SetSubscriptionServiceEndpoint("msmq://localhost/mt_subscriptions");
                        });
                    });

                    container.Kernel.AddComponentInstance("bus", typeof(IServiceBus), bus);

                    return container.Resolve<IObjectBuilder>();
                });

                configurator.WhenStarted(service => { service.Start(); });

                configurator.WhenStopped(service =>
                {
                    service.Stop();
                    service.Dispose();
                });
            }
        }
    } 
