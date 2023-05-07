using FluentAssertions;
using MyDocAppointment.API.Features.Medications;
using System.Net.Http.Json;

namespace MyDocAppointment.Tests.ApiTests
{
    public class MedicationsControllerTests : BaseIntegrationTests<MedicationsController>
    {
        private const string ApiURL = "v1/api/Medications";

        public MedicationsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void When_CreatedMedication_Then_ShouldReturnMedicationInTheGetRequest()
        {
            // Arrange
            CreateMedicationDto createMedicationDto = CreateSUT();
            // Act
            var createMedicationResponse = await HttpClient.PostAsJsonAsync(ApiURL, createMedicationDto);
            var getMedicationResult = await HttpClient.GetAsync(ApiURL);
            // Assert
            createMedicationResponse.EnsureSuccessStatusCode();
            createMedicationResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getMedicationResult.EnsureSuccessStatusCode();
            var doctors = await getMedicationResult.Content.ReadFromJsonAsync<List<MedicationDto>>();
            doctors.Should().HaveCount(1);
            doctors.Should().NotBeNull();
        }

        [Fact]
        public async void When_DeletedMedication_Then_ShouldReturnNoMedicationInTheGetRequest()
        {
            // Arrange
            CreateMedicationDto createMedicationDto = CreateSUT();
            var createMedicationResponse = await HttpClient.PostAsJsonAsync(ApiURL, createMedicationDto);
            var medication = await createMedicationResponse.Content.ReadFromJsonAsync<MedicationDto>();

            medication.Should().NotBeNull();

            if (medication != null)
            {
                // Act
                var resultResponse = await HttpClient.DeleteAsync
                    ($"{ApiURL}/{medication.Id}");

                // Assert
                resultResponse.EnsureSuccessStatusCode();
                resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }

        private static CreateMedicationDto CreateSUT()
        {
            return new CreateMedicationDto
            {
                Name = "Paracetamol",
                Stock = 21,
                Unit = "capsule",
                Capacity= 1,
                Price= 1
            };
        }

    }

}
