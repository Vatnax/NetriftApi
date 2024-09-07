using FluentValidation;
using Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

namespace Netrift.Application.Validation;

/// <summary>
/// A validator for <see cref="GetUserByEmailOrNameQuery"/>
/// </summary>
public class GetUserByEmailOrNameQueryValidator : AbstractValidator<GetUserByEmailOrNameQuery>
{
  /// <summary>
  /// Constructs the validator.
  /// </summary>
  public GetUserByEmailOrNameQueryValidator()
  {
    RuleFor(q => q.EmailOrName)
      .NotEmpty()
        .WithMessage("Email or name cannot be empty!");
  }
}