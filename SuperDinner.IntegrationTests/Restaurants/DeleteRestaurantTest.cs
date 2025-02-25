﻿using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class DeleteRestaurantTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IRestaurantHandler _restaurantHandler;

        public DeleteRestaurantTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _restaurantHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();

        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            responseRestaurantCreated.ShouldNotBeNull();
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBeNull();
            responseRestaurantCreated.Messages.ShouldBeNull();

            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(responseRestaurantCreated.Data.RestaurantId);

            Response<Restaurant> responseRestaurantDeleted = await _restaurantHandler.DeleteRestaurantAsync(deleteRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantDeleted.ShouldNotBeNull();
            responseRestaurantDeleted.IsSuccess.ShouldBeTrue();
            responseRestaurantDeleted.Data.ShouldNotBeNull();
            responseRestaurantDeleted.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Guid_Should_Return_Failure()
        {
            #region Arrange
            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(Guid.Empty);
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantDeleted = await _restaurantHandler.DeleteRestaurantAsync(deleteRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantDeleted.ShouldNotBeNull();
            responseRestaurantDeleted.IsSuccess.ShouldBeFalse();
            responseRestaurantDeleted.Data.ShouldBeNull();
            responseRestaurantDeleted.Messages.ShouldNotBeNull();
            responseRestaurantDeleted.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }       
    }
}