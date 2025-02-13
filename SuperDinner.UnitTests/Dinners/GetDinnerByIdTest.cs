using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Dinners
{
    public sealed class GetDinnerByIdTest : BaseDinnerTest, IDisposable
    {
        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            GetDinnerByIdRequest getDinnerByIdRequest = new GetDinnerByIdRequest(Guid.NewGuid());

            Dinner dinner = _fakeDinner.Generate();

            Response<Dinner> dinnerResponse = new Response<Dinner>(dinner, StatusCodes.Status200OK);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeTrue();
            dinnerResponse.Data.ShouldNotBeNull();
            dinnerResponse.Messages.ShouldBeNull();

            _mockDinnerHandler.Setup(x => x.GetDinnerByIdAsync(getDinnerByIdRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerByIdResponse = await _mockDinnerHandler.Object.GetDinnerByIdAsync(getDinnerByIdRequest);
            #endregion

            #region Assert
            dinnerByIdResponse.ShouldNotBeNull();
            dinnerByIdResponse.IsSuccess.ShouldBeTrue();
            dinnerByIdResponse.Data.ShouldNotBeNull();
            dinnerByIdResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_False()
        {
            #region Arrange
            GetDinnerByIdRequest getDinnerByIdRequest = new GetDinnerByIdRequest(Guid.NewGuid());

            Response<Dinner> dinnerResponse = new Response<Dinner>(null, StatusCodes.Status404NotFound, ["Dinner not found"]);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeFalse();
            dinnerResponse.Data.ShouldBeNull();
            dinnerResponse.Messages.ShouldNotBeNull();

            _mockDinnerHandler.Setup(x => x.GetDinnerByIdAsync(getDinnerByIdRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerByIdResponse = await _mockDinnerHandler.Object.GetDinnerByIdAsync(getDinnerByIdRequest);
            #endregion

            #region Assert
            dinnerByIdResponse.ShouldNotBeNull();
            dinnerByIdResponse.IsSuccess.ShouldBeFalse();
            dinnerByIdResponse.Data.ShouldBeNull();
            dinnerByIdResponse.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose() => _mockDinnerHandler.VerifyAll();
    }
}
