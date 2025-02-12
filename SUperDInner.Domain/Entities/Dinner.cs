namespace SuperDinner.Domain.Entities
{
    public sealed class Dinner : IBaseEntity
    {
        public Guid DinnerId { get; set; }

        public DateTime DinnerDate { get; set; }

        public Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public Guid TransactionId { get; set; }

        public DinnerStatus DinnerStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }        

        public Dinner()
        {
            DinnerStatus = DinnerStatus.WaitingForPayment;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
