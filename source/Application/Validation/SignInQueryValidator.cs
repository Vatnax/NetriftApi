using FluentValidation;
using Netrift.Application.CQRS.Queries.SignInQuery;

namespace Netrift.Application.Validation;

/// <summary>
/// A validator for <see cref="SignInQuery"/>
/// </summary>
public class SignInQueryValidator : AbstractValidator<SignInQuery>
{
  /// <summary>
  /// Constructs the validator.
  /// </summary>
  public SignInQueryValidator()
  {
    RuleFor(q => q.Credentials)
      .NotEmpty()
        .WithMessage("Credentials must exist!");

    Unless(q => q.Credentials is null, () =>
    {
      RuleFor(q => q.Credentials.EmailOrName)
        .NotEmpty()
          .WithMessage("Email or username cannot be empty!");

      RuleFor(q => q.Credentials.Password)
        .NotEmpty()
          .WithMessage("Password cannot be empty!");
    });
  }
}