using Netrift.Application.CQRS.Behaviors;
using Netrift.Application;
using FluentValidation;

namespace Netrift.Api.Extensions;

/// <summary>
/// Extensions for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Adds clean architecture layer to the service collection.
  /// </summary>
  /// <param name="serviceCollection">The service collection that layers should be added to.</param>
  /// <returns>The service collection with added layers.</returns>
  public static IServiceCollection AddCleanArchitectureLayers(this IServiceCollection serviceCollection)
  {
    serviceCollection
      .AddPresentationLayer()
      .AddApplicationLayer();
    return serviceCollection;
  }

  /// <summary>
  /// Adds a presentation layer to the service collection.
  /// </summary>
  /// <param name="serviceCollection">The service collection that a presentation layer should be added to.</param>
  /// <returns>The service collection with a presentation layer added.</returns>
  private static IServiceCollection AddPresentationLayer(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddControllers();
    return serviceCollection;
  }

  /// <summary>
  /// Adds an application layer to the service collection.
  /// </summary>
  /// <param name="serviceCollection">The service collection that an application layer should be added to.</param>
  /// <returns>The service collection with an application layer added.</returns>
  private static IServiceCollection AddApplicationLayer(this IServiceCollection serviceCollection)
  {
    // Configuring Automapper
    serviceCollection.AddAutoMapper(AssemblyReference.ASSEMBLY_REF);

    // Configuring MediatR (CQRS) with behaviors
    serviceCollection.AddMediatR(config =>
    {
      config.RegisterServicesFromAssembly(AssemblyReference.ASSEMBLY_REF);
      config.AddOpenBehavior(typeof(LoggingBehavior<,>));
      config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    // Configuring FluentValidations
    serviceCollection.AddValidatorsFromAssembly(AssemblyReference.ASSEMBLY_REF);

    return serviceCollection;
  }
}