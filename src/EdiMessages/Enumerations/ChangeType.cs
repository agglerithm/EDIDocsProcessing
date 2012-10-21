using AFPST.Common.Enumerations;

namespace EdiMessages.Enumerations
{
    public class ChangeType : EnumerationOfInteger
    {
        public static ChangeType AddItems = new ChangeType(0, "AI");
        public static ChangeType ChangeItems = new ChangeType(1, "CA");
        public static ChangeType DeleteItems = new ChangeType(2, "DI");
        public static ChangeType NoChange = new ChangeType(3, "NC");
        public static ChangeType PriceChange = new ChangeType(4, "PC");
        public static ChangeType PriceAndQuantityChange = new ChangeType(5, "PQ");
        public static ChangeType QuantityDecrease = new ChangeType(6, "QD");
        public static ChangeType QuantityIncrease = new ChangeType(7, "QI");

        private ChangeType(int value, string display):base(value, display)
        { 
        }

        public ChangeType()
        {
            
        }
    }
}
