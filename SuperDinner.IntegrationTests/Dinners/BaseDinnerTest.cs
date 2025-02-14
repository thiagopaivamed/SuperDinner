using Bogus;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;

namespace SuperDinner.IntegrationTests.Dinners
{
    public class BaseDinnerTest
    {
        protected readonly Faker<CreateDinnerRequest> _fakeCreateDinnerRequest;

        public BaseDinnerTest()
        {
            SetEnvironmentForTesting();

            _fakeCreateDinnerRequest = new Faker<CreateDinnerRequest>()
               .RuleFor(x => x.DinnerDate, DateTime.Now)
               .RuleFor(x => x.UserId, Guid.NewGuid())
               .RuleFor(x => x.RestaurantId, Guid.NewGuid())
               .RuleFor(x => x.TransactionId, Guid.NewGuid())
               .RuleFor(x => x.DinnerStatus, DinnerStatus.WaitingForPayment)
               .RuleFor(x => x.CreatedDate, DateTime.Now);

            _fakeCreateDinnerRequest.ShouldNotBeNull();

            string testEnvironmentValue = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
            testEnvironmentValue.ShouldNotBeNullOrEmpty();
            testEnvironmentValue.ShouldBe("Testing");
        }

        private static void SetEnvironmentForTesting()
                => Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }
}
