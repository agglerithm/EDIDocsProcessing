using System;
using System.Collections.Generic;
using System.Linq;
using EdiMessages;
using MassTransit;
using Microsoft.Practices.ServiceLocation;

namespace EDIDocsProcessing.Core
{
    public interface IExecutePostConsumeAction
    {
        void Execute(IEdiMessage message);
    }

    public class ExecutePostConsumeAction : IExecutePostConsumeAction
    {
        private readonly IList<IPostActionSpecification> _specs;

        public ExecutePostConsumeAction()
        {
            _specs = new List<IPostActionSpecification>(ServiceLocator.Current.GetAllInstances<IPostActionSpecification>());
        }

        public void Execute(IEdiMessage message)
        {
            if (_specs.Count == 0) throw new Exception("Specifications are not Registered");

            _specs.Where(spec => spec.IsSatisfiedBy(message))
                .Each(spec => spec.Execute(message));
        }

    }
}

