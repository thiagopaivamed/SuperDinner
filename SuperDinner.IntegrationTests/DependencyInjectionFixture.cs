using Microsoft.Extensions.DependencyInjection;

namespace SuperDinner.IntegrationTests
{
    public sealed class DependencyInjectionFixture : IDisposable
    {
        public readonly ServiceProvider serviceProvider;

        public DependencyInjectionFixture()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.RegisterDbContextForTesting();
            serviceCollection.AddLogging();
            serviceCollection.RegisterRepositories();
            serviceCollection.RegisterServices();
            serviceCollection.RegisterValidation();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }        

        public void Dispose() =>
            serviceProvider.Dispose();
    }

}