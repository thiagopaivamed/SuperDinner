namespace SuperDinner.Domain.Requests
{
    public abstract record Request
    {
        public string UserId { get; set; } = string.Empty;
    }
}
