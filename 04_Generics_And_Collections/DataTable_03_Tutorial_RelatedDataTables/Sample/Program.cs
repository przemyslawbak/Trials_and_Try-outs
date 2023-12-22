using System.Data;

namespace Sample
{
    internal class Program
    {
        //https://www.webdevtutor.net/blog/c-sharp-datarow-guide
        static void Main(string[] args)
        {
            //Creating new customer table
            DataTable customerTable = new DataTable();
            customerTable.Columns.Add("CustomerId", typeof(int));
            customerTable.Columns.Add("CustomerName", typeof(string));
            customerTable.Columns.Add("Age", typeof(int));

            //Create a new DataRow -s and assign values
            DataRow newRow1 = customerTable.NewRow();
            newRow1["CustomerId"] = 1;
            newRow1["CustomerName"] = "John Doe";
            newRow1["Age"] = 30;
            DataRow newRow2 = customerTable.NewRow();
            newRow2["CustomerId"] = 2;
            newRow2["CustomerName"] = "Joana Doe";
            newRow2["Age"] = 69;

            customerTable.Rows.Add(newRow1);
            customerTable.Rows.Add(newRow2);

            //Creating new orders table
            DataTable ordersTable = new DataTable("Orders");
            ordersTable.Columns.Add("OrderId", typeof(int));
            ordersTable.Columns.Add("CustomerId", typeof(int));
            ordersTable.Columns.Add("ProductName", typeof(string));

            //Create a new DataRow -s and assign values
            DataRow newRow3 = ordersTable.NewRow();
            newRow3["OrderId"] = 1;
            newRow3["CustomerId"] = 1;
            newRow3["ProductName"] = "Cool stuff";
            DataRow newRow4 = ordersTable.NewRow();
            newRow4["OrderId"] = 2;
            newRow4["CustomerId"] = 1;
            newRow4["ProductName"] = "Cool stuff";

            ordersTable.Rows.Add(newRow3);
            ordersTable.Rows.Add(newRow4);

            DataColumn customerKeyColumn = customerTable.Columns["CustomerID"];
            DataColumn orderKeyColumn = ordersTable.Columns["CustomerID"];
            DataRelation relation = new DataRelation("CustomerOrderRelation", customerKeyColumn, orderKeyColumn); //they need to be in DataSet

            var parentRow = customerTable.Rows[0];
            DataRow[] childRows = parentRow.GetChildRows(relation);
            var count = childRows.Count();

            PrintInConsole(customerTable);
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