using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class MedicationTests
    {
        [Fact]
        public void Given_UpdateMedication_IsCalled_When_Medication_isNull_Then_Should_Return_Failure()
        {
            //arrange
            var medication = new Medication("paracetamol", 100, "cutie", 10,  20.00);
            //act
            var result = medication.UpdateMedication(null);

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Medication should not be null");
        }
        
        [Fact]
        public void Given_UpdateMedication_IsCalled_When_Medication_isNotNull_Then_Should_Return_Success()
        {
            //arrange
            var medication = new Medication("paracetamol", 100, "cutie", 10,  20.00);
            var updatedMedication = new Medication("Paracetamol", 100, "cutie", 10, 20.00);

            //act
            var result = medication.UpdateMedication(updatedMedication);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

    }
}
