namespace SuperDinner.Domain.Entities
{
    public interface IBaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
