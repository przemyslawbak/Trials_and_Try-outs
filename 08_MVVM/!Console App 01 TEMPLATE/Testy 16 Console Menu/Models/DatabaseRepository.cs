using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Testy_16_Console_Menu.Models
{
    public class DatabaseRepository
    {
        static readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True; MultipleActiveResultSets=true";
        public static void EditItem (DatabaseModel toEdit) //edit the item
        {
            using (var context = new ApplicationDbContext())
            {
                DatabaseModel newEdit = context.DataModels.SingleOrDefault(i => i.ID == toEdit.ID);
                if (newEdit == null)
                {
                    Console.WriteLine($"{Environment.NewLine}Can not find ID number");
                }
                else
                {
                    newEdit.Name = toEdit.Name;
                    newEdit.Salary = toEdit.Salary;
                    newEdit.StartWork = toEdit.StartWork;
                    context.SaveChanges();
                }
            }
        }
        public static void AddItem(DatabaseModel item) //add new item
        {
            using (var context = new ApplicationDbContext())
            {
                context.DataModels.Add(item);
                context.SaveChanges();
            }
        }
        public static void DeleteItem(string id) //delete the item
        {
            using (var context = new ApplicationDbContext())
            {
                int parsedID = int.Parse(id);
                DatabaseModel model = context.DataModels.SingleOrDefault(i => i.ID == parsedID);
                if (model == null)
                {
                    Console.WriteLine($"{Environment.NewLine}Can not find ID number");
                }
                else
                {
                    context.DataModels.Remove(model);
                    context.SaveChanges();
                }
            }
        }
        public static bool CheckForDB() //check if 'dbConsoleApp' DB exists
        {
            bool result = false;
            string sqlCreateDBQuery = string.Format("SELECT s FROM sys.databases WHERE name = 'dbConsoleApp'");
            SqlConnection tmpConn = new SqlConnection(connectionString);
            using (tmpConn)
            {
                using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                {
                    tmpConn.Open();
                    object resultObj = sqlCmd.ExecuteScalar();
                    int databaseID = 0;
                    if (resultObj != null)
                    {
                        int.TryParse(resultObj.ToString(), out databaseID);
                    }
                    tmpConn.Close();
                    result = (databaseID > 0);
                }
            }
            return result;
        }
        public static void PrintAllData() //print out entities
        {
            using (var context = new ApplicationDbContext())
            {
                IEnumerable<DatabaseModel> dataList = context.DataModels.ToList();
                Console.WriteLine($"Data in database:{Environment.NewLine}");
                Console.WriteLine("ID".PadRight(7) + "Name".PadRight(25) + "Salary".PadRight(12) + "Joined");
                foreach (var item in dataList)
                {
                    Console.WriteLine(item.ID.ToString().PadRight(7) +
                        item.Name.PadRight(25) +
                        item.Salary.ToString().PadRight(12) +
                        item.StartWork.ToShortDateString());
                }
            }
            Console.WriteLine("");
        }
        public static void PopulateDB() //populating DB with prepared entities
        {
            using (var context = new ApplicationDbContext())
            {
                context.DataModels.Add(new DatabaseModel { Name = "Grzegorz", Salary = 1000.00, StartWork = DateTime.Now });
                context.DataModels.Add(new DatabaseModel { Name = "Przemyslaw", Salary = 6000.00, StartWork = DateTime.Now.AddDays(-1) });
                context.DataModels.Add(new DatabaseModel { Name = "Alicja", Salary = 4000.00, StartWork = DateTime.Now.AddDays(-5) });
                context.SaveChanges();
                Console.WriteLine($"DB was not populated, added new models.{Environment.NewLine}");
            }
        }
        public static void PopulateCheck()
        {
            using (var context = new ApplicationDbContext())
            {
                Models.DatabaseModel model = new Models.DatabaseModel();
                model = context.DataModels.FirstOrDefault();
                if (model == null)
                {
                    PopulateDB(); //if DB is not populated, seed DB
                }
            }
        }
        public static void CreateDatabase() //creating new DB
        {
            bool result = CheckForDB();
            try
            {
                if (result == false) //if DB not existing, create new one
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        //For MS SQL Server 2000, 2005, 2008
                        connection.Open();
                        var command = connection.CreateCommand();
                        //create database
                        command.CommandText = "CREATE DATABASE dbConsoleApp";
                        //execute
                        command.ExecuteNonQuery();
                        //info
                        command = connection.CreateCommand();
                        command.CommandText = @"
                        CREATE TABLE dbConsoleApp.dbo.DataModels(
                        ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Salary FLOAT(53) NOT NULL,
                        StartWork DATETIME NOT NULL)";
                        //execute
                        command.ExecuteNonQuery();
                        Console.WriteLine($"DB dbConsoleApp not found, created new one.{Environment.NewLine}");
                        connection.Close();
                    }
                    PopulateDB(); //after creating populate
                }
            }
            catch (Exception ex)
            {
                //exception...
            }
        }
    }
}
