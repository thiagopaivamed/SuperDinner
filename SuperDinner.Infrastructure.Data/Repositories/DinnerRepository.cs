using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public sealed class DinnerRepository(SuperDinnerContext superDinnerContext) : 
        GenericRepository<Dinner>(superDinnerContext), 
        IDinnerRepository;
}
