using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;

namespace Financial.Models
{
    public static class ModelHelper
    {
        public static double[] GetAutoCorrelation(double[] x)
        {
            int half = (int)x.Length / 2;
            double[] autoCorrelation = new double[half];
            double[] a = new double[half];
            double[] b = new double[half];
            for (int i = 0; i < half; i++)
            {
                a[i] = x[i];
                b[i] = x[i + i];
                autoCorrelation[i] = GetCorrelation(a, b);
            }
            return autoCorrelation;
        }

        public static double GetCorrelation(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                throw new Exception("Length of sources is different");
            double varx = x.Variance();
            double vary = y.Variance();
            double cov = x.Covariance(y);
            return cov / Math.Sqrt(varx * vary);
        }


        public static double[] GetAvgStd(List<double> xList)
        {
            double[] xa = xList.ToArray();
            return GetAvgStd(xa);
        }

        public static double[] GetAvgStd(double[] xa)
        {
            //avg:
            double avg = xa.Average();
            //std
            double std = 0;
            for (int i = 0; i < xa.Length; i++)
            {
                std += (xa[i] - avg) * (xa[i] - avg);
            }
            std = Math.Sqrt(std / (xa.Length - 1));

            return new double[] { avg, std };
        }

        public static double[] ArrayIntToDouble(int[] array)
        {
            double[] res = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                res[i] = (double)array[i];
            return res;
        }

        public static double[] DataTableToArray(DataTable dt, string col)
        {
            double[] result = new double[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][col] != DBNull.Value)
                    result[i] = Convert.ToDouble(dt.Rows[i][col]);
            }
            return result;
        }

        public static List<double> DataTableToList(DataTable dt, string col)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][col] != DBNull.Value)
                    result.Add(Convert.ToDouble(dt.Rows[i][col]));
            }
            return result;
        }


        public static void DatatableToCsv(DataTable dt, string csvFilePath)
        {
            // Create the CSV file to which Datatable data will be exported.
            StreamWriter sw = new StreamWriter(csvFilePath, false);
            int colCount = dt.Columns.Count;

            // First write the headers:
            for (int i = 0; i < colCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < colCount - 1)
                {
                    sw.Write(',');
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < colCount - 1)
                    {
                        sw.Write(',');
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }


        public static DataTable CsvToDatatable(string cdsFile)
        {
            FileInfo fi = new FileInfo(cdsFile);
            // retrives directory
            string dirCSV = fi.DirectoryName.ToString();
            // retrives file name with extension
            string FileNevCSV = fi.Name.ToString();

            // Write the schema file
            WriteSchema(dirCSV, FileNevCSV);

            DataSet ds = new DataSet();
            try
            {
                // Creates and opens an ODBC connection
                string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" +
                    dirCSV.Trim() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                string sql_select;
                OdbcConnection conn;
                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();

                sql_select = "select * from [" + FileNevCSV.Trim() + "]";

                //Creates the data adapter
                OdbcDataAdapter da = new OdbcDataAdapter(sql_select, conn);

                da.Fill(ds, "csv");
                conn.Close();
            }
            catch { }
            DataTable res = new DataTable();
            if (ds.Tables.Count > 0)
                res = ds.Tables[0];
            return res;
        }



        private static void WriteSchema(string dirCSV, string FileNevCSV)
        {
            try
            {
                FileStream fsOutput = new FileStream(dirCSV + "\\schema.ini", FileMode.Create, FileAccess.Write);
                StreamWriter srOutput = new StreamWriter(fsOutput);
                string s1, s2, s3, s4, s5;

                s1 = "[" + FileNevCSV + "]";
                s2 = "ColNameHeader=" + "True";
                s3 = "Format=Delimited(,)";
                s4 = "MaxScanRows=25";
                s5 = "CharacterSet=ANSI";
                srOutput.WriteLine(s1.ToString() + "\r\n" + s2.ToString() + "\r\n" + s3.ToString() + "\r\n" + s4.ToString() + "\r\n" + s5.ToString());
                srOutput.Close();
                fsOutput.Close();
            }
            catch { }
        }

        public static DataTable DatatableSort(DataTable dt, string sortString)
        {
            DataView v = dt.DefaultView;
            v.Sort = sortString;
            return v.ToTable();
        }

        public static DataTable CopyTable(DataTable dt)
        {
            DataTable res = new DataTable();
            res = dt.Clone();
            foreach (DataRow row in dt.Rows)
                res.ImportRow(row);
            return res;
        }


        public static DateTime fst_day_of_month(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime last_day_of_month(DateTime date)
        {
            return (new DateTime(date.Year, date.Month, 1)).AddMonths(1).AddDays(-1);
        }

        public static DateTime last_workday_of_month(DateTime date)
        {
            return get_previous_workday((new DateTime(date.Year, date.Month, 1)).AddMonths(1));
        }

        public static DateTime first_workday_of_month(DateTime date)
        {
            return get_next_workday((new DateTime(date.Year, date.Month, 1)).AddDays(-1));
        }

        public static DateTime get_next_workday(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            }
            while (date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday);

            return date;
        }


        public static DateTime get_previous_workday(DateTime date)
        {
            do
            {
                date = date.AddDays(-1);
            }
            while (date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday);

            return date;
        }

        public static int get_number_calendar_days(DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate;
            int num = 0;
            while (date <= endDate)
            {
                date = date.AddDays(1);
                num++;
            }
            return num - 1;
        }

        public static int get_number_workdays(DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate;
            int num = 0;
            while (date <= endDate)
            {
                date = get_next_workday(date);
                num++;
            }
            return num;
        }


        public static T To<T>(this object text)
        {
            if (text == null) return default(T);
            if (text.Equals(DBNull.Value)) return default(T);
            if (text is string) if (string.IsNullOrWhiteSpace(text as string)) return default(T);

            var type = typeof(T);
            if (type.ToString() == "QuantLib.Date")
            {
                var dt = (DateTime)text;
                QuantLib.Date date = new QuantLib.Date((int)dt.ToOADate());
                return (T)Convert.ChangeType(date, type);
            }

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return (T)Convert.ChangeType(text, underlyingType);
        }

        public static T ToDatetime<T>(this QuantLib.Date date)
        {
            DateTime dt = Convert.ToDateTime(date.month() + " " + date.dayOfMonth().ToString() + ", " + date.year().ToString());
            var type = typeof(T);
            if (type == typeof(string))
                return (T)Convert.ChangeType(dt.ToShortDateString(), typeof(T));
            else
                return (T)Convert.ChangeType(dt, typeof(T));
        }





        public static double StdDev<T>(this IEnumerable<T> list, Func<T, double> values)
        {
            var mean = 0.0;
            var stdDev = 0.0;
            var n = 0;

            n = 0;
            foreach (var value in list.Select(values))
            {
                n++;
                mean += value;
            }
            mean /= n;

            foreach (var value in list.Select(values))
            {
                stdDev += (value - mean) * (value - mean);
            }
            stdDev = Math.Sqrt(stdDev / (n - 1));
            return stdDev;
        }

    }
}
