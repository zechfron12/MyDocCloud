using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.API.Features.Appointments
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
