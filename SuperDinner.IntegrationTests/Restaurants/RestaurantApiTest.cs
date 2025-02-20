using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using System.Net;
using System.Net.Http.Json;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class RestaurantApiTest : BaseRestaurantTest, IClassFixture<ApiFixture>, IDisposable
    {
        private const string baseUrlRestaurants = "https://localhost:7064/v1/restaurants/";
        private readonly HttpClient _httpClient;
        public RestaurantApiTest(ApiFixture apiFixture)
            => _httpClient = apiFixture.CreateClient();

        private async Task<Restaurant?> CreateRestaurantAsync(CreateRestaurantRequest createRestaurantRequest)
        {
            HttpResponseMessage restaurantCreated = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            restaurantCreated.IsSuccessStatusCode.ShouldBeTrue();
            restaurantCreated.StatusCode.ShouldBe(HttpStatusCode.Created);
            return await restaurantCreated.Content.ReadFromJsonAsync<Restaurant?>();
        }

        [Fact]
        public async Task Get_All_Restaurants_Should_Return_Paged_Restaurants()
        {
            #region Arrange
            IReadOnlyList<CreateRestaurantRequest> createRestaurantRequests = [.. _fakeCreateRestaurantRequest.Generate(5)];
            createRestaurantRequests.ShouldNotBeNull();
            createRestaurantRequests.Count.ShouldBeGreaterThanOrEqualTo(5);

            foreach (CreateRestaurantRequest createRestaurantRequest in createRestaurantRequests)
                await CreateRestaurantAsync(createRestaurantRequest);

            const int pageNumber = 1;
            const int pageSize = 10;
            string getAllRestaurantsUrl = $"{baseUrlRestaurants}?pageNumber={pageNumber}&pageSize={pageSize}";
            #endregion

            #region Act
            HttpResponseMessage response = await _httpClient.GetAsync(getAllRestaurantsUrl);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            #endregion

            #region Assert
            PagedResponse<IReadOnlyList<Restaurant>> pagedRestaurants = await response.Content.ReadFromJsonAsync<PagedResponse<IReadOnlyList<Restaurant>>>();
            pagedRestaurants.ShouldNotBeNull();
            pagedRestaurants.IsSuccess.ShouldBeTrue();
            pagedRestaurants.CurrentPage.ShouldBe(pageNumber);
            pagedRestaurants.PageSize.ShouldBe(pageSize);
            pagedRestaurants.Data.ShouldNotBeNull();
            pagedRestaurants.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Create_Restaurant_Should_Return_Created_Given_Valid_Restaurant()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            #endregion

            #region Act
            Restaurant restaurantCreated = await CreateRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            restaurantCreated.ShouldNotBeNull();
            restaurantCreated.RestaurantId.ShouldNotBe(Guid.Empty);
            restaurantCreated.Name.ShouldNotBeNull();
            restaurantCreated.Description.ShouldNotBeNull();
            restaurantCreated.ContactPhone.ShouldNotBeNull();
            restaurantCreated.Address.ShouldNotBeNull();
            restaurantCreated.Country.ShouldNotBeNull();
            restaurantCreated.Latitude.ShouldBeInRange(-90, 90);
            restaurantCreated.Longitude.ShouldBeInRange(-180, 180);
            restaurantCreated.Price.ShouldBeGreaterThanOrEqualTo(0);
            restaurantCreated.ClientsLimit.ShouldBeGreaterThan(0);
            restaurantCreated.CreatedDate.ShouldNotBe(DateTime.MinValue);
            restaurantCreated.LastModifiedDate.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Create_Restaurant_Should_Return_Bad_Request_Given_Invalid_Restaurant()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            #endregion

            #region Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            #endregion

            #region Assert
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            #endregion
        }

        [Fact]
        public async Task Get_Restaurant_By_Id_Should_Return_Ok_Given_Valid_Guid()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            Restaurant restaurantCreated = await CreateRestaurantAsync(createRestaurantRequest);
            restaurantCreated.ShouldNotBeNull();

            string getRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantCreated.RestaurantId}";
            #endregion

            #region Act
            HttpResponseMessage responseAfterGetRestaurant = await _httpClient.GetAsync(getRestaurantByIdUrl);
            responseAfterGetRestaurant.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterGetRestaurant.StatusCode.ShouldBe(HttpStatusCode.OK);

            Restaurant restaurantAfterGetById = await responseAfterGetRestaurant.Content.ReadFromJsonAsync<Restaurant>();
            #endregion

            #region Assert
            restaurantAfterGetById.ShouldNotBeNull();
            restaurantAfterGetById.RestaurantId.ShouldNotBe(Guid.Empty);
            restaurantAfterGetById.Name.ShouldNotBeNull();
            restaurantAfterGetById.Description.ShouldNotBeNull();
            restaurantAfterGetById.ContactPhone.ShouldNotBeNull();
            restaurantAfterGetById.Address.ShouldNotBeNull();
            restaurantAfterGetById.Country.ShouldNotBeNull();
            restaurantAfterGetById.Latitude.ShouldBeInRange(-90, 90);
            restaurantAfterGetById.Longitude.ShouldBeInRange(-180, 180);
            restaurantAfterGetById.Price.ShouldBeGreaterThanOrEqualTo(0);
            restaurantAfterGetById.ClientsLimit.ShouldBeGreaterThan(0);
            restaurantAfterGetById.CreatedDate.ShouldNotBe(DateTime.MinValue);
            restaurantAfterGetById.LastModifiedDate.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Get_Restaurant_By_Id_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            #region Arrange
            Guid restaurantId = Guid.NewGuid();

            string getRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantId}";
            #endregion

            #region Act
            HttpResponseMessage responseAfterGetRestaurant = await _httpClient.GetAsync(getRestaurantByIdUrl);
            #endregion

            #region Assert
            responseAfterGetRestaurant.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterGetRestaurant.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            #endregion
        }


        [Fact]
        public async Task Update_Restaurant_Should_Return_Ok_Given_Valid_Restaurant()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            Restaurant restaurantCreated = await CreateRestaurantAsync(createRestaurantRequest);

            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            updateRestaurantRequest.RestaurantId = restaurantCreated.RestaurantId;
            updateRestaurantRequest.Name = "Name updated";
            updateRestaurantRequest.Description = "Description updated";
            updateRestaurantRequest.ContactPhone = restaurantCreated.ContactPhone;
            updateRestaurantRequest.Address = "Address updated";
            updateRestaurantRequest.Country = "Country updated";
            updateRestaurantRequest.Latitude = restaurantCreated.Latitude;
            updateRestaurantRequest.Longitude = restaurantCreated.Longitude;
            updateRestaurantRequest.Price = restaurantCreated.Price;
            updateRestaurantRequest.ClientsLimit = restaurantCreated.ClientsLimit;
            #endregion

            #region Act
            HttpResponseMessage responseAfterUpdate = await _httpClient.PutAsJsonAsync(baseUrlRestaurants, updateRestaurantRequest);
            responseAfterUpdate.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterUpdate.StatusCode.ShouldBe(HttpStatusCode.OK);
            #endregion

            #region Assert
            Restaurant restaurantAfterUpdate = await responseAfterUpdate.Content.ReadFromJsonAsync<Restaurant>();
            restaurantAfterUpdate.ShouldNotBeNull();
            restaurantAfterUpdate.Description.ShouldNotBe(restaurantCreated.Description);
            restaurantAfterUpdate.Address.ShouldNotBe(restaurantCreated.Address);
            restaurantAfterUpdate.Country.ShouldNotBe(restaurantCreated.Country);
            #endregion
        }

        [Fact]
        public async Task Update_Restaurant_Should_Return_Bad_Request_Given_Invalid_Restaurant()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            #endregion

            #region Act
            HttpResponseMessage responseAfterUpdate = await _httpClient.PutAsJsonAsync(baseUrlRestaurants, updateRestaurantRequest);
            #endregion

            #region Assert
            responseAfterUpdate.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterUpdate.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            #endregion
        }

        [Fact]
        public async Task Delete_Restaurant_Should_Return_No_Content_Given_Valid_Guid()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            Restaurant restaurantCreated = await CreateRestaurantAsync(createRestaurantRequest);
            restaurantCreated.ShouldNotBeNull();

            string deleteRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantCreated.RestaurantId}";
            #endregion

            #region Act
            HttpResponseMessage responseAfterDelete = await _httpClient.DeleteAsync(deleteRestaurantByIdUrl);
            #endregion

            #region Assert
            responseAfterDelete.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterDelete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            #endregion
        }

        [Fact]
        public async Task Delete_Restaurant_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            #region Arrange
            Guid restaurantId = Guid.NewGuid();

            string deleteRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantId}";
            #endregion

            #region Act
            HttpResponseMessage responseAfterDelete = await _httpClient.DeleteAsync(deleteRestaurantByIdUrl);
            #endregion

            #region Assert
            responseAfterDelete.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterDelete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            #endregion
        }

        public void Dispose()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.CleanDatabaseForTests();
        }
    }
}