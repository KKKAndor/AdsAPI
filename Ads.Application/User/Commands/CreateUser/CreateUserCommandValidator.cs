using Ads.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<AppUser>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command =>
                command.Id).NotEqual(Guid.Empty);
            RuleFor(command =>
                command.Name).NotEmpty().MaximumLength(50);
        }
    }
}
