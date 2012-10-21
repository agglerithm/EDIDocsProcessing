using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
 

namespace EDIDocsProcessing.Core.DataAccess
{
    public static class FluentNHibernateConfigurationBuilder  
    {

        public static FluentConfiguration GetFluentNHibernateConfiguration(string connectionKey, bool createNewTables)
        {
           return Fluently.Configure()
                .Database(MsSqlConfiguration
                .MsSql2005.ConnectionString(c => c.FromAppSetting(connectionKey)))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("EDIDocsProcessing.Core")))
                .ProxyFactoryFactory("NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.Linfu")
//                .ExposeConfiguration(cfg => { 
                                               // if (createNewTables == false) return;
//                                                  new SchemaExport(cfg).Create(true, true); 
//                                         })
                                          ; 

        }
    }
}