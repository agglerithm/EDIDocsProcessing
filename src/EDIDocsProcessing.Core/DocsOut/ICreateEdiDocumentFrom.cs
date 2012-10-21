using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface ICreateEdiDocumentFrom<T>
    {
        EDITransmissionPackage CreateDocumentWith(ICreateEdiContentFrom<T> creator, T message);
    }
}