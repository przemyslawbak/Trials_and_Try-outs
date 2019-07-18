using System;
using System.Data.SqlClient;

namespace T_SQL_01_Create_table
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                //tworzenie tabeli
                command.CommandText = @"
                        USE dbApi;
DROP TABLE IF EXISTS dbo.Employees;
CREATE TABLE dbo.Employees
(
Empid INT NOT NULL,
firstname VARCHAR(30) NOT NULL,
lastname VARCHAR(30) NOT NULL,
hiredate DATE NOT NULL,
mgrid INT NULL,
ssn VARCHAR(20) NOT NULL,
salary MONEY NOT NULL
);
";
                command.ExecuteNonQuery();
                //dodanie klucza głównego do empid
                command.CommandText = @"
                        ALTER TABLE dbo.Employees
ADD CONSTRAINT PK_Employees
PRIMARY KEY(empid);
";
                command.ExecuteNonQuery();
                //unikatowość
                command.CommandText = @"
                        ALTER TABLE dbo.Employees
ADD CONSTRAINT UNQ_Employees_ssn
UNIQUE(ssn);
";
                command.ExecuteNonQuery();
                //utworzenie nowej tabeli
                command.CommandText = @"
DROP TABLE IF EXISTS dbo.Orders;
CREATE TABLE dbo.Orders
(
orderid INT NOT NULL,
empid INT NOT NULL,
custid VARCHAR(10) NOT NULL,
orderts DATETIME2 NOT NULL,
qty INT NOT NULL,
CONSTRAINT PK_Orders
PRIMARY KEY(orderid)
);
";
                command.ExecuteNonQuery();
                /*
                //dodanie obcego klucza - z obcym kluczem nie da się DROP tabeli
                command.CommandText = @"
                        ALTER TABLE dbo.Orders
ADD CONSTRAINT FK_Orders_Employees
FOREIGN KEY(empid)
REFERENCES dbo.Employees(empid);
";
                command.ExecuteNonQuery();
                */

                //check - nie da się wprowadzić wartości poniżej 0
                command.CommandText = @"
                        ALTER TABLE dbo.Employees
ADD CONSTRAINT CHK_Employees_salary
CHECK(salary > 0.00);
";
                command.ExecuteNonQuery();
                //default
                command.CommandText = @"
                        ALTER TABLE dbo.Orders
ADD CONSTRAINT DFT_Orders_orderts
DEFAULT(SYSDATETIME()) FOR orderts;
";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
