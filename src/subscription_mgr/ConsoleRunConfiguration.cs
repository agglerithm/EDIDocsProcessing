using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.Services.Subscriptions.Server;
using Topshelf.Configuration;
using Topshelf.Internal;
using Topshelf.Internal.Actions;

namespace subscription_mgr
{
 

    internal class ConsoleRunConfiguration :  IRunConfiguration
    {
 
 
        public void ConfigureServiceInstaller(ServiceInstaller installer)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServiceProcessInstaller(ServiceProcessInstaller installer)
        {
            throw new NotImplementedException();
        }

        public IServiceCoordinator Coordinator
        {
            get { throw new NotImplementedException(); }
        }

        public Credentials Credentials
        {
            get { throw new NotImplementedException(); }
        }

        public WinServiceSettings WinServiceSettings
        {
            get { throw new NotImplementedException(); }
        }

        public Type FormType
        {
            get { throw new NotImplementedException(); }
        }

        public NamedAction DefaultAction
        {
            get { throw new NotImplementedException(); }
        }
    }
}
