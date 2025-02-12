namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed record DeleteRestaurantRequest(Guid RestaurantId) : Request;
}
