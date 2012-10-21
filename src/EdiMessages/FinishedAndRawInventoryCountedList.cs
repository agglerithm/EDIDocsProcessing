using System;
using System.Collections.Generic;

namespace EdiMessages
{
    [Serializable]
    public class FinishedAndRawInventoryCountedList : IEdiMessage
    {
        public string Location
        {
            get; set;
        }
        public IList<FinishedAndRawInventoryCounted> InventoryList
        {
            get; set;
        }

        public int DocumentId
        {
            get { return 846; }
        }

        public string ControlNumber
        {
            get;
            set;
        }

        public string BusinessPartnerCode
        {
            get;
            set;
        }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber
        {
            get;
            set;
        }


    }
}