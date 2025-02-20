using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class GetAllDinnersTest : BaseDinnerTest, IClassFixture<DependencyInjectionFixture>, IDisposable
    {
        private readonly IDinnerHandler _dinnerHandler;

        public GetAllDinnersTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _dinnerHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IDinnerHandler>();       

        [Fact]
        public async Task Get_All_Dinners_Should_Return_Success()
        {
            #region Arrange
            IReadOnlyList<CreateDinnerRequest> createDinnerRequests = [.. _fakeCreateDinnerRequest.Generate(20)];

            foreach (CreateDinnerRequest createDinnerRequest in createDinnerRequests)
                await _dinnerHandler.AddDinnerAsync(createDinnerRequest);

            GetAllDinnersRequest getAllDinnersRequest = new GetAllDinnersRequest();
            getAllDinnersRequest.PageNumber = 1;
            getAllDinnersRequest.PageSize = 10;
            #endregion

            #region Act
            PagedResponse<IReadOnlyList<Dinner>> dinnersPagedResponsePageOne = await _dinnerHandler.GetAllDinnersAsync(getAllDinnersRequest);

            getAllDinnersRequest.PageNumber = 2;

            PagedResponse<IReadOnlyList<Dinner>> dinnersPagedResponsePageTwo = await _dinnerHandler.GetAllDinnersAsync(getAllDinnersRequest);
            #endregion

            #region Assert
            dinnersPagedResponsePageOne.ShouldNotBeNull();
            dinnersPagedResponsePageOne.IsSuccess.ShouldBeTrue();
            dinnersPagedResponsePageOne.CurrentPage.ShouldBe(1);
            dinnersPagedResponsePageOne.PageSize.ShouldBe(10);
            dinnersPagedResponsePageOne.Data.ShouldNotBeNull();
            dinnersPagedResponsePageOne.Data.Count.ShouldBe(10);
            dinnersPagedResponsePageOne.Messages.ShouldBeNull();

            dinnersPagedResponsePageTwo.ShouldNotBeNull();
            dinnersPagedResponsePageTwo.IsSuccess.ShouldBeTrue();
            dinnersPagedResponsePageTwo.CurrentPage.ShouldBe(2);
            dinnersPagedResponsePageTwo.PageSize.ShouldBe(10);
            dinnersPagedResponsePageTwo.Data.ShouldNotBeNull();
            dinnersPagedResponsePageTwo.Data.Count.ShouldBe(10);
            dinnersPagedResponsePageTwo.Messages.ShouldBeNull();
            #endregion
        }

        public void Dispose()
        {
            //ServiceCollection serviceCollection = new ServiceCollection();
            //serviceCollection.CleanDatabaseForTests();
        }
    }
}