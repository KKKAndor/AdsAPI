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
                command.UserId).NotEqual(Guid.Empty);
            RuleFor(command =>
                command.Description).MaximumLength(250);            
            RuleFor(command =>
                command.ExpirationDate).GreaterThan(DateTime.Now.Date);
            RuleFor(command =>
                command.Rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

        }
    }
}
