using System;
using System.Reflection;
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Core.DocsOut; 
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace EDIDocsProcessing.Core.DataAccess
{
    public class SessionBuilder  
    {
        private static bool _BuildSchema;
        public static void SetUpRepository(string connectionKey, bool buildSchema)
        { 
            _BuildSchema = buildSchema;
            Container.Register<IControlNumberRepository, ControlNumberRepository>("repo").WithDependencies(
                new  { session = openSession(connectionKey) });
        }

        private static ISession openSession(string connectionKey)
        {
            FluentConfiguration configuration = Fluently.Configure();
            var sess = configuration
                .Database(MsSqlConfiguration
                              .MsSql2005.ConnectionString(c => c.FromAppSetting(
                                                                   connectionKey)))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("EDIDocsProcessing.Core")))
                .ExposeConfiguration(build_schema) 
                .BuildSessionFactory().OpenSession();
            return sess;
        }

        private static void build_schema(Configuration obj)
        {
            if(!_BuildSchema) return;
                new SchemaExport(obj).Create(false, true);
        }

        public static ISession GetRepository(string key, bool schema)
        {
            throw new NotImplementedException();
        }
    }
}