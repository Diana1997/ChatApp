using Application._Common.Resources;
using FluentValidation;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredField)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredField);
        }
    }
}