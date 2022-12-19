using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommandValidator : AbstractValidator<DeleteAdCommand>
    {
        public DeleteAdCommandValidator()
        {
            RuleFor(command =>
                command.Id).NotEqual(Guid.Empty);
            RuleFor(command =>
                command.UserId).NotEqual(Guid.Empty);
        }
    }
}
