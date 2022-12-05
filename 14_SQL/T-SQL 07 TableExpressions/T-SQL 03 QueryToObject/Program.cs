using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T_SQL_03_QueryToObject.Models;

namespace T_SQL_03_QueryToObject
{
    /// <summary>
    /// PRZYKŁADY DLA BAZY TSQLV4
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
            //przykład wyrażenia tablicowego
            p.EmployeeQuery(@"
USE Training;
SELECT startyear, COUNT(DISTINCT DEPT_ID) AS numdept
FROM (SELECT YEAR(START_DATE) AS startyear, DEPT_ID
FROM EMPLOYEE) AS D
GROUP BY startyear;
");
            //przykład wyrażenia tablicowego ze zmienną lokalną
            p.EmployeeQuery(@"
USE TSQLV4;
DECLARE @empid AS INT = 3;
SELECT orderyear, COUNT(DISTINCT custid) AS numcusts
FROM (SELECT YEAR(orderdate) AS orderyear, custid
FROM Sales.Orders
WHERE empid = @empid) AS D
GROUP BY orderyear;
");
            //zapytanie z zagnieżdżonymi tabelami pochodnymi
            p.EmployeeQuery(@"
USE TSQLV4;
SELECT orderyear, numcusts
FROM (SELECT orderyear, COUNT(DISTINCT custid) AS numcusts
FROM (SELECT YEAR(orderdate) AS orderyear, custid
FROM Sales.Orders) AS D1
GROUP BY orderyear) AS D2
WHERE numcusts > 70;
");
            */
            //wyrażenie CTE, można też stosować argumenty
            p.EmployeeQuery(@"
USE Training;
WITH TellerRole AS
(
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE TITLE = N'Teller'
)
SELECT * FROM TellerRole;
");
            /*
            //tworzenie widoku
            p.EmployeeQuery(@"
USE Training;
DROP VIEW IF EXISTS TellerEmpList;
GO
CREATE VIEW TellerEmpList
AS
SELECT
FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE TITLE = N'Teller';
GO
");
*/
            /*
                        //TVF funkcje - przykład
                        p.EmployeeQuery(@"
            USE TSQLV4;
DROP FUNCTION IF EXIST dbo.GetCustOrders;
GO
CREATE FUNCTION dbo.GetCustOrders
(@cid AS INT) RETURNS TABLE
AS
RETURN
SELECT orderid, custid, empid, orderdate, requireddate,
shippeddate, shipperid, freight, shipname, shipaddress, shipcity,
shipregion, shippostalcode, shipcountry
FROM Sales.Orders
WHERE custid = @cid;
GO
            ");
            */
        }
    }
}
