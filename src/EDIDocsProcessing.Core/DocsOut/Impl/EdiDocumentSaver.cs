using AFPST.Common.Infrastructure;
using AFPST.Common.Services;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.Impl
{
    using Common;

    public class EdiDocumentSaver : IEdiDocumentSaver
    {
        private readonly IAFPSTConfiguration _config;
        private readonly IFileUtilities _fileUtilities;

        public EdiDocumentSaver(IAFPSTConfiguration config, IFileUtilities fileUtilities)
        {
            _config = config;
            _fileUtilities = fileUtilities;
        }

        public void Save(EDITransmissionPackage ediTransmissionPackage)
        {
            var ediXmlInterchangeControl = ediTransmissionPackage.GetInterchangeControl();
            var fname = GetFileName(ediTransmissionPackage);
            var workingName = _config.WorkingFolder() + fname;
            //string uploadName = ediTransmissionPackage.GetUploadFolder() + fname;
            var code = ediTransmissionPackage.GetBusinessPartner();
            var pathToUse = TransmissionPath.Edict;
            if (code == BusinessPartner.Initech)
                pathToUse = TransmissionPath.Initech;
            var uploadName = _config.GetUploadFolderFor(pathToUse) + fname;
            _fileUtilities.SaveTextAndRename(ediXmlInterchangeControl.Value, workingName, uploadName);
        }

        public void SaveAsXml(EDITransmissionPackage ediTransmissionPackage)
        {
            var ediXmlInterchangeControl = ediTransmissionPackage.GetInterchangeControl();
            var fname = GetFileName(ediTransmissionPackage) + ".xml";
            var workingName = _config.WorkingFolder() + fname;
            var uploadName = _config.ArchiveFolder() + fname;
            _fileUtilities.SaveTextAndRename(ediXmlInterchangeControl.ToString(), workingName, uploadName);
        }


        private static string GetFileName(EDITransmissionPackage package)
        {
            var control = package.GetInterchangeControl();
            var partner = package.GetBusinessPartner();
            return   string.Format("{0}{1}{2}.out", control.FunctionalID,
                control.ControlNumber, partner.Code);
        }
    }
}