using System.Collections;

namespace HRPayrollManagementApp
{
    public class PayrollRepository : IRepository<Payroll>
    {
        private static PayrollRepository _instance;
        public static PayrollRepository Instance
        {
            get
            {
                return _instance ?? new PayrollRepository();
            }
        }

        private List<Payroll> Data;

        private PayrollRepository()
        {
            Data = new List<Payroll>
            {
                new Payroll(1, 1, 50000, DateTime.Now.AddMonths(-1)),
                new Payroll(2, 2, 40000, DateTime.Now.AddMonths(-1)),
                new Payroll(6, 6, 95000, DateTime.Now.AddMonths(-1))
            };
        }

        public void Dispose()
        {
            Data.Clear();
        }

        IEnumerable<Payroll> IRepository<Payroll>.Data { get { return Data; } }

        void IRepository<Payroll>.Add(Payroll entity)
        {
            if (Data.Any(p => p.Id == entity.Id))
            {
                throw new Exception("Payroll Id Is Duplicate, Try Another");
            }
            else if (entity.IsValid())
            {
                Data.Add(entity);
            }
            else
            {
                throw new Exception("Payroll Data Is not valid");
            }
        }

        bool IRepository<Payroll>.Delete(Payroll entity)
        {
            return Data.Remove(entity);
        }

        void IRepository<Payroll>.Update(Payroll entity)
        {
            var index = Data.FindIndex(p => p.Id == entity.Id);
            if (index >= 0)
            {
                Data[index] = entity;
            }
        }

        Payroll? IRepository<Payroll>.FindById(int Id)
        {
            return Data.FirstOrDefault(p => p.Id == Id);
        }

        IEnumerable<Payroll> IRepository<Payroll>.Search(string value)
        {
            return Data.Where(p => p.Salary.ToString().Contains(value) || p.PayPeriod.ToString("yyyy-MM-dd").Contains(value));
        }

        public IEnumerator<Payroll> GetEnumerator()
        {
            foreach (var payroll in Data)
            {
                yield return payroll;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


}
