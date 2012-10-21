using System;
using FluentNHibernate;
using NHibernate;

namespace EDIDocsProcessing.Core.DataAccess
{
    public class EdiSessionSource:  SessionSource
    {
        public EdiSessionSource(string connectionKey, bool createnewTables)
            : base(FluentNHibernateConfigurationBuilder
                .GetFluentNHibernateConfiguration(connectionKey, createnewTables)) { }

        public new ISession CreateSession()
        {
            return base.CreateSession();
        }

        public new void BuildSchema()
        {
            throw new NotImplementedException();
            //base.BuildSchema();
        }
    }
}