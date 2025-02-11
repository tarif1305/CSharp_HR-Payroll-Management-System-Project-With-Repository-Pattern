using System.Collections;

namespace HRPayrollManagementApp
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private static EmployeeRepository _instance;
        public static EmployeeRepository Instance
        {
            get
            {
                return _instance ?? new EmployeeRepository();
            }
        }

        private List<Employee> Data;

        private EmployeeRepository()
        {
            Data = new List<Employee>
            {
                new Employee(1, "Tarif", "Hossain", new DateTime(1997, 5, 15), EmployeeType.FullTime),
                new Employee(2, "Ahsanul", "Kabir", new DateTime(1996, 3, 25), EmployeeType.PartTime),
                new Employee(6, "Rohan", "Amir", new DateTime(1999, 3, 25), EmployeeType.Contractor)
            };
        }

        public void Dispose()
        {
            Data.Clear();
        }

        IEnumerable<Employee> IRepository<Employee>.Data { get { return Data; } }

        void IRepository<Employee>.Add(Employee entity)
        {
            if (Data.Any(e => e.Id == entity.Id))
            {
                throw new Exception("Duplicate employee ID, try another");
            }
            else if (entity.IsValid())
            {
                Data.Add(entity);
            }
            else
            {
                throw new Exception("Invalid employee data");
            }
        }

        bool IRepository<Employee>.Delete(Employee entity)
        {
            return Data.Remove(entity);
        }

        void IRepository<Employee>.Update(Employee entity)
        {
            var index = Data.FindIndex(e => e.Id == entity.Id);
            if (index >= 0)
            {
                Data[index] = entity;
            }
        }

        Employee IRepository<Employee>.FindById(int Id)
        {
            return Data.FirstOrDefault(e => e.Id == Id);
        }

        IEnumerable<Employee> IRepository<Employee>.Search(string value)
        {
            return Data.Where(e => e.FirstName.Contains(value) || e.LastName.Contains(value));
        }

        public IEnumerator<Employee> GetEnumerator()
        {
            foreach (var employee in Data)
            {
                yield return employee;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


}
