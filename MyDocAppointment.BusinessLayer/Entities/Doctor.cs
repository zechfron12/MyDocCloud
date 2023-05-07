using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Doctor
    {
        private const string SEPARATOR = ", ";

        public Doctor(string firstName, string lastName, string specialization, string email, string phone, string title, string profession, string location, double grade, int reviews)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            Email = email; 
            Phone = phone;
            Title = title;
            Profession = profession;
            Location = location;
            Grade = grade;
            Reviews = reviews;
            Appointments = new List<Appointment>();
            Prescriptions = new List<Prescription>();
        }

        public Guid Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Specialization { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }
        public string Title { get; private set; }
        public string Profession { get; private set; }
        public string Location { get; private set; }
        public double Grade { get; private set; }

        public int Reviews { get; private set; }



        public Hospital? Hospial { get; private set; }

        public Guid? HospitalId { get; private set; }

        public ICollection<Appointment> Appointments { get; private set; }
        public ICollection<Prescription> Prescriptions { get; private set; }


        public string FullName
        {
            get
            {
                return FirstName + SEPARATOR + LastName;
            }
        }

        public Result AddReview(int review)
        {
            if (review < 1 || review >10)
            {
                return Result.Failure("The review grade should be between 1 and 10");
            }

            Grade = (Grade * Reviews + review) / (Reviews + 1);
            Reviews += 1;

            return Result.Success();
        }

        public Result AddHospitalToDoctor(Hospital? hospital)
        {
            if (hospital == null)
            {
                return Result.Failure("Hospital should not be null");
            }
            else
            {
                this.Hospial = hospital;
                HospitalId = hospital.Id;
                return Result.Success();
            }
        }

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

            if (appointment.StartTime > appointment.EndTime)
            {
                return Result.Failure("Start time should be before End time");
            }

            foreach (var existentAppointment in Appointments)
            {
                if (!(appointment.StartTime >= existentAppointment.EndTime 
                        || appointment.EndTime <= existentAppointment.StartTime))
                {
                    return Result.Failure("A new appointment should not intersect with a fixed appointment");
                }
            }

            appointment.AddDoctorToAppointment(this);
            Appointments.Add(appointment);
            return Result.Success();
            
        }

        public Result AddPrescription(Prescription? prescription)
        {
            if (prescription == null)
            {
                return Result.Failure("Prescription should not be null");
            }

            prescription.AddDoctorToPrescription(this);
            Prescriptions.Add(prescription);
            return Result.Success();
        }

        public Result UpdateDoctor(Doctor? doctor)
        {
            if (doctor == null)
            {
                return Result.Failure("Doctor should not be null");
            }
            FirstName = doctor.FirstName;
            LastName= doctor.LastName;
            Specialization= doctor.Specialization;
            Email= doctor.Email;
            Phone= doctor.Phone;
            Title= doctor.Title;
            Profession= doctor.Profession;
            Location=doctor.Location;
            Grade= doctor.Grade;
            Reviews= doctor.Reviews;

            return Result.Success();
        }

    }
}