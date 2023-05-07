using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class PatientTests
    {
        [Fact]
        public void Given_AddAppointment_IsCalled_When_Appointment_isNull_Then_Should_Return_Failure()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");

            //act
            var result = patient.AddAppointment(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Appointment should not be null");
        }

        [Fact]
        public void Given_AddAppointment_IsCalled_When_AppointmentStartTime_isInPast_Then_Should_Return_Failure()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");
            DateTime startTime = DateTime.Now.AddDays(-5);
            DateTime endTime = DateTime.Now;
            var appointment = new Appointment(startTime, endTime);

            //act
            var result = patient.AddAppointment(appointment);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Appointment should be in the future");
        }

        [Fact]
        public void Given_AddAppointment_IsCalled_When_AppointmentStartTime_isAfterEndTime_Then_Should_Return_Failure()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");
            DateTime startTime = DateTime.Now.AddDays(5);
            DateTime endTime = DateTime.Now;
            var appointment = new Appointment(startTime, endTime);

            //act
            var result = patient.AddAppointment(appointment);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Start time should be before End time");
        }

        [Fact]
        public void Given_AddAppointment_IsCalled_When_Appointment_overlaps_Then_Should_Return_Failure()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment1 = new Appointment(startTime, endTime);
            var appointment2 = new Appointment(startTime, endTime);
            var result1 = patient.AddAppointment(appointment1);

            //act
            var result2 = patient.AddAppointment(appointment2);

            //assert
            result2.IsFailure.Should().BeTrue();
            result2.Error.Should().Be("A new appoinments should not intersect with a fixed appointment");
        }

        [Fact]
        public void Given_AddAppointment_IsCalled_When_Appointment_isGood_Then_Should_Return_Success()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var appointment = new Appointment(startTime, endTime);

            //act
            var result = patient.AddAppointment(appointment);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void Given_AddPrescription_IsCalled_When_Prescription_isNull_Then_Should_Return_Failure()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");

            //act
            var result = patient.AddPrescription(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Prescription should not be null");
        }
        
        [Fact]
        public void Given_AddPrescription_IsCalled_When_Prescription_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");
            var prescription = new Prescription();

            //act
            var result = patient.AddPrescription(prescription);

            //assert
            result.IsSuccess.Should().BeTrue();

        }
    }
}
