using EdiMessages;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IBusinessPartnerResolver<T> where T:IEdiMessage
    {
        ICreateEdiContentFrom<T>  ResolveFrom(T message);
    }
}
