
using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Patient
    {
        public Patient(string firstName, string lastName, string email, string phone)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Appointments = new List<Appointment>();
            Prescriptions = new List<Prescription>();
        }

        public Guid Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public ICollection<Appointment> Appointments { get; private set; }

        public ICollection<Prescription> Prescriptions { get; private set; }

        public Result AddAppointment(Appointment? appointment)
        {
            if(appointment == null)
            {
                return Result.Failure("Appointment should not be null");
            }    
            if(appointment.StartTime < DateTime.Now)
            {
                return Result.Failure("Appointment should be in the future");
            }
            if(appointment.StartTime > appointment.EndTime)
            {
                return Result.Failure("Start time should be before End time");
            }

            foreach (var existentAppointment in Appointments)
            {
                if (!(appointment.StartTime >= existentAppointment.EndTime
                         || appointment.EndTime <= existentAppointment.StartTime))
                {
                    return Result.Failure("A new appoinments should not intersect with a fixed appointment");
                }
            }

            appointment.AddPatientToAppointment(this);
            Appointments.Add(appointment);
            return Result.Success();
        }

        public Result AddPrescription(Prescription? prescription) 
        {
            if(prescription == null)
            {
                return Result.Failure("Prescription should not be null");
            }
            prescription.AddPatientToPrescription(this);
            Prescriptions.Add(prescription);

            return Result.Success();
        }

    }

}
