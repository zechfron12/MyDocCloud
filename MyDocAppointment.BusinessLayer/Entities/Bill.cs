using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Bill
    {
        public Bill()
        {
            Id = Guid.NewGuid();
            Medications = new List<Medication>();
            PaymentId = "";
            BillTotal= 0;  
        }
        public Guid Id { get; private set; }

        public ICollection<Medication> Medications { get; private set; }

        public double BillTotal { get; private set; }

        public Payment? Payment { get; private set; }

        public string PaymentId { get; private set; }

        public Result AddMedications(List<Medication> medications)
        {
            if (!medications.Any())
            {
                return Result.Failure("You must add at least one medication");
            }

            medications.ForEach(m =>
            {
                m.RegisterBillToMedication(this);
                Medications.Add(m);
            });

            BillTotal += medications.Sum(m => m.Price);

            return Result.Success();
        }

        public Result AddPaymentToBill(Payment? payment)
        {
            if (payment == null)
            {
                return Result.Failure("Payment should not be null");
            }

            Payment = payment;
            PaymentId = payment.Id;
            return Result.Success();
        }

        public Result RemoveMedication(Medication medication)
        {
            var result = Medications.Remove(medication);
            if(result)
            {
                BillTotal -= medication.Price;
            }
            return Result.Success();
        }

    }
}
