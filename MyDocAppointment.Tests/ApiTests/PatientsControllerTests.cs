using FluentAssertions;
using MyDocAppointment.API.Features.Patients;
using System.Net.Http.Json;

namespace MyDocAppointment.Tests.ApiTests
{

    public class PatientsControllerTests : BaseIntegrationTests<PatientsController>
    {
        private const string ApiURL = "v1/api/Patients";

        public PatientsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void When_CreatedPatient_Then_ShouldReturnPatientInTheGetRequest()
        {
            // Arrange
            CreatePatientDto createPatientDto = CreateSUT();
            // Act
            var createPatientResponse = await HttpClient.PostAsJsonAsync(ApiURL, createPatientDto);
            var getPatientResult = await HttpClient.GetAsync(ApiURL);
            // Assert
            createPatientResponse.EnsureSuccessStatusCode();
            createPatientResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getPatientResult.EnsureSuccessStatusCode();
            var patients = await getPatientResult.Content.ReadFromJsonAsync<List<PatientDto>>();
            patients.Should().HaveCount(1);
            patients.Should().NotBeNull();
        }

        //[Fact]
        //public async void When_RegisterAppointmentsToHospital_Then_ShouldReturnAppointmentsInTheGetRequest()
        //{
        //    // Arrange
        //    CreatePatientDto createPatientDto = CreateSUT();
        //    var createPatientResponse = await HttpClient.PostAsJsonAsync(ApiURL, createPatientDto);
        //    var patient = await createPatientResponse.Content.ReadFromJsonAsync<PatientDto>();

        //    var appointments = new List<AppointmentsDtoFromPatient>
        //    {
        //        new AppointmentsDtoFromPatient
        //        {
        //            StartTime = DateTime.Now,
        //            EndTime = DateTime.Now,
        //        },
        //        new AppointmentsDtoFromPatient
        //        {
        //            StartTime = DateTime.Now,
        //            EndTime = DateTime.Now,
        //        }
        //    };
        //    // Act
        //    var resultResponse = await HttpClient.PostAsJsonAsync
        //        ($"{ApiURL}/{patient.Id}/appointments", appointments);

        //    // Assert
        //    resultResponse.EnsureSuccessStatusCode();
        //    resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        //}

        [Fact]
        public async void When_DeletedPatient_Then_ShouldReturnNoPatientInTheGetRequest()
        {
            // Arrange
            CreatePatientDto createPatientDto = CreateSUT();
            var createPatientResponse = await HttpClient.PostAsJsonAsync(ApiURL, createPatientDto);
            var patient = await createPatientResponse.Content.ReadFromJsonAsync<PatientDto>();

            patient.Should().NotBeNull();

            if (patient != null)
            {
                // Act
                var resultResponse = await HttpClient.DeleteAsync
                    ($"{ApiURL}/{patient.Id}");

                // Assert
                resultResponse.EnsureSuccessStatusCode();
                resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }

        private static CreatePatientDto CreateSUT()
        {
            return new CreatePatientDto
            {
                FirstName = "Eu",
                LastName = "Tot eu",
                Email = "eu@datoteu.eu",
                Phone = "0712312312",
            };
        }
    }
}
