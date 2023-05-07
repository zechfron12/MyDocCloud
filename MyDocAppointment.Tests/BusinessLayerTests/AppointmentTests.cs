using MyDocAppointment.BusinessLayer.Entities;
using FluentAssertions;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class AppointmentTests
    {
        [Fact]
        public void Given_AddDoctorToAppointment_IsCalled_When_Doctor_isNull_Then_Should_Return_Failure()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment = new Appointment(startTime, endTime);

            //act
            var result = appointment.AddDoctorToAppointment(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Doctor should not be null");
        }

        [Fact]
        public void Given_AddDoctorToAppointment_IsCalled_When_Doctor_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment = new Appointment(startTime, endTime);
            var doctor = new Doctor("Jackie", "Chan", "all", "na", "na", "Dr.", "Medic specialist", "Iasi", 9.10, 117);

            //act
            var result = appointment.AddDoctorToAppointment(doctor);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void Given_AddPatientToAppointment_IsCalled_When_Patient_isNull_Then_Should_Return_Failure()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment = new Appointment(startTime, endTime);

            //act
            var result = appointment.AddPatientToAppointment(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Patient should not be null");
        }

        [Fact]
        public void Given_AddPatientToAppointment_IsCalled_When_Patient_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment = new Appointment(startTime, endTime);
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");

            //act
            var result = appointment.AddPatientToAppointment(patient);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
