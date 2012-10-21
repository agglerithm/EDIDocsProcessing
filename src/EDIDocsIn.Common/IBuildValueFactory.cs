using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface IBuildValueFactory
    {
        EdiXmlBuildValues GetValues();
        bool For(BusinessPartner partner);
    }
}