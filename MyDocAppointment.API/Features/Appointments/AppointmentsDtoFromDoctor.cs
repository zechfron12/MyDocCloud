namespace MyDocAppointment.API.Features.Appointments
{
    public class AppointmentsDtoFromDoctor
    {
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid PatientId { get; set; }

        
    }
}
