using FluentValidation;

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
