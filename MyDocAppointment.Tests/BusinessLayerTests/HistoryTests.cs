using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class HistoryTests
    {
        [Fact]
        public void Given_AddPatientToHistory_IsCalled_When_Patient_isNull_Then_Should_Return_Failure()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);

            //act
            var result = history.AddPatientToHistory(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Patient should not be null");
        }

        [Fact]
        public void Given_AddPatientToHistory_IsCalled_When_Patient_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);
            var patient = new Patient("First", "Last", "firstlast@yahoo.ro", "0711111111");

            //act
            var result = history.AddPatientToHistory(patient);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Given_AddMedications_IsCalled_When_ListIsNotEmpty_ThenShould_Return_Succcess()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);

            var medications = new List<MedicationDosageHistory>()
            {
                new MedicationDosageHistory(startTime, endTime, 1, 2)
            };

            //act
            var result = history.AddMedications(medications);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public void Given_AddMedications_IsCalled_When_ListIsEmpty_ThenShould_Return_Failure()
        {
            //arrange
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);
            var medications = new List<MedicationDosageHistory>();

            //act
            var result = history.AddMedications(medications);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("You must add at least a medication dosage");
        }

        [Fact]
        public void Given_IsStartDateValid_When_Start_isInFuture_Then_Should_Return_True()
        {
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);

            Assert.True(history.IsStartDateValid());

        }
        
        [Fact]
        public void Given_IsStartDateValid_When_Start_isInPast_Then_Should_Return_False()
        {
            DateTime startTime = DateTime.Now.AddDays(-1);
            DateTime endTime = DateTime.Now.AddDays(2);
            var history = new History(startTime, endTime);

            Assert.False(history.IsStartDateValid());

        }
    }
}
