using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Medication
    {
        public Medication(string name, int stock, string unit, int capacity, double price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stock = stock;
            Prescriptions = new List<Prescription>();
            Histories = new List<History>();
            Bills = new List<Bill>();
            Unit = unit;
            Capacity = capacity;
            Price = price;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public int Stock { get; private set; }

        public string Unit { get; private set; }
        public int Capacity { get; private set; }
        public double Price { get; private set; }

        public ICollection<Prescription> Prescriptions { get; private set; }
    
        public ICollection<History> Histories { get; private set; }

        public ICollection<Bill> Bills { get; private set; }

        public Result UpdateMedication(Medication? medication)
        {
            if (medication == null)
            {
                return Result.Failure("Medication should not be null");
            }

            Name = medication.Name;
            Stock= medication.Stock;
            Unit= medication.Unit;
            Capacity= medication.Capacity;
            Price = medication.Price;

            return Result.Success();
        }

        public Result RegisterBillToMedication(Bill? bill)
        {
            if (bill == null)
            {
                return Result.Failure("Bill should not be null");
            }

            Bills.Add(bill);

            return Result.Success();
        }

        public Result UpdateStock()
        {
            if (Stock >= 1)
            {
                Stock -= 1;
                return Result.Success();
            }

            return Result.Failure($"Medication {Name} does not have enough stock.");
        }
    }
}
