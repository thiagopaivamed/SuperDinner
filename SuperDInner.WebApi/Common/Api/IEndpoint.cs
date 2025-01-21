namespace SuperDInner.Application.Common.Api
{
    public interface IEndpoint
    {
        static abstract void Map(IEndpointRouteBuilder endpointRouteBuilder);
    }
}
