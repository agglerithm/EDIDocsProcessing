namespace EDIDocsProcessing.Common.IO
{
    public interface IFileCreationService
    {
        void SendFile(string contents, string controlNumber, int docID);
    }
}