using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T_SQL_03_QueryToObject.Models;

namespace T_SQL_03_QueryToObject
{
    /// <summary>
    /// WSZYSTKIE PRZYKŁADY DLA BAZY Training
    /// </summary>
    class Program
    {
        //customerTemplate
        public void CustomerQuery(string query)
        {
            string sql = query;
            var con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true");
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Customer>();
                    while (reader.Read())
                        list.Add(new Customer { ADDRESS = reader.GetString(0), CITY = reader.GetString(1) });
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.ADDRESS + " " + item.CITY);
                    }
                    Console.ReadKey();
                }
            }
        }
        //employeeTemplate
        public void EmployeeQuery(string query)
        {
            string sql = query;
            var con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true");
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Employee>();
                    while (reader.Read())
                        list.Add(new Employee { FIRST_NAME = reader.GetString(0), LAST_NAME = reader.GetString(1)});
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.FIRST_NAME + " " + item.LAST_NAME + " data:" + item.START_DATE);
                    }
                    Console.ReadKey();
                }
            }
        }
        static void Main(string[] args)
        {
            var p = new Program();
            //UNION ALL - połączenie (bez ALL zwraca bez duplikatów)
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME FROM INDIVIDUAL
UNION ALL
SELECT FIRST_NAME, LAST_NAME FROM EMPLOYEE
");
            //INTERSECT - część wspólna zbioru
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME FROM INDIVIDUAL
INTERSECT
SELECT FIRST_NAME, LAST_NAME FROM EMPLOYEE
");
            //EXCEPT - zwraca elementy pierwszego zbioru, które nie występują w drugim
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME FROM INDIVIDUAL
EXCEPT
SELECT FIRST_NAME, LAST_NAME FROM EMPLOYEE
");
        }
    }
}
