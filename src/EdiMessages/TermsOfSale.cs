using System;

namespace EdiMessages
{
    [Serializable]
    public class TermsOfSale
    {
        public float DiscountPercent
        {
            get; set;
        }

        public int DiscountDays
        {
            get; set;
        }

        public int NetDays
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0}[{4}]{0} DiscountPercent: {1}{0} DiscountDays: {2}{0} NetDays: {3}{0}", Environment.NewLine, DiscountPercent, DiscountDays, NetDays,GetType().Name);
        }
    }
}