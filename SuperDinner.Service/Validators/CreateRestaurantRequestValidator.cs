using FluentValidation;
using SuperDinner.Domain.Requests.Restaurant;

namespace SuperDinner.Service.Validators
{
    public sealed class CreateRestaurantRequestValidator : AbstractValidator<CreateRestaurantRequest>
    {
        public CreateRestaurantRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must not exceed 30 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must not exceed 30 characters");

            RuleFor(d => d.Latitude)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(-90.0, 90.0).WithMessage("{PropertyName} must be between -90 and 90.");

            RuleFor(d => d.Longitude)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(-180.0, 180.0).WithMessage("{PropertyName} must be between -180 and 180.");

            RuleFor(d => d.Price)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0 and cannot have negative values");

            RuleFor(x => x.ClientsLimit)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} should be greater than 0 and cannot have negative values");

            RuleFor(d => d.CreatedDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Must(NotCreatedDateBeforeToday).WithMessage("{PropertyValue} cannot be in the past");

            RuleFor(d => d.LastModifiedDate)
                .Null()
                .Must(NotUpdatedDateBeforeToday).WithMessage("{PropertyValue} cannot be in the past");
        }

        private bool NotBeforeToday(DateTime eventDate) =>
        eventDate >= DateTime.Today.Date;

        private bool NotCreatedDateBeforeToday(DateTime createdDate) =>
            createdDate == default || NotBeforeToday(createdDate);

        private bool NotUpdatedDateBeforeToday(DateTime? updatedDate) =>
            updatedDate is null || NotBeforeToday((DateTime)updatedDate);
    }
}