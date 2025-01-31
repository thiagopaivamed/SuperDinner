namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed class GetRestaurantByIdRequest : Request
    {
        public Guid RestaurantId { get; set; }
    }
}
