using Bogus;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators.Dinner;

namespace SuperDinner.UnitTests.Dinners
{
    public sealed class UpdateDinnerTest : BaseDinnerTest, IDisposable
    {
        private readonly Faker<UpdateDinnerRequest> _fakeUpdateDinnerRequest;

        public UpdateDinnerTest()
        {
            _fakeUpdateDinnerRequest = new Faker<UpdateDinnerRequest>()
                .RuleFor(x => x.DinnerId, Guid.NewGuid())
                .RuleFor(x => x.DinnerDate, DateTime.Now)
                .RuleFor(x => x.UserId, Guid.NewGuid())
                .RuleFor(x => x.RestaurantId, Guid.NewGuid())
                .RuleFor(x => x.TransactionId, Guid.NewGuid())
                .RuleFor(x => x.DinnerStatus, DinnerStatus.Confirmed)
                .RuleFor(x => x.LastModifiedDate, DateTime.Now);
        }

        [Fact]
        public async Task Use_Valid_Dinner_Should_Return_True()
        {
            UpdateDinnerRequest request = _fakeUpdateDinnerRequest.Generate();
            UpdateDinnerRequestValidator validator = new UpdateDinnerRequestValidator();

            FluentValidation.Results.ValidationResult dinnerUpdatedResult = await validator.ValidateAsync(request);

            dinnerUpdatedResult.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Use_Invalid_Dinner_Should_Return_False()
        {
            UpdateDinnerRequest request = new UpdateDinnerRequest();
            UpdateDinnerRequestValidator validator = new UpdateDinnerRequestValidator();

            FluentValidation.Results.ValidationResult dinnerUpdatedResult = await validator.ValidateAsync(request);

            dinnerUpdatedResult.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Update_Valid_Dinner_Should_Return_True()
        {
            UpdateDinnerRequest updateDinnerRequest = _fakeUpdateDinnerRequest.Generate();
            updateDinnerRequest.ShouldNotBeNull();

            Dinner dinner = _fakeDinner.Generate();
            dinner.ShouldNotBeNull();

            Response<Dinner> dinnerResponse = new Response<Dinner>(dinner, StatusCodes.Status200OK);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeTrue();
            dinnerResponse.Data.ShouldNotBeNull();
            dinnerResponse.Messages.ShouldBeNull();

            _mockDinnerHandler.Setup(x => x.UpdateDinnerAsync(updateDinnerRequest)).ReturnsAsync(dinnerResponse);

            #region Act
            Response<Dinner> dinnerUpdatedResponse = await _mockDinnerHandler.Object.UpdateDinnerAsync(updateDinnerRequest);
            #endregion

            #region Assert
            dinnerUpdatedResponse.ShouldNotBeNull();
            dinnerUpdatedResponse.IsSuccess.ShouldBeTrue();
            dinnerUpdatedResponse.Data.ShouldNotBeNull();
            dinnerUpdatedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Update_Invalid_Dinner_Should_Return_False()
        {
            UpdateDinnerRequest updateDinnerRequest = new UpdateDinnerRequest();
            updateDinnerRequest.ShouldNotBeNull();

            Dinner dinner = _fakeDinner.Generate();
            dinner.ShouldNotBeNull();

            Response<Dinner> dinnerResponse = new Response<Dinner>(null, StatusCodes.Status400BadRequest, ["Invalid dinner data"]);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeFalse();
            dinnerResponse.Data.ShouldBeNull();
            dinnerResponse.Messages.ShouldNotBeNull();

            _mockDinnerHandler.Setup(x => x.UpdateDinnerAsync(updateDinnerRequest)).ReturnsAsync(dinnerResponse);

            #region Act
            Response<Dinner> dinnerUpdatedResponse = await _mockDinnerHandler.Object.UpdateDinnerAsync(updateDinnerRequest);
            #endregion

            #region Assert
            dinnerUpdatedResponse.ShouldNotBeNull();
            dinnerUpdatedResponse.IsSuccess.ShouldBeFalse();
            dinnerUpdatedResponse.Data.ShouldBeNull();
            dinnerUpdatedResponse.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose() => _mockDinnerHandler.VerifyAll();
    }
}
