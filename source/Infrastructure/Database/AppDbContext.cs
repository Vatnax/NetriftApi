using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Netrift.Infrastructure.Identity.IdentityEntities;

namespace Netrift.Infrastructure.Database;

/// <summary>
/// Main database context.
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>
{
  /// <summary>
  /// Constructs the database context.
  /// </summary>
  /// <param name="options">Options for the context.</param>
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {

  }
}