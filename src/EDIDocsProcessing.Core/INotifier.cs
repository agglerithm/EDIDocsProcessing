using System;
using MassTransit;

namespace EDIDocsProcessing.Core
{
    public interface INotifier<T> where T : class 
    {
        void NotifyFailureOf(Fault<T> fault);
    }
}