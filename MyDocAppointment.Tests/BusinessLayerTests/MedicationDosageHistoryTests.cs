using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class MedicationDosageHistoryTests
    {
        [Fact]
        public void Given_AddMedication_IsCalled_When_Medication_isEmpty_Then_Should_Return_Failure()
        {
            //arrange
            var startTime = DateTime.Now.AddDays(5);
            var endTime = DateTime.Now.AddDays(10);
            var medicationDosage = new MedicationDosageHistory(startTime, endTime, 2, 12);

            //act
            var result = medicationDosage.AddMedication(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Medication should not be null");
        }

        [Fact]
        public void Given_AddMedication_IsCalled_When_Medication_isNotEmpty_Then_Should_Return_Success()
        {
            //arrange
            var startTime = DateTime.Now.AddDays(5);
            var endTime = DateTime.Now.AddDays(10);
            var medicationDosage = new MedicationDosageHistory(startTime, endTime, 2, 12);
            var medication = new Medication("name", 100, "ml", 10000, 25.50);

            //act
            var result = medicationDosage.AddMedication(medication);

            //assert
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public void Given_RegisterMedicationInfoToHistory_IsCalled_When_History_isEmpty_Then_Should_Return_Failure()
        {
            //arrange
            var startTime = DateTime.Now.AddDays(5);
            var endTime = DateTime.Now.AddDays(10);
            var medicationDosage = new MedicationDosageHistory(startTime, endTime, 2, 12);

            //act
            var result = medicationDosage.RegisterMedicationInfoToHistory(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("History should not be null");
        }

        [Fact]
        public void Given_RegisterMedicationInfoToHistory_IsCalled_When_History_isNotEmpty_Then_Should_Return_Success()
        {
            //arrange
            var startTime = DateTime.Now.AddDays(5);
            var endTime = DateTime.Now.AddDays(10);
            var medicationDosage = new MedicationDosageHistory(startTime, endTime, 2, 12);
            var history = new History(startTime, endTime);

            //act
            var result = medicationDosage.RegisterMedicationInfoToHistory(history);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

    }
}
