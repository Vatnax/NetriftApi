using Netrift.Application;

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
    });

    return serviceCollection;
  }
}