using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class DeleteDinnerTest : BaseDinnerTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IDinnerHandler _dinnerHandler;

        public DeleteDinnerTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _dinnerHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IDinnerHandler>();

        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
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
            DeleteDinnerRequest deleteDinnerRequest = new DeleteDinnerRequest(dinnerCreatedResponse.Data.DinnerId);

            Response<Dinner> dinnerDeletedResponse = await _dinnerHandler.DeleteDinnerAsync(deleteDinnerRequest);
            #endregion

            #region Assert
            dinnerDeletedResponse.ShouldNotBeNull();
            dinnerDeletedResponse.IsSuccess.ShouldBeTrue();
            dinnerDeletedResponse.Data.ShouldNotBeNull();
            dinnerDeletedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Guid_Should_Return_Failure()
        {
            #region Arrange
            DeleteDinnerRequest deleteDinnerRequest = new DeleteDinnerRequest(Guid.NewGuid());
            #endregion

            #region Act
            Response<Dinner> dinnerDeletedResponse = await _dinnerHandler.DeleteDinnerAsync(deleteDinnerRequest);
            #endregion

            #region Assert
            dinnerDeletedResponse.ShouldNotBeNull();
            dinnerDeletedResponse.IsSuccess.ShouldBeFalse();
            dinnerDeletedResponse.Data.ShouldBeNull();
            dinnerDeletedResponse.Messages.ShouldNotBeNull();
            #endregion
        }
    }
}
