namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed record GetRestaurantByIdRequest(Guid RestaurantId) : Request;
}
