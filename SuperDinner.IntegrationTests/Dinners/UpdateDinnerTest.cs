using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class UpdateDinnerTest : BaseDinnerTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IDinnerHandler _dinnerHandler;

        public UpdateDinnerTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _dinnerHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IDinnerHandler>();

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();
            createDinnerRequest.ShouldNotBeNull();

            Response<Dinner> dinnerCreatedResponse = await _dinnerHandler.AddDinnerAsync(createDinnerRequest);
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeTrue();
            dinnerCreatedResponse.Data.ShouldNotBeNull();
            dinnerCreatedResponse.Messages.ShouldBeNull();
            #endregion

            #region Act
            UpdateDinnerRequest updateDinnerRequest = new UpdateDinnerRequest();
            updateDinnerRequest.DinnerId = dinnerCreatedResponse.Data.DinnerId;
            updateDinnerRequest.DinnerDate = dinnerCreatedResponse.Data.DinnerDate;
            updateDinnerRequest.UserId = Guid.NewGuid();
            updateDinnerRequest.RestaurantId = Guid.NewGuid();
            updateDinnerRequest.TransactionId = dinnerCreatedResponse.Data.TransactionId;
            updateDinnerRequest.DinnerStatus = dinnerCreatedResponse.Data.DinnerStatus;

            Response<Dinner> dinnerUpdatedResponse = await _dinnerHandler.UpdateDinnerAsync(updateDinnerRequest);
            #endregion

            #region Assert
            dinnerUpdatedResponse.ShouldNotBeNull();
            dinnerUpdatedResponse.IsSuccess.ShouldBeTrue();
            dinnerUpdatedResponse.Data.ShouldNotBeNull();
            dinnerUpdatedResponse.Data.UserId.ShouldBe(updateDinnerRequest.UserId);
            dinnerUpdatedResponse.Data.RestaurantId.ShouldBe(updateDinnerRequest.RestaurantId);
            dinnerUpdatedResponse.Data.ShouldNotBeNull();
            dinnerUpdatedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_Failure()
        {
            #region Arrange
            UpdateDinnerRequest updateDinnerRequest = new UpdateDinnerRequest();
            updateDinnerRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Dinner> dinnerUpdatedResponse = await _dinnerHandler.UpdateDinnerAsync(updateDinnerRequest);
            #endregion

            #region Assert
            dinnerUpdatedResponse.ShouldNotBeNull();
            dinnerUpdatedResponse.IsSuccess.ShouldBeFalse();
            dinnerUpdatedResponse.Data.ShouldBeNull();
            dinnerUpdatedResponse.Messages.ShouldNotBeNull();
            #endregion
        }     
    }
}
