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
                command.Number).NotEmpty();
            RuleFor(command =>
                command.UserId).NotEqual(Guid.Empty);
            RuleFor(command =>
                command.Description).NotEmpty().MaximumLength(250);
            RuleFor(command =>
                command.ImagePath).NotEmpty();
            RuleFor(command =>
                command.Rating).NotEmpty();
            RuleFor(command =>
                command.ExpirationDate).NotEmpty();
        }
    }
}
