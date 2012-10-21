namespace EDIDocsProcessing.Core
{
    public interface ISubscriptionService
    { 
        void Start(bool runOnce);
        void Stop();
    }
}