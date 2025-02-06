using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using System.Net;
using System.Net.Http.Json;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class RestaurantApiTest : BaseRestaurantTest, IClassFixture<ApiFixture>
    {
        private const string baseUrlRestaurants = "https://localhost:7064/v1/restaurants/";
        private readonly HttpClient _httpClient;
        public RestaurantApiTest(ApiFixture apiFixture)
            => _httpClient = apiFixture.CreateClient();

        [Fact]
        public async Task Get_All_Restaurants_Should_Return_Paged_Restaurants()
        {
            List<CreateRestaurantRequest> createRestaurantRequests = _fakeCreateRestaurantRequest.Generate(5).ToList();
            createRestaurantRequests.ShouldNotBeNull();
            createRestaurantRequests.Count.ShouldBeGreaterThanOrEqualTo(5);

            createRestaurantRequests.ForEach(async restaurant =>
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, restaurant);
                response.IsSuccessStatusCode.ShouldBeTrue();
                response.StatusCode.ShouldBe(HttpStatusCode.Created);
            });

            const int pageNumber = 1;
            const int pageSize = 10;
            string getAllRestaurantsUrl = $"{baseUrlRestaurants}?pageNumber={pageNumber}&pageSize={pageSize}";

            HttpResponseMessage response = await _httpClient.GetAsync(getAllRestaurantsUrl);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            PagedResponse<List<Restaurant>> pagedRestaurants = await response.Content.ReadFromJsonAsync<PagedResponse<List<Restaurant>>>();
            pagedRestaurants.ShouldNotBeNull();
            pagedRestaurants.IsSuccess.ShouldBeTrue();
            pagedRestaurants.CurrentPage.ShouldBe(pageNumber);
            pagedRestaurants.PageSize.ShouldBe(pageSize);
            pagedRestaurants.Data.ShouldNotBeNull();
            pagedRestaurants.Messages.ShouldBeNull();
        }

        [Fact]
        public async Task Create_Restaurant_Should_Return_Created_Given_Valid_Restaurant()
        {
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            Restaurant restaurantCreated = await response.Content.ReadFromJsonAsync<Restaurant>();
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
            restaurantCreated.CreatedDate.ShouldNotBe(new DateTime());
            restaurantCreated.LastModifiedDate.ShouldBeNull();
        }

        [Fact]
        public async Task Create_Restaurant_Should_Return_Bad_Request_Given_Invalid_Restaurant()
        {
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_Restaurant_By_Id_Should_Return_Ok_Given_Valid_Guid()
        {
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            Restaurant restaurantCreated = await response.Content.ReadFromJsonAsync<Restaurant>();
            restaurantCreated.ShouldNotBeNull();

            string getRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantCreated.RestaurantId}";

            HttpResponseMessage responseAfterGetRestaurant = await _httpClient.GetAsync(getRestaurantByIdUrl);
            responseAfterGetRestaurant.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterGetRestaurant.StatusCode.ShouldBe(HttpStatusCode.OK);

            Restaurant restaurantAfterGetById = await responseAfterGetRestaurant.Content.ReadFromJsonAsync<Restaurant>();

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
            restaurantAfterGetById.CreatedDate.ShouldNotBe(new DateTime());
            restaurantAfterGetById.LastModifiedDate.ShouldBeNull();
        }

        [Fact]
        public async Task Get_Restaurant_By_Id_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            Guid restaurantId = Guid.NewGuid();

            string getRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantId}";

            HttpResponseMessage responseAfterGetRestaurant = await _httpClient.GetAsync(getRestaurantByIdUrl);
            responseAfterGetRestaurant.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterGetRestaurant.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Update_Restaurant_Should_Return_Ok_Given_Valid_Restaurant()
        {
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            Restaurant restaurantCreated = await response.Content.ReadFromJsonAsync<Restaurant>();
            restaurantCreated.Price = 10;

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

            HttpResponseMessage responseAfterUpdate = await _httpClient.PutAsJsonAsync(baseUrlRestaurants, updateRestaurantRequest);
            responseAfterUpdate.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterUpdate.StatusCode.ShouldBe(HttpStatusCode.OK);

            Restaurant restaurantAfterUpdate = await responseAfterUpdate.Content.ReadFromJsonAsync<Restaurant>();
            restaurantAfterUpdate.ShouldNotBeNull();
            restaurantAfterUpdate.Description.ShouldNotBe(restaurantCreated.Description);
            restaurantAfterUpdate.Address.ShouldNotBe(restaurantCreated.Address);
            restaurantAfterUpdate.Country.ShouldNotBe(restaurantCreated.Country);
        }

        [Fact]
        public async Task Update_Restaurant_Should_Return_Bad_Request_Given_Invalid_Restaurant()
        {
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();

            HttpResponseMessage responseAfterUpdate = await _httpClient.PutAsJsonAsync(baseUrlRestaurants, updateRestaurantRequest);
            responseAfterUpdate.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterUpdate.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_Restaurant_Should_Return_No_Content_Given_Valid_Guid()
        {
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrlRestaurants, createRestaurantRequest);
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            Restaurant restaurantCreated = await response.Content.ReadFromJsonAsync<Restaurant>();
            restaurantCreated.ShouldNotBeNull();

            string deleteRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantCreated.RestaurantId}";
            HttpResponseMessage responseAfterDelete = await _httpClient.DeleteAsync(deleteRestaurantByIdUrl);
            responseAfterDelete.IsSuccessStatusCode.ShouldBeTrue();
            responseAfterDelete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Restaurant_Should_Return_Not_Found_Given_Invalid_Guid()
        {
            Guid restaurantId = Guid.NewGuid();

            string deleteRestaurantByIdUrl = $"{baseUrlRestaurants}{restaurantId}";

            HttpResponseMessage responseAfterDelete = await _httpClient.DeleteAsync(deleteRestaurantByIdUrl);
            responseAfterDelete.IsSuccessStatusCode.ShouldBeFalse();
            responseAfterDelete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
