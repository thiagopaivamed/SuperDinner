using Microsoft.AspNetCore.Components;
using MudBlazor;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;

namespace SuperDinner.FrontEnd.Pages.Restaurant
{
    public partial class CreateRestaurant : ComponentBase
    {
        public bool isLoading { get; set; } = false;

        public CreateRestaurantRequest createRestaurantRequest { get; set; } = new CreateRestaurantRequest();

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public IRestaurantHandler RestaurantHandler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        public async Task OnValidSubmitAsync()
        {
            isLoading = true;

            try
            {
                var restaurantCreated = await RestaurantHandler.AddRestaurantAsync(createRestaurantRequest);

                if(restaurantCreated.IsSuccess)
                {
                    Snackbar.Add("Restaurant created successfully.", Severity.Success);
                    NavigationManager.NavigateTo("/restaurants");
                }
                else
                {
                    Snackbar.Add("Unable to create restaurant.", Severity.Error);
                }
            }
            catch (Exception ex)
            {

               Snackbar.Add(ex.Message, Severity.Error);
            }

            finally
            {
                isLoading = false;
            }
        }


    }
}
