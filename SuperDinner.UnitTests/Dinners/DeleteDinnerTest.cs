using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Dinners
{
    public sealed class DeleteDinnerTest : BaseDinnerTest, IDisposable
    {
        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            DeleteDinnerRequest deleteDinnerRequest = new DeleteDinnerRequest(Guid.NewGuid());

            Dinner dinner = _fakeDinner.Generate();

            Response<Dinner> dinnerResponse = new Response<Dinner>(dinner, StatusCodes.Status204NoContent);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeTrue();
            dinnerResponse.Data.ShouldNotBeNull();
            dinnerResponse.Messages.ShouldBeNull();

            _mockDinnerHandler.Setup(x => x.DeleteDinnerAsync(deleteDinnerRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerDeletedResponse = await _mockDinnerHandler.Object.DeleteDinnerAsync(deleteDinnerRequest);
            #endregion

            #region Assert
            dinnerDeletedResponse.ShouldNotBeNull();
            dinnerDeletedResponse.IsSuccess.ShouldBeTrue();
            dinnerDeletedResponse.Data.ShouldNotBeNull();
            dinnerDeletedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_Not_Found()
        {
            #region Arrange
            DeleteDinnerRequest deleteDinnerRequest = new DeleteDinnerRequest(Guid.NewGuid());

            Response<Dinner> dinnerResponse = new Response<Dinner>(null, StatusCodes.Status404NotFound, ["Restaurant not found"]);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeFalse();
            dinnerResponse.Data.ShouldBeNull();
            dinnerResponse.Messages.ShouldNotBeNull();

            _mockDinnerHandler.Setup(x => x.DeleteDinnerAsync(deleteDinnerRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerDeletedResponse = await _mockDinnerHandler.Object.DeleteDinnerAsync(deleteDinnerRequest);
            #endregion

            #region Assert
            dinnerDeletedResponse.ShouldNotBeNull();
            dinnerDeletedResponse.IsSuccess.ShouldBeFalse();
            dinnerDeletedResponse.Data.ShouldBeNull();
            dinnerDeletedResponse.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose() => _mockDinnerHandler.VerifyAll();
    }
}
