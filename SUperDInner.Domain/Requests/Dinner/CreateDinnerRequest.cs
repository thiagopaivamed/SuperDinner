using SuperDinner.Domain.Entities;

namespace SuperDinner.Domain.Requests.Dinner
{
    public sealed record CreateDinnerRequest : Request
    {
        public DateTime DinnerDate { get; set; }

        public Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }
        public Entities.Restaurant Restaurant { get; set; }

        public Guid TransactionId { get; set; }

        public DinnerStatus DinnerStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public CreateDinnerRequest() => CreatedDate = DateTime.UtcNow;
    }
}
