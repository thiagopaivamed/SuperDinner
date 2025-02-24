using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.FrontEnd.Pages.Restaurant
{
    public partial class CreateRestaurant : ComponentBase
    {
        public bool IsLoading { get; set; } = false;

        public CreateRestaurantRequest CreateRestaurantRequest { get; set; } = new CreateRestaurantRequest();

        public EditForm CreateRestaurantForm { get; set; } = new EditForm();

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public IRestaurantHandler RestaurantHandler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IJSRuntime jsRuntime { get; set; } = null!;

        public async Task OnValidSubmitAsync()
        {
            IsLoading = true;
            CreateRestaurantRequest.Latitude = 50;
            CreateRestaurantRequest.Longitude = 50;
            CreateRestaurantRequest.UserId = Guid.NewGuid().ToString();

            try
            {
                bool createRestaurantFormIsValid = CreateRestaurantForm.EditContext!.Validate();
                if (createRestaurantFormIsValid)
                {
                    Response<Domain.Entities.Restaurant> restaurantCreated = await RestaurantHandler.AddRestaurantAsync(CreateRestaurantRequest);

                    if (restaurantCreated.IsSuccess)
                    {
                        Snackbar.Add($"The restaurant {restaurantCreated.Data!.Name} created successfully.", Severity.Success);
                        NavigationManager.NavigateTo("/restaurants");
                    }
                    else
                        Snackbar.Add($"{string.Join("\n", restaurantCreated.Messages)}", Severity.Error);

                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }

            finally
            {
                IsLoading = false;
            }
        }

        public void OnCancel()
            => NavigationManager.NavigateTo("/restaurants");
    }
}