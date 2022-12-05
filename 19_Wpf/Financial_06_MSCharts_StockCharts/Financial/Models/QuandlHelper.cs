using QuandlCS.Requests;
using QuandlCS.Types;
using System;
using System.Data;
using System.IO;
using System.Net;

namespace Financial.Models
{
    public static class QuandlHelper
    {
        private const string quandlKey = "gp_z7rn26KEP3uJFuuiw";

        public static DataTable GetQuandlData(string ticker, string dataSource, DateTime startDate, DateTime endDate)
        {
            QuandlDownloadRequest request = new QuandlDownloadRequest();
            request.APIKey = quandlKey;
            request.Datacode = new Datacode(dataSource, ticker);
            request.Format = FileFormats.CSV;
            request.Frequency = Frequencies.Daily;
            request.StartDate = startDate;
            request.EndDate = endDate;
            request.Sort = SortOrders.Ascending;

            string ss = request.ToRequestString().Replace("/v1/", "/v3/");

            DataTable dt = new DataTable();
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(ss, "my.csv");
                    dt = ModelHelper.CsvToDatatable("my.csv");
                    File.Delete("my.csv");
                }
                catch { }
            }
            return dt;
        }
    }
}
