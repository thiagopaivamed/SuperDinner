namespace SuperDinner.Domain.Requests.Restaurant
{
    public sealed record GetRestaurantByIdRequest : Request
    {
        public Guid RestaurantId { get; set; }
    }
}
