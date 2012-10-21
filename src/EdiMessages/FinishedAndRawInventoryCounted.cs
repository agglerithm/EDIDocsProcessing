using System;
using System.Linq;
using System.Text;

namespace EdiMessages
{
    [Serializable]
    public class FinishedAndRawInventoryCounted
    {
        public string CustomerPartNumber
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string UserDefinedIdentifier
        {
            get; set;
        }

        public int AvailableFinishedQuantity
        {
            get; set;
        }

        public int AvailableRawQuantity
        {
            get; set;
        }


    }
}