using FluentValidation;
using Netrift.Application.CQRS.Queries.GetUserByIdQuery;

namespace Netrift.Application.Validation;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
  public GetUserByIdQueryValidator()
  {

  }
}