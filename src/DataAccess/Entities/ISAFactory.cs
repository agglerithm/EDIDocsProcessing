namespace EDIDocsOut.Common.DataAccess.Entities
{
    public interface IISAFactory
    {
        ISAEntity CreateISA(string functionalGroupID);
    }

    public class ISAFactory : IISAFactory
    {
        public ISAEntity CreateISA(string functionalGroupID)
        {
            var isa = new ISAEntity
                          {
                              Group = new GroupEntity() {GroupID = functionalGroupID}
                          };
            return isa;
        }
    }
}
