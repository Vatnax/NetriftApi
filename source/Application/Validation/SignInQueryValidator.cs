using System.Data;
using FluentValidation;
using Netrift.Application.CQRS.Commands.CreateUserCommand;
using Netrift.Application.CQRS.Queries.SignInQuery;

namespace Netrift.Application.Validation;

public class SignInQueryValidator : AbstractValidator<SignInQuery>
{
  public SignInQueryValidator()
  {
    RuleFor(q => q.Credentials.EmailOrName)
      .NotEmpty()
        .WithMessage("Email or username cannot be empty!");

    RuleFor(q => q.Credentials.Password)
      .NotEmpty()
        .WithMessage("Password cannot be empty!");
  }
}