using FluentValidation;
using Netrift.Application.CQRS.Commands.CreateUserCommand;

namespace Netrift.Application.Validation;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
  public CreateUserCommandValidator()
  {
    RuleFor(cuc => cuc.AppUser)
      .NotNull()
        .WithMessage("User object cannot be empty!");

    RuleFor(cuc => cuc.AppUser.UserName)
      .NotEmpty()
        .WithMessage("Username cannot be empty!")
      .Length(4, 16)
        .WithMessage("Username cannot be longer than 16 characters and less than 4 characters!");

    RuleFor(cuc => cuc.AppUser.Password)
      .NotEmpty()
        .WithMessage("Password cannot be empty!");

    RuleFor(cuc => cuc.AppUser.Email)
      .NotEmpty()
        .WithMessage("Email cannot be empty!")
      .EmailAddress()
        .WithMessage("Email has to be in a correct format!");
  }
}