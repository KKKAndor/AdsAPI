using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommandValidator : AbstractValidator<CreateAdCommand>
    {
        public CreateAdCommandValidator()
        {
            RuleFor(command =>
                command.Number)
                .NotEmpty();
            RuleFor(command =>
                command.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(command =>
                command.Description)
                .NotEmpty()
                .WithMessage("You should add some description in your add")
                .MaximumLength(500)
                .WithMessage("The description length must be lesse than 500");
            RuleFor(command =>
                command.ImagePath)
                .NotEmpty();
            RuleFor(command =>
                command.ExpirationDate)
                .GreaterThan(DateTime.Now.Date)
                .WithMessage("The expiration date should be after today");
            RuleFor(command =>
                command.Rating)
                .LessThanOrEqualTo(100)
                .WithMessage("Rating must be lesser or equal to 100")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Rating must be lesser or equal to 0");
        }
    }
}
