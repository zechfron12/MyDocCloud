using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Appointment
    {

        public Appointment(DateTime startTime, DateTime endTime)
        {
            Id = Guid.NewGuid();
            StartTime = startTime;
            EndTime = endTime;
        }

        public Guid Id { get; private set; }
        public Doctor? Doctor { get; private set; }
        public Guid DoctorId { get; private set; }
        public Patient? Patient { get; private set; }
        public Guid PatientId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public Result AddDoctorToAppointment(Doctor? doctor)
        {
            if (doctor == null)
            {
                return Result.Failure("Doctor should not be null");
            }

            Doctor = doctor;
            DoctorId = doctor.Id;
            return Result.Success();
        }

        public Result AddPatientToAppointment(Patient? patient)
        {
            if (patient == null)
            {
                return Result.Failure("Patient should not be null");

            }

            Patient = patient;
            PatientId = patient.Id;
            return Result.Success();
        }

    }
}
