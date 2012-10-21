using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFPST.Common.Infrastructure;
using Castle.Windsor;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class ContainerExtensions
    {
        public static IFileParser GetFileParserFor(this IWindsorContainer container, BusinessPartner partner)
        {
            return container.Resolve<IFileParser>(partner.Code + "FileParser");
        }
    }
}
