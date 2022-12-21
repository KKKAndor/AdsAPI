using FluentValidation;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQueryValidator : AbstractValidator<GetAdListQuery>
    {
        public GetAdListQueryValidator()
        {
            RuleFor(command =>
                command.AdsParameters.MaxRating)
                .LessThanOrEqualTo(100)
                .WithMessage("Rating must be lesser than 100");
            RuleFor(command =>
                    command.AdsParameters.MaxRating)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Rating must be greater than 0");
        }
    }
}
