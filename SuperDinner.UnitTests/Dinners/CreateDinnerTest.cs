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
    public sealed class CreateDinnerTest : BaseDinnerTest, IDisposable
    {
        private readonly Faker<CreateDinnerRequest> _fakeCreateDinnerRequest;

        public CreateDinnerTest()
        {
            _fakeCreateDinnerRequest = new Faker<CreateDinnerRequest>()
                .RuleFor(x => x.DinnerDate, DateTime.Now)
                .RuleFor(x => x.UserId, Guid.NewGuid())
                .RuleFor(x => x.RestaurantId, Guid.NewGuid())
                .RuleFor(x => x.TransactionId, Guid.NewGuid())
                .RuleFor(x => x.DinnerStatus, DinnerStatus.WaitingForPayment)
                .RuleFor(x => x.CreatedDate, DateTime.Now);

            _fakeCreateDinnerRequest.ShouldNotBeNull();
        }

        [Fact]
        public async Task Use_Valid_Dinner_Should_Return_True()
        {
            #region Arrange
            CreateDinnerRequest request = _fakeCreateDinnerRequest.Generate();
            CreateDinnerRequestValidator validator = new CreateDinnerRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult dinnerCreatedResult = await validator.ValidateAsync(request);
            #endregion

            #region Assert
            dinnerCreatedResult.IsValid.ShouldBeTrue();
            #endregion
        }

        [Fact]
        public async Task Use_Invalid_Dinner_Should_Return_False()
        {
            #region Arrange
            CreateDinnerRequest request = new CreateDinnerRequest();
            CreateDinnerRequestValidator validator = new CreateDinnerRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult dinnerCreatedResult = await validator.ValidateAsync(request);
            #endregion

            #region Assert
            dinnerCreatedResult.IsValid.ShouldBeFalse();
            #endregion
        }

        [Fact]
        public async Task Add_Valid_Dinner_Should_Return_Success()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = _fakeCreateDinnerRequest.Generate();
            createDinnerRequest.ShouldNotBeNull();

            Dinner dinner = _fakeDinner.Generate();
            dinner.ShouldNotBeNull();

            Response<Dinner> dinnerResponse = new Response<Dinner>(dinner, StatusCodes.Status201Created);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeTrue();
            dinnerResponse.Data.ShouldNotBeNull();
            dinnerResponse.Messages.ShouldBeNull();

            _mockDinnerHandler.Setup(x => x.AddDinnerAsync(createDinnerRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerCreatedResponse = await _mockDinnerHandler.Object.AddDinnerAsync(createDinnerRequest);
            #endregion

            #region Assert
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeTrue();
            dinnerCreatedResponse.Data.ShouldNotBeNull();
            dinnerCreatedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Add_Invalid_Dinner_Should_Return_False()
        {
            #region Arrange
            CreateDinnerRequest createDinnerRequest = new CreateDinnerRequest();
            createDinnerRequest.ShouldNotBeNull();
            
            Response<Dinner> dinnerResponse = new Response<Dinner>(null, StatusCodes.Status400BadRequest, ["Invalid dinner data"]);
            dinnerResponse.ShouldNotBeNull();
            dinnerResponse.IsSuccess.ShouldBeFalse();
            dinnerResponse.Data.ShouldBeNull();
            dinnerResponse.Messages.ShouldNotBeNull();

            _mockDinnerHandler.Setup(x => x.AddDinnerAsync(createDinnerRequest)).ReturnsAsync(dinnerResponse);
            #endregion

            #region Act
            Response<Dinner> dinnerCreatedResponse = await _mockDinnerHandler.Object.AddDinnerAsync(createDinnerRequest);
            #endregion

            #region Assert
            dinnerCreatedResponse.ShouldNotBeNull();
            dinnerCreatedResponse.IsSuccess.ShouldBeFalse();
            dinnerCreatedResponse.Data.ShouldBeNull();
            dinnerCreatedResponse.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose() => _mockDinnerHandler.VerifyAll();
    }
}
