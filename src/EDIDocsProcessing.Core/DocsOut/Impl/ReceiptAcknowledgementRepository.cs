namespace EDIDocsProcessing.Core.DocsOut.Impl
{
    using System;
    using System.Linq;
    using AFPST.Common.Extensions;
    using Common.DTOs;
    using DataAccess;
    using DocsIn.impl;
    using EdiMessages;
    using FluentNHibernate;

    public class ReceiptAcknowledgementRepository : Repository, IReceiptAcknowledgementRepository
    {
        public ReceiptAcknowledgementRepository(ISessionSource source) : base(source)
        {
        }

        public ReceiptAcknowledgement GetSentDocument(int controlNum)
        { 
            var lst = Query<ReceiptAcknowledgement>(ack => ack.DocumentControlNumber == controlNum);
            return lst.Count() == 0 ? null : lst.First();
        }

        public void SetPlaceholder(ReceiptAcknowledgement ack)
        { 
            if(ack.ID != Guid.Empty)
                throw new EDIWorkflowException(string.Format("ACK placeholder already exists for control number {0}",ack.DocumentControlNumber));
            base.Save(ack);
        }

 

        public void SaveAcks(Acknowledgements acks)
        {
            acks.Acks.Where(a => a != null).ForEach(a =>
                                  { 
                                      base.Save(getAck(a, acks));
                                  });
        }

        private ReceiptAcknowledgement getAck(ReceiptAcknowledgementMsg ackMsg, Acknowledgements acks)
        {
            var ack = GetSentDocument(ackMsg.ControlNumber.CastToInt());  
                                      if(ack == null)
                                          throw new EDIWorkflowException(
                                              string.Format(
                                                  "The ACK placeholder for control number {0} was not found!",
                                                  ackMsg.ControlNumber));
            ack.AckReceiveDate = DateTime.Now;
            ack.AckControlNumber = acks.ControlNumber.CastToInt();

            return ack;
        }
    }
}