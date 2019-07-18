using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T_SQL_03_QueryToObject.Models;

namespace T_SQL_03_QueryToObject
{
    /// <summary>
    /// WSZYSTKIE PRZYKŁADY DLA BAZY TSQLV4, ćwiczenia też dla niej
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
                        list.Add(new Employee { FIRST_NAME = reader.GetString(0), LAST_NAME = reader.GetString(1), START_DATE = reader.GetDateTime(2)});
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

            /*
            //Zwykłe złącze krzyżowe między 2(!) bazami <-- CROSS JOIN
            p.EmployeeQuery(@"
USE TSQLV4;
SELECT C.custid, E.empid
FROM Sales.Customers AS C
CROSS JOIN HR.Employees AS E;
");
*/
            /*
                        //Samo złącze dla 1(!) bazy
                        p.EmployeeQuery(@"
            USE TSQLV4;
            SELECT
E1.empid, E1.firstname, E1.lastname,
E2.empid, E2.firstname, E2.lastname
FROM HR.Employees AS E1
CROSS JOIN HR.Employees AS E2;
            ");
            */
            /*
                        //Złączenie wewnętrzne <-- ON
                        p.EmployeeQuery(@"
            USE TSQLV4;
SELECT E.empid, E.firstname, E.lastname, O.orderid
FROM HR.Employees AS E
JOIN Sales.Orders AS O
ON E.empid = O.empid;
            ");
            */
            /*
                       //Złączenie złożone
                       p.EmployeeQuery(@"
           FROM dbo.Table1 AS T1
JOIN dbo.Table2 AS T2
ON T1.col1 = T2.col1
AND T1.col2 = T2.col2
           ");
           */
            /*
                        //Złączenie wielokrotne
                        p.EmployeeQuery(@"
            USE TSQLV4;
            SELECT
C.custid, C.companyname, O.orderid,
OD.productid, OD.qty
FROM Sales.Customers AS C
JOIN Sales.Orders AS O
ON C.custid = O.custid
JOIN Sales.OrderDetails AS OD
ON O.orderid = OD.orderid;
            ");
            */
            /*
                        //Złączenie zewnętrzne <-- LEFT
                        p.EmployeeQuery(@"
            USE TSQLV4;
            SELECT C.custid, C.companyname
FROM Sales.Customers AS C
LEFT OUTER JOIN Sales.Orders AS O
ON C.custid = O.custid
WHERE O.orderid IS NULL; <-- dla klientów którzy nie złożyli zamówienia
            ");
            */
        }
    }
}
