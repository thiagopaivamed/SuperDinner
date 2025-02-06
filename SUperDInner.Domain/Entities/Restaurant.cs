namespace SuperDinner.Domain.Entities
{
    public sealed class Restaurant : IBaseEntity
    {
        public Guid RestaurantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ContactPhone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Price { get; set; }

        public int ClientsLimit { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public ICollection<Dinner> Dinners { get; set; }
    }
}
