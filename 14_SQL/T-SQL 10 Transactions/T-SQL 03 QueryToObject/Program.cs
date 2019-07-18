using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T_SQL_03_QueryToObject.Models;

namespace T_SQL_03_QueryToObject
{
    class Program
    {
        //T1Template
        public void T1Query(string query)
        {
            string sql = query;
            var con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true");
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<T1>();
                    while (reader.Read())
                        list.Add(new T1 { keycol = reader.GetInt32(0), col1 = reader.GetInt32(1), col2 = reader.GetString(2)});
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.col1.ToString() + " " + item.col2);
                    }
                    Console.ReadKey();
                }
            }
        }
        static void Main(string[] args)
        {
            var p = new Program();
            //BEGIN TRAN
            p.T1Query(@"
USE TSQLV4;
BEGIN TRAN;
INSERT INTO dbo.T1(col1, col2) VALUES(101, 'C');
INSERT INTO dbo.T1(col1, col2) VALUES(201, 'X');
COMMIT TRAN;
SELECT col1, col2
FROM dbo.T1;
");
        }
    }
}
