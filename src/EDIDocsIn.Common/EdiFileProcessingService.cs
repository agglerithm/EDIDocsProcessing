using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AFPST.Common.IO;
using AFPST.Common.Services;
using AFPST.Common.Services.Logging;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;

namespace EDIDocsProcessing.Common
{
    using EmailService.Messages;

    public class EdiFileProcessingService  : IEdiFileProcessingService 
    {
        private readonly IFileUtilities _fileUtil;
        private readonly IAssignDocumentsToPartners _assigner;
        private readonly INotificationSender _notificationSender;


        public EdiFileProcessingService(IFileUtilities fileUtil, IAssignDocumentsToPartners assigner, INotificationSender notificationSender)
        {
            _fileUtil = fileUtil;
            _notificationSender = notificationSender;
            _assigner = assigner;
        }


        //public List<CreateOrderMessage> CustomerOrders { get; private set; }

        public IEnumerable<IEdiMessage> ProcessFiles()
        { 

            return null;
        }

        public IEnumerable<IEdiMessage> ProcessFiles(TransmissionPath path)
        {
            var downloadPath = EdiConfig.GetDownloadFolderFor(path);
            IList<FileEntity> files = _fileUtil.GetListFromFolder(downloadPath, "*.*",
                                                      DateTime.Today.AddYears(-2));
            var assignments = _assigner.MakeAssignments(files);
            var documents = new List<IEdiMessage>();
             
             

            foreach(var assignment in assignments)
            {
                documents.AddRange(processPartner(assignment));
            }


            return documents;
        }

        private IEnumerable<IEdiMessage> processPartner(DocumentAssignments assignment)
        {
            var fileParser = assignment.Partner.Parser();
            var documents = new List<IEdiMessage>();
            var segList = assignment.Documents;
            foreach (var doc in segList)
            {
                var segments = doc.Segments;
                var file = doc.File;
                try
                {
                    var docs = fileParser.Parse(segments);
                    if (docs.Count() > 0)
                    {
                        documents.AddRange(docs.Where(d => d != null));
                        moveFileToArchive(file);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(this, "Error processing file ", e);
                    _notificationSender.SendNotification("Failure processing incoming file", getRecipientList(),
                                                         string.Format("The file {0} could not be processed.  The file is being moved to the Error folder.  Error message: {1}", doc.File, e));
                    moveFileToErrorFolder(file);
                }
            }
            return documents;
        }

        private IEnumerable<EmailAddress> getRecipientList()
        {
            return new List<EmailAddress>()
                             {
                                 new EmailAddress()
                                     {Address = EmailAddressConstants.AccountsReceivableEmailAddress , DisplayName = "A/R"},
                                 new EmailAddress() {Address = "ITIS@austinfoam.com", DisplayName = "IT/IS"}
                             };
        }


        private void moveFileToErrorFolder(FileEntity file)
        {
            if (!Directory.Exists(file.ContainingFolder + @"\Error\"))
                Directory.CreateDirectory(file.ContainingFolder + @"\Error\");
            var destPath = file.ContainingFolder + @"\Error\" + file.FileName;
            _fileUtil.MoveFileWithOverwrite(file.FullPath, destPath);
            File.Move(destPath, getNewPath(file, @"\Error\"));
        }

        private void moveFileToArchive(FileEntity file)
        {
            if (!Directory.Exists(file.ContainingFolder + @"\Archive\"))
                Directory.CreateDirectory(file.ContainingFolder + @"\Archive\");
            var destPath = file.ContainingFolder + @"\Archive\" + file.FileName;
            _fileUtil.MoveFileWithOverwrite(file.FullPath, destPath);
            File.Move(destPath, getNewPath(file, @"\Archive\"));
        }

        private static string getNewPath(FileEntity file, string folder)
        {
            return file.ContainingFolder + folder + Path.GetFileNameWithoutExtension(file.FileName) +
                   DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(file.FileName);
        }
    }
}