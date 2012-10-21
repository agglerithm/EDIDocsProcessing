﻿// Copyright 2007-2008 The Apache Software Foundation.
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
using System;
using Microsoft.Practices.ServiceLocation;
using Topshelf.Configuration;
using Topshelf.Internal;

namespace subscription_mgr.Topshelf
{
    public interface IServiceConfigurator<TService> :
    IDisposable
    {
        void WhenStarted(Action<TService> startAction);
        void WhenStopped(Action<TService> stopAction);
        void WhenPaused(Action<TService> pauseAction);
        void WhenContinued(Action<TService> continueAction);

        void CreateServiceLocator(Func<IServiceLocator> createServiceLocator);
    }

    public class ServiceConfigurator<TService> :
        ServiceConfiguratorBase<TService>,
        IServiceConfigurator<TService>
    {
        public ServiceConfigurator(string name)
            : base(name)
        {
        }

        public IService Create()
        {
            IService service = new Service<TService>
                                   {
                                       CreateServiceLocator = _createServiceLocator,
                                       StartAction = _startAction,
                                       StopAction = _stopAction,
                                       PauseAction = _pauseAction,
                                       ContinueAction = _continueAction,
                                       Name = _name,
                                   };

            return service;
        }
    }
}