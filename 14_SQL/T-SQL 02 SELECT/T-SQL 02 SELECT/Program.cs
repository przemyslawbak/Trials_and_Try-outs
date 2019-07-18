using System;
using System.Data.SqlClient;

namespace T_SQL_02_SELECT
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                /*
                //Przykład zapytania
                command.CommandText = @"
                        USE dbApi;
SELECT empid, YEAR(orderdate) AS orderyear, COUNT(*) AS numorders
FROM Sales.Orders
WHERE custid = 71
GROUP BY empid, YEAR(orderdate)
HAVING COUNT(*) > 1
ORDER BY empid, orderyear;
";
                command.ExecuteNonQuery();
                */
                connection.Close();
            }
        }
    }
}
