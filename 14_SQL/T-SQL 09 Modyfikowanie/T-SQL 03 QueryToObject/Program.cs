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
            //INSERT VALUES - wstawienie pojedynczego wiersza w tabeli Orders
            p.EmployeeQuery(@"
USE Training;
INSERT INTO dbo.ORDERS(orderid, orderdate, empid, custid)
VALUES(10001, '20160212', 3, 'A');
");
            //INSERT VALUES - wstawienie kilku wierszy w tabeli Orders
            p.EmployeeQuery(@"
USE Training;
INSERT INTO dbo.Orders
(orderid, orderdate, empid, custid)
VALUES
(10003, '20160213', 4, 'B'),
(10004, '20160214', 1, 'A'),
(10005, '20160213', 1, 'C'),
(10006, '20160215', 3, 'C');
");
//INSERT SELECT - wstawienie wierwszy zwróconych przez SELECT
            p.EmployeeQuery(@"
            USE TSQLV4;
INSERT INTO dbo.Orders(orderid, orderdate, empid, custid)
SELECT orderid, orderdate, empid, custid
FROM Sales.Orders
WHERE shipcountry = 'UK';
");
//EXEC - wykonanie istniejącej procedury
            p.EmployeeQuery(@"
            USE TSQLV4;
EXEC Sales.usp_getorders @country = 'France';
");
//SELECT INTO - wstawia elementy do nowej tabeli (nie może istnieć)
            p.EmployeeQuery(@"
            USE TSQLV4;
DROP TABLE IF EXIST dbo.Orders;
SELECT orderid, orderdate, empid, custid
INTO dbo.Orders
FROM Sales.Orders;
//IDENTITY - tworzenie kluczy PK, Sekwencje są alternatywą dla Identity, czasami są elastyczniejsze
            p.EmployeeQuery(@"
            USE TSQLV4;
DROP TABLE IF EXISTS dbo.T1;
CREATE TABLE dbo.T1
(
keycol INT NOT NULL IDENTITY(1, 1)
CONSTRAINT PK_T1 PRIMARY KEY,
datacol VARCHAR(10) NOT NULL
CONSTRAINT CHK_T1_datacol CHECK(datacol LIKE '[A-Za-z]%')
);
");
*/
            //DELETE i TURNCATE
            p.EmployeeQuery(@"
            USE Training;
DELETE FROM EMPLOYEE
WHERE START_DATE > '20021231';
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE;
USE TSQLV4;
TRUNCATE TABLE dbo.T1;
");
            //UPDATE
            p.EmployeeQuery(@"
            USE Training;
USE Training;
UPDATE EMPLOYEE
SET FIRST_NAME = 'Miron'
WHERE EMP_ID = 1;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE;
");
            /*
             * //Przykład dla MERGE, gdzie przy MATCHED -> UPDATE, przy NOT MATCHED -> INSERT
            p.EmployeeQuery(@"
            MERGE INTO dbo.Customers AS TGT
USING dbo.CustomersStage AS SRC
ON TGT.custid = SRC.custid
WHEN MATCHED THEN
UPDATE SET
TGT.companyname = SRC.companyname,
TGT.phone = SRC.phone,
TGT.address = SRC.address
WHEN NOT MATCHED THEN
INSERT (custid, companyname, phone, address)
VALUES (SRC.custid, SRC.companyname, SRC.phone, SRC.address);
);
");

            //DELETE + TOP
            p.EmployeeQuery(@"
            USE Training;
DELETE TOP(2) FROM EMPLOYEE;
SELECT FIRST_NAME, LAST_NAME, START_DATE
FROM EMPLOYEE;
");*/
            //OUTPUT + INSERT (podobnie z DELETE, MERGE i UPDATE)
            p.EmployeeQuery(@"
            USE Training;
USE Training;
INSERT INTO EMPLOYEE(EMP_ID, FIRST_NAME, LAST_NAME, START_DATE)
OUTPUT inserted.TITLE, inserted.START_DATE
VALUES ('20', 'Miron', 'Zaparlo', '20190201')
");
        }
    }
}
