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
            employeesTable.Columns.Add("Name", typeof(string));
            employeesTable.Columns.Add("Age", typeof(int));

            //Create a new DataRow and assign values
            DataRow newRow1 = employeesTable.NewRow();
            newRow1["Id"] = 1;
            newRow1["Name"] = "John Doe";
            newRow1["Age"] = 30;
            DataRow newRow2 = employeesTable.NewRow();
            newRow2["Id"] = 2;
            newRow2["Name"] = "Joana Doe";
            newRow2["Age"] = 69;

            employeesTable.Rows.Add(newRow1);
            employeesTable.Rows.Add(newRow2);

            //Accessing Data by Column Name
            var row = employeesTable.Rows[0];
            int id = row.Field<int>("Id");
            string name = row.Field<string>("Name");

            //Accessing Data by Column Index
            id = (int)row[0];  // Assuming Id is the first column
            name = (string)row[1];  // Assuming Name is the second column

            //Updating Data in a DataRow
            row["Name"] = "Joana Doe";
            employeesTable.AcceptChanges();

            //Deleting Data from a DataRow
            row.Delete();
            employeesTable.AcceptChanges();

            PrintInConsole(employeesTable);
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