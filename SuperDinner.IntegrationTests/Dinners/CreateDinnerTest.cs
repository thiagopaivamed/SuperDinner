using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class CreateDinnerTest : BaseDinnerTest, IClassFixture<DependencyInjectionFixture>, IDisposable
    {
        private readonly IDinnerHandler _dinnerHandler;

        public CreateDinnerTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _dinnerHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IDinnerHandler>();

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();
            createDinnerRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Dinner> dinnerCreatedResponse = await _dinnerHandler.AddDinnerAsync(createDinnerRequest);
            #endregion

            #region Assert
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeTrue();
            dinnerCreatedResponse.Data.ShouldNotBeNull();
            dinnerCreatedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_Failure()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = new CreateDinnerRequest();
            createDinnerRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Dinner> dinnerCreatedResponse = await _dinnerHandler.AddDinnerAsync(createDinnerRequest);
            #endregion

            #region Assert
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeFalse();
            dinnerCreatedResponse.Data.ShouldBeNull();
            dinnerCreatedResponse.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.CleanDatabaseForTests();
        }
    }
}
