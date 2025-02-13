using Bogus;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;

namespace SuperDinner.UnitTests.Dinners
{
    public class BaseDinnerTest
    {
        protected readonly Faker<Dinner> _fakeDinner;

        protected readonly Mock<IDinnerHandler> _mockDinnerHandler;

        public BaseDinnerTest()
        {
            _fakeDinner = new Faker<Dinner>()
                .RuleFor(x => x.DinnerDate, DateTime.Now)
                .RuleFor(x => x.UserId, Guid.NewGuid())
                .RuleFor(x => x.RestaurantId, Guid.NewGuid())
                .RuleFor(x => x.TransactionId, Guid.NewGuid())
                .RuleFor(x => x.DinnerStatus, DinnerStatus.WaitingForPayment)
                .RuleFor(x => x.CreatedDate, DateTime.Now);

            _fakeDinner.ShouldNotBeNull();

            _mockDinnerHandler = new Mock<IDinnerHandler>();
            _mockDinnerHandler.ShouldNotBeNull();
        }
    }
}
