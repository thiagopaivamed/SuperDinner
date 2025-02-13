using FluentValidation;
using SuperDinner.Domain.Requests.Dinner;

namespace SuperDinner.Service.Validators.Dinner
{
    public sealed class CreateDinnerRequestValidator : AbstractValidator<CreateDinnerRequest>
    {
        public CreateDinnerRequestValidator()
        {
            RuleFor(d => d.DinnerDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("{PropertyName} cannot be in the past");

            RuleFor(d => d.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(d => d.RestaurantId)
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(d => d.TransactionId)
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(d => d.DinnerStatus)
                .NotNull().WithMessage("{PropertyName} is required");

            RuleFor(d => d.CreatedDate)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("{PropertyName} cannot be in the past");
        }
    }
}
