namespace MyDocAppointment.API.Features.Bills
{
    public class PaymentDto
    {
        public string? Id { get; set; } // this is OrderId
        public int OrderStatus { get; set; }
        public string? CardholderName { get; set; }
    }
}
