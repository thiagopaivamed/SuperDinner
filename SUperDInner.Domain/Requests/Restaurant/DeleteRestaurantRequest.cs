namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed record DeleteRestaurantRequest : Request
    {
        public Guid RestaurantId { get; set; }
    }
}
