using ApiServer.Protocol;
using FluentValidation;

namespace ApiServer.Validations;

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress()
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("이메일은 알파벳과 숫자로만 구성되어야 합니다.");

        RuleFor(e => e.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(12);

    }
}