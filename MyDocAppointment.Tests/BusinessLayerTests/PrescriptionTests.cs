using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class PrescriptionTests
    {
        [Fact]
        public void Given_AddDoctorToPrescription_IsCalled_When_Doctor_isNull_Then_Should_Return_Failure()
        {
            //arrange
            var prescription = new Prescription();

            //act
            var result = prescription.AddDoctorToPrescription(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Doctor should not be null");
        }

        [Fact]
        public void Given_AddDoctorToPrescription_IsCalled_When_Doctor_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            var prescription = new Prescription();
            var doctor = new Doctor("Jackie", "Chan", "all", "na", "na", "Dr.", "Medic specialist", "Iasi", 9.10, 117);
            
            //act
            var result = prescription.AddDoctorToPrescription(doctor);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public void Given_AddPatientToPrescription_IsCalled_When_Patient_isNull_Then_Should_Return_Failure()
        {
            //arrange
            var prescription = new Prescription();

            //act
            var result = prescription.AddPatientToPrescription(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Patient should not be null");
        }

        [Fact]
        public void Given_AddPatientToPrescription_IsCalled_When_Patient_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            var prescription = new Prescription();
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");

            //act
            var result = prescription.AddPatientToPrescription(patient);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Given_AddMedications_IsCalled_When_MedicationsDosage_isEmpty_Then_Should_Return_Failure()
        {
            //arrange
            var prescription = new Prescription();
            var medicationDosages = new List<MedicationDosagePrescription>(); 
            
            //act
            var result = prescription.AddMedications(medicationDosages);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("You must add at least one medication dosage");
        }

        [Fact]
        public void Given_AddMedications_IsCalled_When_MedicationsDosage_isNotEmpty_Then_Should_Return_Success()
        {
            //arrange
            var startTime = DateTime.Now.AddDays(2);
            var endTime = DateTime.Now.AddDays(4);

            var prescription = new Prescription();
            var medicationDosages = new List<MedicationDosagePrescription>()
            {
                new MedicationDosagePrescription(startTime, endTime, 3, 10)
            };

            //act
            var result = prescription.AddMedications(medicationDosages);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
