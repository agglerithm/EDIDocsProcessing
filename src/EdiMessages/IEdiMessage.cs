using AFPST.Common.Structures;

namespace EdiMessages
{
    public interface IEdiMessage
    {
        int DocumentId { get; }
        string ControlNumber { get; set; }
        string BusinessPartnerCode { get; set; }
        string BusinessPurpose { get;} 
        int BusinessPartnerNumber { get; set; }
    }

    public interface IEdiMessageWithAddress : IEdiMessage 
    {
        void AddAddress(Address address);
    }
}