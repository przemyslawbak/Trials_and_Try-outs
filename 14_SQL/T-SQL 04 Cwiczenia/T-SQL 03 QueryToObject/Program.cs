using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T_SQL_03_QueryToObject.Models;

namespace T_SQL_03_QueryToObject
{
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

            //1. zwraca zatrudnionych w 2002 roku
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE START_DATE >= '20020101' AND START_DATE < '20030101';
");
            //2. zwraca zatrudnionych w ostatnim dniu miesiąca
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE START_DATE = EOMONTH(START_DATE);
");
            //3. zwraca zatrudnionych z 2x 'a' w nazwisku
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE LAST_NAME LIKE N'%a%a%';
");
            /*
            //4. zwracające zamówienia o łącznej wartości (ilość * cena jednostkowa) przekraczającej 10000, posortowane według łącznej wartości.
            p.EmployeeQuery(@"
USE TSQLV4;
SELECT orderid, SUM(qty*unitprice) AS totalvalue
FROM Sales.OrderDetails
GROUP BY orderid
HAVING SUM(qty*unitprice) > 10000
ORDER BY totalvalue DESC;
");
*/
            //5. zwraca z nazwiskami z małej litery
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE LAST_NAME COLLATE Latin1_General_CS_AS LIKE N'[abcdefghijklmnopqrstuvwxyz]%';
");
            /*
            //7. zwracające trzy kraje, do których wysyłka produktów miała najwyższą średnią wartość w roku 2015.
                        p.EmployeeQuery(@"
USE TSQLV4;
SELECT TOP (3) shipcountry, AVG(freight) AS avgfreight
FROM Sales.Orders
WHERE orderdate >= '20150101' AND orderdate < '20160101'
GROUP BY shipcountry
ORDER BY avgfreight DESC;
");
*/
            //8. oblicza ilość wierszy dla pracownika, po ilości działów
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, EMP_ID, START_DATE, DEPT_ID,
ROW_NUMBER() OVER(PARTITION BY EMP_ID ORDER BY START_DATE, DEPT_ID) AS rownum
FROM EMPLOYEE
ORDER BY EMP_ID, rownum;
");
        }
    }
}
