using System.Configuration;
using AFPST.Common.Data;
using AFPST.Common.Infrastructure.impl;
using AFPST.Common.Services;
using AFPST.Common.Services.imp;
using Castle.Core;

namespace AFPST.Common.Infrastructure
{
    public class CommonContainer
    {
        public static void SetUp()
        {
            bool testMode = ConfigurationManager.AppSettings["TestMode"] != "0";
            var connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            if (!testMode)
                connectionString = ConfigurationManager.ConnectionStrings["Production"].ConnectionString; 
            Container.Register<IAFPSTConfiguration, AFPSTConfiguration>("config",LifestyleType.Transient); 
            Container.Register<ISQLWrapper, SQLWrapper>("sql").WithDependencies(new
            {
               connectionstring = connectionString
            });
            Container.Register<IDataUtilities, DataUtilities>("dataUtil").WithDependencies(
                new { test = testMode }); 
            Container.Register<IEmailUtilities, EmailUtilities>("emailutilities", LifestyleType.Singleton);
            Container.Register<IERPContextRepository, ERPContextRepository>();
        }
    }
}
