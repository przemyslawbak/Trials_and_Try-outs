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
            //DECLARE i SET, można też przypisywać zmiennej zwracaną wartość z SELECT
            p.T1Query(@"
DECLARE @i AS INT;
SET @i = 10;
USE TSQLV4;
SELECT keycol, col1 + @i, col2
FROM dbo.T1;
");
            //Przyklad wsadu
            p.T1Query(@"
-- Prawidłowy wsad
PRINT 'Pierwszy wsad';
USE TSQLV4;
SELECT keycol, col1, col2
FROM dbo.T1;
");
            //GO (GO n obsługuje argument wskazujący, ile razy chcemy wykonać wsad.
                        p.T1Query(@"
USE TSQLV4;
SELECT keycol, col1, col2
FROM dbo.T1;
GO
");
            //IF ... ELSE (mogą być zagnieżdżane)
            p.T1Query(@"
IF YEAR(SYSDATETIME()) <> YEAR(DATEADD(day, 1, SYSDATETIME()))
PRINT 'Dzisiejszy dzień to ostatni dzień roku.';
ELSE
PRINT 'Dzisiejszy dzień nie jest ostatnim dniem roku.';
USE TSQLV4;
SELECT keycol, col1, col2
FROM dbo.T1;
GO
");
            //WHERE
            p.T1Query(@"
DECLARE @i AS INT = 1;
WHILE @i <= 10
BEGIN
PRINT @i;
SET @i = @i + 1;
END;
USE TSQLV4;
SELECT keycol, col1, col2
FROM dbo.T1;
GO
");
            //CURSOR
            p.T1Query(@"
USE TSQLV4;
DECLARE kursor CURSOR FOR
SELECT col2, col1 FROM T1 WHERE col1 > 200
DECLARE @kolumna2 VARCHAR(50), @kolumna1 INT
PRINT 'Pracownicy o pensji wyższej niż 200:'
OPEN kursor
FETCH NEXT FROM kursor INTO @kolumna2, @kolumna1
WHILE @@FETCH_STATUS = 0
   BEGIN
      PRINT @kolumna2 + ' ' + Cast(@kolumna1 As Varchar)
      FETCH NEXT FROM kursor INTO @kolumna2, @kolumna1
   END
CLOSE kursor
DEALLOCATE kursor
=
USE TSQLV4;
SELECT keycol, col1, col2
FROM dbo.T1;
GO
");
            /* <-- lokalna tabela tymczasowa, dostępn tylko z bieżącej sesji
             IF OBJECT_ID('tempdb.dbo.#MyOrderTotalsByYear') IS NOT NULL
DROP TABLE dbo.#MyOrderTotalsByYear;
GO
CREATE TABLE #MyOrderTotalsByYear
(
orderyear INT NOT NULL PRIMARY KEY,
qty INT NOT NULL
);
INSERT INTO #MyOrderTotalsByYear(orderyear, qty)
SELECT
YEAR(O.orderdate) AS orderyear,
SUM(OD.qty) AS qty
FROM Sales.Orders AS O
JOIN Sales.OrderDetails AS OD
ON OD.orderid = O.orderid
GROUP BY YEAR(orderdate);
SELECT Cur.orderyear, Cur.qty AS curyearqty, Prv.qty AS prvyearqty
FROM dbo.#MyOrderTotalsByYear AS Cur
LEFT OUTER JOIN dbo.#MyOrderTotalsByYear AS Prv
ON Cur.orderyear = Prv.orderyear + 1;
*/

            /* <-- globalna tabela tymczasowa, dostępna ze wszystkich sesji
             CREATE TABLE dbo.##Globals
(
id sysname NOT NULL PRIMARY KEY,
val SQL_VARIANT NOT NULL
);
*/
            /* <-- zmienna tablicowa, używamy DECLARE
                         DECLARE @MyOrderTotalsByYear TABLE
(
orderyear INT NOT NULL PRIMARY KEY,
qty INT NOT NULL
);
INSERT INTO @MyOrderTotalsByYear(orderyear, qty)
SELECT
YEAR(O.orderdate) AS orderyear,
SUM(OD.qty) AS qty
FROM Sales.Orders AS O
JOIN Sales.OrderDetails AS OD
ON OD.orderid = O.orderid
GROUP BY YEAR(orderdate);
SELECT orderyear, qty AS curyearqty,
LAG(qty) OVER(ORDER BY orderyear) AS prvyearqty
FROM @MyOrderTotalsByYear;
            */
        }
    }
}
