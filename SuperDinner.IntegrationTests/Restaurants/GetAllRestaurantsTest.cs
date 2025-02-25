﻿using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class GetAllRestaurantsTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IRestaurantHandler _restaurantHandler;

        public GetAllRestaurantsTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _restaurantHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();

        [Fact]
        public async Task Get_All_Restaurants_Should_Return_Success()
        {
            IReadOnlyList<CreateRestaurantRequest> createRestaurantRequests = [.. _fakeCreateRestaurantRequest.Generate(20)];

            foreach (CreateRestaurantRequest createRestaurantRequest in createRestaurantRequests)
                await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);

            GetAllRestaurantsRequest getAllRestaurantsRequest = new GetAllRestaurantsRequest();
            getAllRestaurantsRequest.PageNumber = 1;
            getAllRestaurantsRequest.PageSize = 10;

            PagedResponse<IReadOnlyList<Restaurant>> responseRestaurantsPageOne = await _restaurantHandler.GetAllRestaurantsAsync(getAllRestaurantsRequest);

            getAllRestaurantsRequest.PageNumber = 2;

            PagedResponse<IReadOnlyList<Restaurant>> responseRestaurantsPageTwo = await _restaurantHandler.GetAllRestaurantsAsync(getAllRestaurantsRequest);

            responseRestaurantsPageOne.ShouldNotBeNull();
            responseRestaurantsPageOne.IsSuccess.ShouldBeTrue();
            responseRestaurantsPageOne.CurrentPage.ShouldBe(1);
            responseRestaurantsPageOne.PageSize.ShouldBe(10);
            responseRestaurantsPageOne.Data.ShouldNotBeNull();
            responseRestaurantsPageOne.Data.Count.ShouldBe(10);
            responseRestaurantsPageOne.Messages.ShouldBeNull();

            responseRestaurantsPageTwo.ShouldNotBeNull();
            responseRestaurantsPageTwo.IsSuccess.ShouldBeTrue();
            responseRestaurantsPageTwo.CurrentPage.ShouldBe(2);
            responseRestaurantsPageTwo.PageSize.ShouldBe(10);
            responseRestaurantsPageTwo.Data.ShouldNotBeNull();
            responseRestaurantsPageTwo.Data.Count.ShouldBe(10);
            responseRestaurantsPageTwo.Messages.ShouldBeNull();
        }
    }
}
