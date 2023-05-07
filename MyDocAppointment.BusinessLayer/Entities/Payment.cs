using ShelterManagement.Business.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Payment
    {
        public Payment(string Id, int OrderStatus, string CardholderName) 
        {
            this.Id = Id;
            this.OrderStatus = OrderStatus;
            this.CardholderName = CardholderName;
        }
        public string Id { get; private set; } // this is OrderId
        public int OrderStatus { get; private set; }
        public string CardholderName { get; private set; }
        public Bill? Bill { get; private set; }
        public Guid BillId { get; private set; }

        public Result AddBillToPayment(Bill? bill)
        {
            if (bill == null)
            {
                return Result.Failure("Bill should not be null");
            }

            Bill = bill;
            BillId = bill.Id;
            return Result.Success();
        }
    }
}
