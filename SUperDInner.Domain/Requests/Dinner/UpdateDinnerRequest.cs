using SuperDinner.Domain.Entities;

namespace SuperDinner.Domain.Requests.Dinner
{
    public sealed record UpdateDinnerRequest : Request
    {
        public Guid DinnerId { get; set; }

        public DateTime DinnerDate { get; set; }

        public new Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }
        public Entities.Restaurant Restaurant { get; set; }

        public Guid TransactionId { get; set; }

        public DinnerStatus DinnerStatus { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public UpdateDinnerRequest() =>
            LastModifiedDate = DateTime.UtcNow;
    }
}
