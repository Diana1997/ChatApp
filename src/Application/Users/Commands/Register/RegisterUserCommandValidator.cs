using Application._Common.Interfaces;
using Application._Common.Resources;
using FluentValidation;
using MongoDB.Driver;

namespace Application.Users.Commands.Register
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IAppDbContext _context;
        
        public RegisterUserCommandValidator(IAppDbContext context)
        {
            _context = context;
            
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredField)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredField)
                .Must(BeUniqueNickname)
                .WithMessage(ValidationMessages.AlreadyExists);
        }
        
        private bool  BeUniqueNickname(string nickname) => 
            !_context.Users.Find(u => u.Nickname == nickname).Any();
        
    }
}