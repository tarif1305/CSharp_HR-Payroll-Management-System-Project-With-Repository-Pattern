using System;
using System.Collections.Generic;
using System.Linq;

namespace HRPayrollManagementApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Project Of HR Payroll Management System In Repository Pattern:" );
            Console.WriteLine("--------------------------------------------------------------" );

            Console.WriteLine();
            Console.ResetColor();
            
            try
            {
                using (IRepository<Employee> employeeRepository = EmployeeRepository.Instance)
                using (IRepository<Payroll> payrollRepository = PayrollRepository.Instance)
                {
                    // Adding employees
                    employeeRepository.Add(new Employee(3, "Ashraf", "Uddin", "1996/09/10", EmployeeType.FullTime));
                    employeeRepository.Add(new Employee(4, "Adil", "Irfan", new DateTime(1998, 12, 20), EmployeeType.PartTime));

                    // Adding payrolls
                    payrollRepository.Add(new Payroll(3, 3, 60000, DateTime.Now.AddMonths(-1)));
                    payrollRepository.Add(new Payroll(4, 4, 35000, DateTime.Now.AddMonths(-1)));

                    // Updating payroll
                    var payrollToUpdate = payrollRepository.FindById(3);
                    payrollToUpdate.Salary = 70000;
                    payrollRepository.Update(payrollToUpdate);

                    // Deleting an employee and associated payroll
                    var employeeToDelete = employeeRepository.FindById(4);
                    if (employeeToDelete != null)
                    {
                        var payrollsToDelete = payrollRepository.Search(employeeToDelete.Id.ToString()).ToList();
                        foreach (var payroll in payrollsToDelete)
                        {
                            payrollRepository.Delete(payroll);
                        }
                        employeeRepository.Delete(employeeToDelete);
                    }

                    // Displaying employees
                    foreach (var employee in employeeRepository)
                    {
                        Console.WriteLine(employee.ToString());
                    }

                    // Displaying payrolls
                    foreach (var payroll in payrollRepository)
                    {
                        Console.WriteLine(payroll.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
    public enum EmployeeType
    {
        FullTime,
        PartTime,
        Contractor
    }

    public interface IEntity : IDisposable
    {
        int Id { get; }
        bool IsValid();
    }
    public abstract class Person
    {
        private DateTime dateOfBirth;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public Person(string f, string l, DateTime dob)
        {
            this.FirstName = f;
            this.LastName = l;
            this.DateOfBirth = dob;
        }
        public Person(string f, string l, string dob)
        {
            this.FirstName = f;
            this.LastName = l;
            DateTime.TryParse(dob, out dateOfBirth);
        }
    }

    public sealed class Employee :Person, IEntity
    {
        public int Id { get; }

        public EmployeeType Type { get; set; }

        public Employee(int id, string firstName, string lastName, DateTime dateOfBirth, EmployeeType type):base(firstName, lastName, dateOfBirth)
        {
            Id = id;
            Type = type;
        }
        public Employee(int id, string firstName, string lastName, string dateOfBirth, EmployeeType type) : base(firstName, lastName, dateOfBirth)
        {
            Id = id;
            Type = type;
        }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        public override string ToString()
        {
            return $"Employee ID: {Id}\nName: {FirstName} {LastName}\nDate of Birth: {DateOfBirth:yyyy-MM-dd}\nType: {Type}\n";
        }

        public void Dispose()
        {

        }
    }

    public sealed class Payroll : IEntity
    {
        public int Id { get; }
        public int EmployeeId { get; }
        public decimal Salary { get; set; }
        public DateTime PayPeriod { get; set; }

        public Payroll(int id, int employeeId, decimal salary, DateTime payPeriod)
        {
            Id = id;
            EmployeeId = employeeId;
            Salary = salary;
            PayPeriod = payPeriod;
        }

        public bool IsValid()
        {
            return Salary > 0;
        }

        public override string ToString()
        {
            return $"Payroll ID: {Id}\nEmployee ID: {EmployeeId}\nSalary: {Salary}\nPay Period: {PayPeriod:yyyy-MM-dd}\n";
        }

        public void Dispose()
        {

        }
    }

    public interface IRepository<T> : IDisposable, IEnumerable<T> where T : IEntity
    {
        IEnumerable<T> Data { get; }
        void Add(T entity);
        bool Delete(T entity);
        void Update(T entity);
        T FindById(int Id);
        IEnumerable<T> Search(string value);
    }


}
