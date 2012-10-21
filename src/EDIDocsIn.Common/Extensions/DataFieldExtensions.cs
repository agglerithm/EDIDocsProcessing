using System;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class DataFieldExtensions
    {

        public static int CastToInt(this object obj)
        {
            return obj.ToString().CastToInt();
        }

        public static double CastToDouble(this object obj)
        {
            return obj.ToString().CastToDouble();
        }

        public static bool CastToBool(this object obj)
        {
            return obj.ToString().CastToBool();
        }

        public static DateTime CastToDateTime(this object obj)
        {
            return obj.ToString().CastToDateTime();
        }
    }
}
