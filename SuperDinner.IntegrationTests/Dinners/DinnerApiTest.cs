using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;
using System.Net;
using System.Net.Http.Json;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class DinnerApiTest : BaseDinnerTest, IClassFixture<ApiFixture>
    {
        private const string baseUrlDinners = "https://localhost:7064/v1/dinners/";
        private readonly HttpClient _httpClient;

        public DinnerApiTest(ApiFixture apiFixture)
            => _httpClient = apiFixture.CreateClient();

        private async Task<Dinner?> CreateDinnerAsync(CreateDinnerRequest createDinnerRequest)
        {
            HttpResponseMessage dinnerCreated = await _httpClient.PostAsJsonAsync(baseUrlDinners, createDinnerRequest);
            dinnerCreated.IsSuccessStatusCode.ShouldBeTrue();
            dinnerCreated.StatusCode.ShouldBe(HttpStatusCode.Created);

            return await dinnerCreated.Content.ReadFromJsonAsync<Dinner?>();
        }
     
        [Fact]
        public async Task Get_All_Dinners_Should_Return_Paged_Dinners()
        {
            #region Arrange
            IReadOnlyCollection<CreateDinnerRequest> createDinnerRequests = [.. _fakeCreateDinnerRequest.Generate(5)];
            createDinnerRequests.ShouldNotBeNull();
            createDinnerRequests.Count.ShouldBeGreaterThanOrEqualTo(5);

            foreach (CreateDinnerRequest createDinnerRequest in createDinnerRequests)
                await CreateDinnerAsync(createDinnerRequest);

            const int pageNumber = 1;
            const int pageSize = 10;
            string getAllDinnersUrl = $"{baseUrlDinners}?pageNumber={pageNumber}&pageSize={pageSize}";
            #endregion

            #region Act
            HttpResponseMessage getAllDinnersResponse = await _httpClient.GetAsync(getAllDinnersUrl);
            getAllDinnersResponse.IsSuccessStatusCode.ShouldBeTrue();
            getAllDinnersResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            #endregion

            #region Assert
            PagedResponse<IReadOnlyList<Dinner>> pagedDinners = await getAllDinnersResponse.Content.ReadFromJsonAsync<PagedResponse<IReadOnlyList<Dinner>>>();

            pagedDinners.ShouldNotBeNull();
            pagedDinners.IsSuccess.ShouldBeTrue();
            pagedDinners.CurrentPage.ShouldBe(pageNumber);
            pagedDinners.PageSize.ShouldBe(pageSize);
            pagedDinners.Data.ShouldNotBeNull();
            pagedDinners.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Create_Dinner_Should_Return_Created_Given_Valid_Dinner()
        {
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlDinners, createDinnerRequest);
            
            Dinner dinnerCreated = await CreateDinnerAsync(createDinnerRequest);
            dinnerCreated.ShouldNotBeNull();
            dinnerCreated.RestaurantId.ShouldNotBe(Guid.Empty);
            dinnerCreated.UserId.ShouldNotBe(Guid.Empty);
            dinnerCreated.TransactionId.ShouldNotBe(Guid.Empty);
            dinnerCreated.DinnerDate.ShouldNotBe(DateTime.MinValue);
            dinnerCreated.DinnerStatus.ShouldBe(DinnerStatus.WaitingForPayment);
            dinnerCreated.CreatedDate.ShouldNotBe(DateTime.MinValue);
            dinnerCreated.LastModifiedDate.ShouldBeNull();
        }

        [Fact]
        public async Task Create_Dinner_Should_Return_Bad_Request_Given_Invalid_Dinner()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = new CreateDinnerRequest();
            #endregion

            #region Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlDinners, createDinnerRequest);
            #endregion

            #region Assert
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            #endregion
        }

        [Fact]
        public async Task Get_Dinner_By_Id_Should_Return_Dinner_Given_Existing_Id()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();

            Dinner dinnerCreated = await CreateDinnerAsync(createDinnerRequest);
            dinnerCreated.ShouldNotBeNull();

            string getRestaurantByIdUrl = $"{baseUrlDinners}{dinnerCreated.DinnerId}";
            #endregion

            #region Act
            HttpResponseMessage getDinnerByIdResponse = await _httpClient.GetAsync(getRestaurantByIdUrl);
            getDinnerByIdResponse.IsSuccessStatusCode.ShouldBeTrue();
            getDinnerByIdResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            Dinner dinnerAfterGetById = await getDinnerByIdResponse.Content.ReadFromJsonAsync<Dinner>();
            #endregion

            #region Assert
            dinnerAfterGetById.ShouldNotBeNull();
            dinnerAfterGetById.RestaurantId.ShouldNotBe(Guid.Empty);
            dinnerAfterGetById.UserId.ShouldNotBe(Guid.Empty);
            dinnerAfterGetById.TransactionId.ShouldNotBe(Guid.Empty);
            dinnerAfterGetById.DinnerDate.ShouldNotBe(DateTime.MinValue);
            dinnerAfterGetById.DinnerStatus.ShouldBe(DinnerStatus.WaitingForPayment);
            dinnerAfterGetById.CreatedDate.ShouldNotBe(DateTime.MinValue);
            dinnerAfterGetById.LastModifiedDate.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Get_Dinner_By_Id_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            #region Arrange
            Guid dinnerId = Guid.NewGuid();
            string getDinnerByIdUrl = $"{baseUrlDinners}{dinnerId}";
            #endregion

            #region Act
            HttpResponseMessage getDinnerByIdResponse = await _httpClient.GetAsync(getDinnerByIdUrl);
            #endregion

            #region Assert
            getDinnerByIdResponse.IsSuccessStatusCode.ShouldBeFalse();
            getDinnerByIdResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            #endregion
        }

        [Fact]
        public async Task Update_Dinner_Should_Return_Ok_Given_Valid_Dinner()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();

            Dinner dinnerCreated = await CreateDinnerAsync(createDinnerRequest);

            UpdateDinnerRequest updateDinnerRequest = new UpdateDinnerRequest();
            updateDinnerRequest.DinnerId = dinnerCreated.DinnerId;
            updateDinnerRequest.DinnerDate = dinnerCreated.DinnerDate.AddDays(1);
            updateDinnerRequest.UserId = dinnerCreated.UserId;
            updateDinnerRequest.RestaurantId = dinnerCreated.RestaurantId;
            updateDinnerRequest.TransactionId = dinnerCreated.TransactionId;
            updateDinnerRequest.DinnerStatus = DinnerStatus.Confirmed;
            updateDinnerRequest.LastModifiedDate = DateTime.Now;
            #endregion

            #region Act
            HttpResponseMessage responseAfterDinnerUpdate = await _httpClient.PutAsJsonAsync(baseUrlDinners, updateDinnerRequest);
            responseAfterDinnerUpdate.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterDinnerUpdate.StatusCode.ShouldBe(HttpStatusCode.OK);
            #endregion

            #region Assert
            Dinner dinnerAfterUpdate = await responseAfterDinnerUpdate.Content.ReadFromJsonAsync<Dinner>();
            dinnerAfterUpdate.ShouldNotBeNull();
            dinnerAfterUpdate.DinnerDate.ShouldBe(updateDinnerRequest.DinnerDate);
            dinnerAfterUpdate.DinnerStatus.ShouldBe(updateDinnerRequest.DinnerStatus);
            #endregion
        }

        [Fact]
        public async Task Update_Dinner_Should_Return_Bad_Request_Given_Invalid_Dinner()
        {
            #region Arrange
            UpdateDinnerRequest updateDinnerRequest = new UpdateDinnerRequest();
            #endregion

            #region Act
            HttpResponseMessage responseAfterDinnerUpdate = await _httpClient.PutAsJsonAsync(baseUrlDinners, updateDinnerRequest);
            #endregion

            #region Assert
            responseAfterDinnerUpdate.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterDinnerUpdate.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            #endregion
        }

        [Fact]
        public async Task Delete_Dinner_Should_Return_No_Content_Given_Valid_Guid()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();

            Dinner dinnerCreated = await CreateDinnerAsync(createDinnerRequest);
            dinnerCreated.ShouldNotBeNull();

            string deleteDinnerByIdUrl = $"{baseUrlDinners}{dinnerCreated.DinnerId}";
            #endregion

            #region Act
            HttpResponseMessage deleteDinnerByIdResponse = await _httpClient.DeleteAsync(deleteDinnerByIdUrl);
            #endregion

            #region Assert
            deleteDinnerByIdResponse.IsSuccessStatusCode.ShouldBeTrue();
            deleteDinnerByIdResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            #endregion            
        }

        [Fact]
        public async Task Delete_Dinner_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            #region Arrange
            Guid dinnerId = Guid.NewGuid();
            string deleteDinnerByIdUrl = $"{baseUrlDinners}{dinnerId}";
            #endregion

            #region Act
            HttpResponseMessage deleteDinnerByIdResponse = await _httpClient.DeleteAsync(deleteDinnerByIdUrl);
            #endregion

            #region Assert
            deleteDinnerByIdResponse.IsSuccessStatusCode.ShouldBeFalse();
            deleteDinnerByIdResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            #endregion
        }
    }
}
