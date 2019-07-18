using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testy_16_Console_Menu.Models
{
    class DataModelValidator
    {
        public bool IsNameValid(string name) //name validation
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cant be empty string");
                return false;
            }
            if (name.Length > 25)
            {
                Console.WriteLine("Maximum 25 characters");
                return false;
            }
            if (name.Any(char.IsDigit))
            {
                Console.WriteLine("Name can not contain digits");
                return false;
            }
            if (name.Any(char.IsSymbol))
            {
                Console.WriteLine("Name can not contain symbols");
                return false;
            }
            return true;

        }
        public bool IsSalaryValid(string salary) //salary validation
        {
            double number;
            if (double.TryParse(salary, out number))
            {
                return true;
            }
            else {
                Console.WriteLine("You need to enter a number");
                return false;
            }
        }
        public bool IsDateValid(string joined) //date validation
        {
            DateTime date;
            if (DateTime.TryParse(joined, out date))
            {
                return true;
            }
            else
            {
                Console.WriteLine("You need to enter date in proper format (yyyy-mm-dd)");
                return false;
            }
        }
        public bool IsIdValid(string id) //ID validation
        {
            int number;
            if (!int.TryParse(id, out number))
            {
                Console.WriteLine("ID have to be a number");
                return false;
            }
            using (var context = new ApplicationDbContext())
            {
                int parsedID = int.Parse(id);
                DatabaseModel item = context.DataModels.FirstOrDefault(i => i.ID == parsedID);
                if (item == null)
                {
                    Console.WriteLine("Can not find ID number");
                    return false;
                }
            }
            return true;
        }
    }
}
