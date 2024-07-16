using FluentValidation;

namespace Application.Features.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be null.");
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}
