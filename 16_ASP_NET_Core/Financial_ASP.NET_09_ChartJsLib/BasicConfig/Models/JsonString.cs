using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class JsonString
    {
        public static string GetGoogleJson(JsonColumn[] cols, JsonRow[] rows)
        {
            var gc = new GoogleChart();
            gc.cols = new JsonColumn[cols.Length];
            gc.rows = new JsonRow[rows.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                gc.cols[i] = cols[i];
            }
            for (int i = 0; i < rows.Length; i++)
            {
                gc.rows[i] = rows[i];
            }
            return JsonConvert.SerializeObject(gc);
        }
    }

    #region define json structure for google chart
    public class GoogleChart
    {
        public JsonColumn[] cols { get; set; }
        public JsonRow[] rows { get; set; }
    }

    public class JsonColumn
    {
        private string id1 = "";
        private string label1 = "";

        public string id
        {
            get { return id1; }
            set { id1 = value; }
        }
        public string label
        {
            get { return label1; }
            set { label1 = value; }
        }
        public string type { get; set; }
        public string role { get; set; }
    }

    public class JsonRow
    {
        public JsonCell[] c { get; set; }
    }

    public class JsonCell
    {
        public object v { get; set; }
        public string f { get; set; }
        public object p { get; set; }
    }
    #endregion define json struicture for google chart
}
