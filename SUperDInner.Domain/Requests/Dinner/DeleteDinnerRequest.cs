namespace SuperDinner.Domain.Requests.Dinner
{
    public sealed record DeleteDinnerRequest (Guid DinnerId) : Request;
}
