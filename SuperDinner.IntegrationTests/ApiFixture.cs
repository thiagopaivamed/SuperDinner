using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.IntegrationTests
{
    public sealed class ApiFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(serviceCollection =>
            {
                List<ServiceDescriptor> descriptors = serviceCollection
                    .Where(d => d.ServiceType == typeof(DbContextOptions<SuperDinnerContext>))
                    .ToList();

                descriptors.ForEach(descriptor => serviceCollection.Remove(descriptor));
                
                serviceCollection.RegisterDbContextForTesting();
                serviceCollection.AddLogging();
                serviceCollection.RegisterRepositories();
                serviceCollection.RegisterServices();
                serviceCollection.RegisterValidation();
            });
        }
    }
}
