using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Common.IO;

namespace FedexEDIDocsOut
{
    public class FedexFileCreationService:IFileCreationService
    {
        public void SendFile(string contents, string controlNumber, int docID)
        {
            var fileName = BusinessPartner.FedEx.Code + docID + controlNumber + ".edi";
            string fedexUploadPath = _ediConfiguration.UploadFolder;
            string fedexWorkingPath = _ediConfiguration.WorkingFolder;
            _fileUtil.SaveTextAndRename(contents,fedexWorkingPath + fileName, fedexUploadPath + fileName);
            
        }

        private readonly IFileUtilities _fileUtil;
        private readonly IEdiConfiguration _ediConfiguration;  


        public FedexFileCreationService(IFileUtilities fileUtil,
                                        IEdiConfiguration ediConfiguration )
        {
            _fileUtil = fileUtil;
            _ediConfiguration = ediConfiguration; 
        }
 
    }
}