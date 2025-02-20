using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class GetDinnerByIdTest : BaseDinnerTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IDinnerHandler _dinnerHandler;

        public GetDinnerByIdTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _dinnerHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IDinnerHandler>();

        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();
            createDinnerRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Dinner> dinnerCreatedResponse = await _dinnerHandler.AddDinnerAsync(createDinnerRequest);
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeTrue();
            dinnerCreatedResponse.Data.ShouldNotBeNull();
            dinnerCreatedResponse.Messages.ShouldBeNull();

            GetDinnerByIdRequest getDinnerByIdRequest = new GetDinnerByIdRequest(dinnerCreatedResponse.Data.DinnerId);

            Response<Dinner> dinnerRetrievedResponse = await _dinnerHandler.GetDinnerByIdAsync(getDinnerByIdRequest);
            #endregion

            #region Assert
            dinnerRetrievedResponse.ShouldNotBeNull();
            dinnerRetrievedResponse.IsSuccess.ShouldBeTrue();
            dinnerRetrievedResponse.Data.ShouldNotBeNull();
            dinnerRetrievedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_Failure()
        {
            #region Arrange
            GetDinnerByIdRequest getDinnerByIdRequest = new GetDinnerByIdRequest(Guid.NewGuid());
            #endregion

            #region Act
            Response<Dinner> dinnerRetrievedResponse = await _dinnerHandler.GetDinnerByIdAsync(getDinnerByIdRequest);
            #endregion

            #region Assert
            dinnerRetrievedResponse.ShouldNotBeNull();
            dinnerRetrievedResponse.IsSuccess.ShouldBeFalse();
            dinnerRetrievedResponse.Data.ShouldBeNull();
            dinnerRetrievedResponse.Messages.ShouldNotBeNull();
            #endregion
        }
    }
}
