namespace EDIDocsProcessing.Common
{
    public interface IBusinessPartnerSpecificServiceResolver
    {
        ISegmentFactory GetSegmentFactoryFor(BusinessPartner partner);

        //IFileCreationService GetFileCreationServiceFor(BusinessPartner partner);

        IBuildValueFactory GetBuildValueFactoryFor(BusinessPartner partner);

        IFileParser GetFileParserFor(BusinessPartner partner);

        bool TestMode
        { get; }

        IDocumentParser GetDocumentParserFor(BusinessPartner Initech, string docType);
        IFileParser GetFileParserBySenderId(string senderId);
    }
}