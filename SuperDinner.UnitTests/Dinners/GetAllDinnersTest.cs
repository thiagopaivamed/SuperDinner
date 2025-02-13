using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Dinners
{
    public sealed class GetAllDinnersTest : BaseDinnerTest, IDisposable
    {
        [Fact]
        public async Task Get_All_Dinners_Should_Return_Success()
        {
            #region Arrange
            GetAllDinnersRequest getAllDinnersRequest = new GetAllDinnersRequest();
            getAllDinnersRequest.PageNumber = 1;
            getAllDinnersRequest.PageSize = 10;

            IReadOnlyList<Dinner> dinners = _fakeDinner.Generate(10);

            PagedResponse<IReadOnlyList<Dinner>> dinnersResponse = new PagedResponse<IReadOnlyList<Dinner>>(data: dinners,
               totalCount: dinners.Count,
               currentPage: 1,
               pageSize: 10);

            _mockDinnerHandler.Setup(d => d.GetAllDinnersAsync(getAllDinnersRequest)).ReturnsAsync(dinnersResponse);
            #endregion

            #region Act
            PagedResponse<IReadOnlyList<Dinner>> dinnersPagedResponse = await _mockDinnerHandler.Object.GetAllDinnersAsync(getAllDinnersRequest);
            #endregion

            #region Assert
            dinnersPagedResponse.ShouldNotBeNull();
            dinnersPagedResponse.IsSuccess.ShouldBeTrue();
            dinnersPagedResponse.Data.ShouldNotBeNull();
            dinnersPagedResponse.Data.Count.ShouldBe(10);
            dinnersPagedResponse.Messages.ShouldBeNull();
            #endregion
        }

        public void Dispose() => _mockDinnerHandler.VerifyAll();
    }
}
