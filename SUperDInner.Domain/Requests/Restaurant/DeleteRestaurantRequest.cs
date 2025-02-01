namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed class DeleteRestaurantRequest : Request
    {
        public Guid RestaurantId { get; set; }
    }
}
