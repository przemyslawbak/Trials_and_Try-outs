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

            //podzapytanie niezależne, wybiera max ID
            p.CustomerQuery(@"
USE Training;
DECLARE @q AS INT = (SELECT MAX(CUST_ID) FROM CUSTOMER);
SELECT ADDRESS, CITY
FROM CUSTOMER
WHERE CUST_ID = @q;
");
            //Podzapytania niezależne o wielu wartościach
            p.CustomerQuery(@"
USE Training;
SELECT EMP_ID
FROM EMPLOYEE
WHERE ASSIGNED_BRANCH_ID IN
(SELECT BRANCH_ID
FROM BRANCH
WHERE NAME LIKE N'W%');
");
            //Podzapytanie skorelowane
            p.CustomerQuery(@"
USE Training;
SELECT ASSIGNED_BRANCH_ID, EMP_ID, START_DATE
FROM EMPLOYEE AS O1
WHERE EMP_ID =
(SELECT MAX(O2.EMP_ID)
FROM EMPLOYEE AS O2
WHERE O2.ASSIGNED_BRANCH_ID = O1.ASSIGNED_BRANCH_ID);
");
            //EXISTS - zwraca TRUE lub FALSE
            p.CustomerQuery(@"
USE Training;
SELECT ASSIGNED_BRANCH_ID, EMP_ID, START_DATE
FROM EMPLOYEE AS O1
WHERE EMP_ID =
(SELECT MAX(O2.EMP_ID)
FROM EMPLOYEE AS O2
WHERE O2.ASSIGNED_BRANCH_ID = O1.ASSIGNED_BRANCH_ID);
");
        }
    }
}
