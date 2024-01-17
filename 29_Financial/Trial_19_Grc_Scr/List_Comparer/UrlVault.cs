using System;

namespace List_Comparer
{
    internal class UrlVault
    {
        internal string GetBaseUrl()
        {
            return "https://www.global-rates.com/highchart-api/?minticks=1578268800000&maxticks=1801820800000&series[0].id=19&series[0].type=3&extra=null";
        }

        internal string GetResolution()
        {
            return "5";
        }

        internal Int64 GetToMaxInt()
        {
            return 1701820800000;
        }

        internal int GetToMinInt()
        {
            return 1643355300;
        }

        internal DateTime GetToMaxIntUtcTimeStampe()
        {
            return new DateTime(2023, 12, 6, 22, 00, 0);
        }
    }
}
