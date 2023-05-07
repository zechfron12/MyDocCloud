namespace MyDocAppointment.API.Features.Histories
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
