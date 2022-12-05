using System;
using System.Collections.Generic;

namespace Pattern_
{
    //factory: https://dev.to/gary_woodfine/how-to-use-factory-method-design-pattern-in-c-3ia3
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeBusinessLogic BL = new EmployeeBusinessLogic();
            Employee employeeDetails = BL.GetEmployeeDetails(1);
            Console.WriteLine();
            Console.WriteLine("Employee Details:");
            Console.WriteLine("ID : {0}, Name : {1}, Department : {2}, Salary : {3}",
                                employeeDetails.ID, employeeDetails.Name, employeeDetails.Department,
                                employeeDetails.Salary);
            // Wait for user
            Console.ReadKey();
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }

    public class EmployeeDataAccess
    {
        public Employee GetEmployeeDetails(int id)
        {
            // In real-time get the employee details from db
            //but here we are hard coded the employee details
            Employee emp = new Employee()
            {
                ID = id,
                Name = "Pranaya",
                Department = "IT",
                Salary = 10000
            };
            return emp;
        }
    }

    public class EmployeeBusinessLogic
    {
        EmployeeDataAccess _EmployeeDataAccess;
        public EmployeeBusinessLogic()
        {
            _EmployeeDataAccess = new EmployeeDataAccess();
        }
        public Employee GetEmployeeDetails(int id)
        {
            return _EmployeeDataAccess.GetEmployeeDetails(id);
        }
    }

}
