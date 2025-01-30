using FluentValidation.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Reflection;
using Shouldly;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.IntegrationTests
{
    public class ValidationFixture<TEntity> : IDisposable where TEntity : class
    {
        private const string npgSqlConnectionString = "Host=localhost; Database=SuperDinner; User Id = postgres; Password=admin;";
        
        public readonly int propertiesCount;
        public readonly int stringsCount;
        public readonly int doublesCount;
        public readonly int intsCount;

        public ValidationFixture()
        {
            propertiesCount = GetPropertyCount();
            stringsCount = GetPropertyCount<string>();
            doublesCount = GetPropertyCount<double>();
            intsCount = GetPropertyCount<int>();

            propertiesCount.ShouldBeGreaterThanOrEqualTo(0);
            stringsCount.ShouldBeGreaterThanOrEqualTo(0);
            doublesCount.ShouldBeGreaterThanOrEqualTo(0);
            intsCount.ShouldBeGreaterThanOrEqualTo(0);
        }

        private static PropertyInfo[] GetProperties() => typeof(TEntity).GetProperties();

        private static int GetPropertyCount<T>() => GetProperties().Count(p => p.PropertyType == typeof(T));

        private static int GetPropertyCount() => GetProperties().Length;

        public string[] GetPropertyNames(string[]? propertiesToIgnore = null) =>
            GetFilteredPropertyNames(null, propertiesToIgnore);

        public string[] GetPropertyNames<T>(string[]? propertiesToIgnore = null) =>
            GetFilteredPropertyNames(typeof(T), propertiesToIgnore);

        private static string[] GetFilteredPropertyNames(Type? propertyType, string[]? propertiesToIgnore)
        {
            return GetProperties()
                .Where(p => (propertyType == null || p.PropertyType == propertyType) &&
                            (propertiesToIgnore == null || !propertiesToIgnore.Contains(p.Name)))
                .Select(p => p.Name)
                .ToArray();
        }        

        public EntityTypeBuilder<TEntity> GetEntityTypeBuilder<TEntity, TEntityConfiguration>()
            where TEntity : class
            where TEntityConfiguration : IEntityTypeConfiguration<TEntity>, new()
        {
            DbContextOptions<SuperDinnerContext> options = new DbContextOptionsBuilder<SuperDinnerContext>()
                .UseNpgsql(new NpgsqlConnection(npgSqlConnectionString))
                .Options;

            SuperDinnerContext context = new SuperDinnerContext(options);
            ConventionSet conventionSet = ConventionSet.CreateConventionSet(context);
            ModelBuilder modelBuilder = new ModelBuilder(conventionSet);
            EntityTypeBuilder<TEntity> entityTypeBuilder = modelBuilder.Entity<TEntity>();

            TEntityConfiguration entityTypeConfiguration = new TEntityConfiguration();
            entityTypeConfiguration.Configure(entityTypeBuilder);

            entityTypeBuilder.ShouldNotBeNull();

            return entityTypeBuilder;
        }

        public Dictionary<string, IMutableProperty?> GetFluentEfApiConfigurationProperties<TEntity>(string[] propertiesToValidate, EntityTypeBuilder<TEntity> entityTypeBuilder) 
            where TEntity : class
        {
            Dictionary<string, IMutableProperty?> fluentApiConfigurationProperties = propertiesToValidate
                .Select(p => new
                {
                    Key = p,
                    FieldMetadata = entityTypeBuilder.Metadata.FindDeclaredProperty(p)
                })
                .ToDictionary(key => key.Key, value => value.FieldMetadata);

            fluentApiConfigurationProperties.ShouldNotBeNull();

            return fluentApiConfigurationProperties;
        }

        public Dictionary<string, IValidator> GetFluentValidationProperties<TEntity, IValidator>(string[] propertiesToValidate, IValidator<TEntity> entityValidator) 
            where TEntity : class
        {
            Dictionary<string, IValidator> fluentValidationProperties = propertiesToValidate
               .Select(p => new
               {
                   Key = p,
                   Validator = entityValidator.GetValidatorsForMember(p).OfType<IValidator>().First()
               })
               .ToDictionary(key => key.Key, value => value.Validator);

            fluentValidationProperties.ShouldNotBeNull();

            return fluentValidationProperties;
        }

        public IPropertyValidator[] GetValidatorsForMember<TEntity>(IValidator<TEntity> entityValidator, string memberName)
        {
            IValidatorDescriptor descriptor = entityValidator.CreateDescriptor();

            descriptor.ShouldNotBeNull();

            return descriptor.GetValidatorsForMember(memberName).Select(x => x.Validator).ToArray();
        }

        public void Dispose()
        {

        }
    }
}
