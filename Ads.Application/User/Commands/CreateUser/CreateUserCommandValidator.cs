using FluentValidation;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command =>
                command.UserName)
                .NotEmpty()
                .WithMessage("Name shouldn't be empty")
                .MaximumLength(50)
                .WithMessage("Name shouldn't be longer than 50 symbols")
                .MinimumLength(5)
                .WithMessage("Name shouldn't be lesser than 5 symbols");
        }
    }
}
