using Netrift.Application.CQRS.Behaviors;
using Netrift.Application;
using FluentValidation;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddCleanArchitectureLayers(this IServiceCollection serviceCollection)
  {
    serviceCollection
      .AddPresentationLayer()
      .AddApplicationLayer();
    return serviceCollection;
  }

  private static IServiceCollection AddPresentationLayer(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddControllers();
    return serviceCollection;
  }

  private static IServiceCollection AddApplicationLayer(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddAutoMapper(AssemblyReference.ASSEMBLY_REF);
    serviceCollection.AddMediatR(config =>
    {
      config.RegisterServicesFromAssembly(AssemblyReference.ASSEMBLY_REF);
      config.AddOpenBehavior(typeof(LoggingBehavior<,>));
      config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
    serviceCollection.AddValidatorsFromAssembly(AssemblyReference.ASSEMBLY_REF);

    return serviceCollection;
  }
}