using System.Data;

namespace Sample
{
    internal class Program
    {
        //https://www.webdevtutor.net/blog/c-sharp-datarow-guide
        static void Main(string[] args)
        {
            //Assuming we have a DataTable named "employeesTable" with columns "Id", "Name", and "Age"
            DataTable employeesTable = new DataTable();
            employeesTable.Columns.Add("Id", typeof(int));
            employeesTable.Columns.Add("Col1", typeof(int));
            employeesTable.Columns.Add("Col2", typeof(int));
            employeesTable.Columns.Add("Res", typeof(int));

            //Create a new DataRow and assign values
            DataRow newRow1 = employeesTable.NewRow();
            newRow1["Id"] = 1;
            newRow1["Col1"] = 1;
            newRow1["Col2"] = 2;
            DataRow newRow2 = employeesTable.NewRow();
            newRow2["Id"] = 2;
            newRow2["Col1"] = 3;
            newRow2["Col2"] = 4;
            DataRow newRow3 = employeesTable.NewRow();
            newRow3["Id"] = 3;
            newRow3["Col1"] = 5;
            newRow3["Col2"] = 6;
            DataRow newRow4 = employeesTable.NewRow();
            newRow4["Id"] = 4;
            newRow4["Col1"] = 7;
            newRow4["Col2"] = 8;

            employeesTable.Rows.Add(newRow1);
            employeesTable.Rows.Add(newRow2);
            employeesTable.Rows.Add(newRow3);
            employeesTable.Rows.Add(newRow4);

            for (int i = 0; i < employeesTable.Rows.Count; i++)
            {
                var row = employeesTable.Rows[i];
                int col1 = row.Field<int>("Col1");
                int col2 = row.Field<int>("Col2");
                row["Res"] = col1 + col2;
            }

            employeesTable.AcceptChanges();

            PrintInConsole(employeesTable);
            ExportDataTableToFile(employeesTable, "|", true, "_exportedDataTable1.txt");
        }

        public static void ExportDataTableToFile(DataTable datatable, string delimited, bool exportcolumnsheader, string file)
        {
            StreamWriter str = new StreamWriter(file, false, System.Text.Encoding.Default);
            if (exportcolumnsheader)
            {
                string Columns = string.Empty;
                foreach (DataColumn column in datatable.Columns)
                {
                    Columns += column.ColumnName + delimited;
                }
                str.WriteLine(Columns.Remove(Columns.Length - 1, 1));
            }
            foreach (DataRow datarow in datatable.Rows)
            {
                string row = string.Empty;
                foreach (object items in datarow.ItemArray)
                {
                    row += items.ToString() + delimited;
                }
                str.WriteLine(row.Remove(row.Length - 1, 1));
            }
            str.Flush();
            str.Close();
        }

        private static void PrintInConsole(DataTable data)
        {
            Console.WriteLine();
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            foreach (DataColumn col in data.Columns)
            {
                Console.Write(col.ColumnName);
                var maxLabelSize = data.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 10; i++) Console.Write(" ");
            }

            Console.WriteLine();

            foreach (DataRow dataRow in data.Rows)
            {
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    Console.Write(dataRow.ItemArray[j]);
                    for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 10; i++) Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}