using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Hospital
    {
        public Hospital(string name, string address, string phone)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Phone = phone;
            Doctors = new List<Doctor>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }

        public ICollection<Doctor> Doctors { get; private set; }
        public bool Validate()
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(Name))
            {
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Address))
            {
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                isValid = false;
            }
            return isValid;
        }

        public Result AddDoctors(List<Doctor> doctors)
        {
            if (!doctors.Any())
            {
                return Result.Failure("You must add at least a doctor");
            }
            doctors.ForEach(d =>
            {
                if (!Doctors.Contains(d))
                {
                    d.AddHospitalToDoctor(this);
                    Doctors.Add(d);
                }
            });
            return Result.Success();
        }

    }
}


