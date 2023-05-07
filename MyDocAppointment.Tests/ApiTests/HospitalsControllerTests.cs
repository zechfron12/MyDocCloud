using FluentAssertions;
using MyDocAppointment.API.Features.Doctors;
using MyDocAppointment.API.Features.Hospitals;
using System.Net.Http.Json;

namespace MyDocAppointment.Tests.ApiTests
{
    public class HospitalsControllerTests : BaseIntegrationTests<HospitalsController>
    {
        private const string ApiURL = "v1/api/Hospitals";

        public HospitalsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void When_CreatedHospital_Then_ShouldReturnHospitalInTheGetRequest()
        {
            // Arrange
            CreateHospitalDto createHospitalDto = CreateSUT();

            // Act
            var createHospitalResponse = await HttpClient.PostAsJsonAsync(ApiURL, createHospitalDto);
            var getHospitalResult = await HttpClient.GetAsync(ApiURL);

            // Assert
            createHospitalResponse.EnsureSuccessStatusCode();
            createHospitalResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getHospitalResult.EnsureSuccessStatusCode();
            var hospitals = await getHospitalResult.Content.ReadFromJsonAsync<List<HospitalDto>>();
            hospitals.Should().HaveCount(1);
            hospitals.Should().NotBeNull();
        }


        [Fact]
        public async void When_RegisterDoctorsToHospital_Then_ShouldReturnDoctorsInTheGetRequest()
        {
            // Arrange
            CreateHospitalDto createHospitalDto = CreateSUT();
            var createHospitalResponse = await HttpClient.PostAsJsonAsync(ApiURL, createHospitalDto);

            var doctors = new List<CreateDoctorDto>
            {
                new CreateDoctorDto
                {
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Specialization = "Dermatology",
                    Email = "doctor@gmail.com",
                    Phone = "1234567890",
                    Title = "doctor  docent",
                    Profession = "--",
                    Location = "Bosnia",
                    Grade = 9,
                    Reviews = 10
                },
                new CreateDoctorDto
                {
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Specialization = "Dermatology",
                    Email = "doctor@gmail.com",
                    Phone = "1234567890",
                    Title = "doctor  docent",
                    Profession = "--",
                    Location = "Bosnia",
                    Grade = 9,
                    Reviews = 10
                }
            };
            var hospital = await createHospitalResponse.Content.ReadFromJsonAsync<HospitalDto>();


            hospital.Should().NotBeNull();

            if (hospital != null)
            {
                // Act
                var resultResponse = await HttpClient.PostAsJsonAsync
                    ($"{ApiURL}/{hospital.Id}/doctors", doctors);

                // Assert
                resultResponse.EnsureSuccessStatusCode();
                resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }

        [Fact] 
        public async void When_DeletedHospital_Then_ShouldReturnNoHospitalInTheGetRequest()
        {
            // Arrange
            CreateHospitalDto createHospitalDto = CreateSUT();
            var createHostpitalResponse = await HttpClient.PostAsJsonAsync(ApiURL, createHospitalDto);
            var hospital = await createHostpitalResponse.Content.ReadFromJsonAsync<DoctorDto>();

            hospital.Should().NotBeNull();

            if (hospital != null)
            {
                // Act
                var resultResponse = await HttpClient.DeleteAsync
                    ($"{ApiURL}/{hospital.Id}");

                // Assert
                resultResponse.EnsureSuccessStatusCode();
                resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }

        private static CreateHospitalDto CreateSUT()
        {
            return new CreateHospitalDto
            {
                Name = "Regina Maria",
                Address = "Iasi",
                Phone = "0733333333"
            };
        }
    }
}
