using FluentValidation;

namespace Ads.Application.Upload.Commands;

public class UploadCommandValidator : AbstractValidator<UploadCommand>
{
    public UploadCommandValidator()
    {
        RuleFor(command =>
                command.FileName)
            .NotNull()
            .NotEqual(string.Empty);
        RuleFor(command =>
                command.FileContent)
            .NotNull()
            .NotEqual(Array.Empty<byte>());
    }
}