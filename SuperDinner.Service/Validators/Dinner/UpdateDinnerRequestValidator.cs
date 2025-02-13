using FluentValidation;
using SuperDinner.Domain.Requests.Dinner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperDinner.Service.Validators.Dinner
{
    public sealed class UpdateDinnerRequestValidator : AbstractValidator<UpdateDinnerRequest>
    {
        public UpdateDinnerRequestValidator()
        {
            RuleFor(d => d.DinnerId)
                .NotEmpty().WithMessage("{PropertyName} is required");

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
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(d => d.LastModifiedDate)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("{PropertyName} cannot be in the past");
        }
    }
}
