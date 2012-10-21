using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIDocsOut.Common.Entities
{
    public interface IEdiMessage
    {
        int DocumentID { get; }
        string ControlNumber { get; set; }

        void AddAddress(Address address);
    }
}
