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

            //TOP - ilość zwracanych wierszy (lub procent)
            p.CustomerQuery(@"
USE Training;
SELECT TOP(5) ADDRESS, CITY
FROM CUSTOMER;
");
            //WHERE - filtrowanie
            p.CustomerQuery(@"
USE Training;
SELECT TOP(5) ADDRESS, CITY
FROM CUSTOMER
WHERE CUST_ID BETWEEN 5 AND 10");
            //LIKE - zawiera wzorzec
            p.CustomerQuery(@"
USE Training;
SELECT TOP(5) ADDRESS,CITY
FROM CUSTOMER
WHERE FED_ID LIKE N'04%'
");
            /*
            //CASE - przypadki, wrzuca do wyniku zapytania
            p.Query(@"USE Training;
SELECT ASSIGNED_BRANCH_ID,
CASE
WHEN ASSIGNED_BRANCH_ID = 1 THEN 'dzial pierwszy'
WHEN ASSIGNED_BRANCH_ID = 2 THEN 'dzial drugi'
WHEN ASSIGNED_BRANCH_ID = 3 THEN 'dzial trzeci'
WHEN ASSIGNED_BRANCH_ID = 4 THEN 'dzial czwarty'
ELSE 'Unknown'
END AS NAZWA_DZIALU
FROM dbo.EMPLOYEE;");
*/
//dla CUST_TYPE I
            p.CustomerQuery(@"
USE Training;
SELECT ADDRESS, CITY
FROM CUSTOMER
WHERE CUST_TYPE_CD LIKE N'I';
");
            //dla NULL
            p.CustomerQuery(@"
USE Training;
SELECT ADDRESS, CITY
FROM CUSTOMER
WHERE STATE IS NULL;
");
            //COLLATE - uściśla opcje sortowania
            p.CustomerQuery(@"
USE Training;
SELECT ADDRESS,CITY
FROM CUSTOMER
WHERE CITY COLLATE Latin1_General_CS_AS = N'Wilmington';
");
            /*
            //+ (łączenie), wrzuca do wyniku zapytania; COALESCE - zastępuje NULL jakimś znakiem
            p.Query(@"
USE Training;
SELECT ADDRESS + N', ' + COALESCE(CITY, N'') AS FULL_ADDRESS
FROM dbo.CUSTOMER;
");
*/
            /*
            //+ (łączenie), wrzuca do wyniku zapytania; COALESCE - zastępuje NULL jakimś znakiem
            p.Query(@"
USE Training;
SELECT ADDRESS, CITY,
CONCAT(ADDRESS, N',' + CITY) AS FULL_ADDRESS
FROM dbo.CUSTOMER;
*/
            Console.WriteLine("EMPLOYEE:");
            //DATETIME - druk + opcjonalnie wybór po dacie - literał
            p.EmployeeQuery(@"
USE Training;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE
WHERE START_DATE = '20020912';
");
        }
    }
}
