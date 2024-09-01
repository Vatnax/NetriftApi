using System.Data;
using FluentValidation;
using Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

namespace Netrift.Application.Validation;

public class GetUserByEmailOrNameQueryValidator : AbstractValidator<GetUserByEmailOrNameQuery>
{
  public GetUserByEmailOrNameQueryValidator()
  {
    RuleFor(q => q.EmailOrName)
      .NotEmpty()
        .WithMessage("Email or name cannot be empty!");
  }
}