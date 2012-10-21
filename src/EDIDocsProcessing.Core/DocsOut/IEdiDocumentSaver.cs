using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IEdiDocumentSaver
    {
        void Save(EDITransmissionPackage ediTransmissionPackage);
        void SaveAsXml(EDITransmissionPackage ediTransmissionPackage);
    }
}