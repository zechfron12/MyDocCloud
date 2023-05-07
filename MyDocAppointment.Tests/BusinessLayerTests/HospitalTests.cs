using FluentAssertions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.Tests.BusinessLayerTests
{
    public class HospitalTests
    {
        [Fact]
        public void Given_AddDoctors_IsCalled_When_Doctors_isEmpty_Then_Should_Return_Failure()
        {
            //arrange
            var hospital = new Hospital("Hospital", "addr", "07324234234");

            //act
            var result = hospital.AddDoctors(new List<Doctor>());

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("You must add at least a doctor");
        }

        [Fact]
        public void Given_AddDoctors_IsCalled_When_ListIsNotEmpty_ThenShould_Return_Succcess()
        {
            //arrange
            var hospital = new Hospital("Hospital", "addr", "07324234234");
            var doctors = new List<Doctor>()
            {
                new Doctor("John", "Murray", "chirurg", "efsd.sdf@com", "07777777", "Dr.", "Medic Specialist", "Iasi", 9.10, 117)
            };

            //act
            var result = hospital.AddDoctors(doctors);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Given_Validate_IsCalled_When_NameIsWhitespace_ThenShould_Return_False()
        {
            //arrange
            var hospital = new Hospital(" ", "addr", "07324234234");
           
            //act
            var isValid = hospital.Validate();

            //assert
            Assert.False(isValid);
        }
        
        [Fact]
        public void Given_Validate_IsCalled_When_AddressIsWhitespace_ThenShould_Return_False()
        {
            //arrange
            var hospital = new Hospital("Hospital", " ", "07324234234");
           
            //act
            var isValid = hospital.Validate();

            //assert
            Assert.False(isValid);
        }
        
        [Fact]
        public void Given_Validate_IsCalled_When_PhoneIsWhitespace_ThenShould_Return_False()
        {
            //arrange
            var hospital = new Hospital("Hospital", "addr", " ");
           
            //act
            var isValid = hospital.Validate();

            //assert
            Assert.False(isValid);
        }
        
        [Fact]
        public void Given_Validate_IsCalled_When_NameAddressAndPhoneAreNotWhitespace_ThenShould_Return_True()
        {
            //arrange
            var hospital = new Hospital("Hospital", "addr", "07324234234");
           
            //act
            var isValid = hospital.Validate();

            //assert
            Assert.True(isValid);
        }
    }
}
