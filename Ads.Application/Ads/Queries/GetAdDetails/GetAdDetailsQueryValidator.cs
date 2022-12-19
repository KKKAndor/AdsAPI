using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class GetAdDetailsQueryValidator : AbstractValidator<GetAdDetailsQuery>
    {
        public GetAdDetailsQueryValidator()
        {
            RuleFor(command =>
                command.Id).NotEqual(Guid.Empty);
        }
    }
}
