using Microsoft.AspNetCore.Components;
using MudBlazor;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.FrontEnd.Pages.Restaurant
{
    public partial class Restaurants : ComponentBase
    {
        public bool IsLoading { get; set; } = false;

        private MudTable<Domain.Entities.Restaurant> restaurantsTable = null!;

        [Inject]
        private IRestaurantHandler RestaurantHandler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        private string searchString { get; set; } = string.Empty;

        private async Task<TableData<Domain.Entities.Restaurant>> GetTableData(TableState tableState, CancellationToken cancellationToken)
        {
            try
            {
                IsLoading = true;

                GetAllRestaurantsRequest getAllRestaurantsRequest = new GetAllRestaurantsRequest();
                getAllRestaurantsRequest.PageNumber = tableState.Page + 1;
                getAllRestaurantsRequest.PageSize = tableState.PageSize;
                PagedResponse<IReadOnlyList<Domain.Entities.Restaurant>> getAllRestauratsPagedResponse = await RestaurantHandler.GetAllRestaurantsAsync(getAllRestaurantsRequest);

                if (getAllRestauratsPagedResponse.IsSuccess)
                {
                    IEnumerable<Domain.Entities.Restaurant> restaurantsCreated = getAllRestauratsPagedResponse.Data!;

                    restaurantsCreated = tableState.SortLabel switch
                    {
                        "nameField" => restaurantsCreated.OrderByDirection(tableState.SortDirection, o => o.Name),
                        "addressField" => restaurantsCreated.OrderByDirection(tableState.SortDirection, o => o.Address),
                        _ => restaurantsCreated
                    };

                    return new TableData<Domain.Entities.Restaurant>()
                    {
                        TotalItems = getAllRestauratsPagedResponse.TotalCount,
                        Items = [.. restaurantsCreated]
                    };
                }

                Snackbar.Add("Unable to get restaurants", Severity.Error);
                return new TableData<Domain.Entities.Restaurant>()
                {
                    TotalItems = 0,
                    Items = []
                };
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                return new TableData<Domain.Entities.Restaurant>()
                {
                    TotalItems = 0,
                    Items = []
                };
            }

            finally
            {
                IsLoading = false;
            }
        }

        private void OnSearch(string text)
        {
            searchString = text;
            restaurantsTable.ReloadServerData();
        }
    }
}