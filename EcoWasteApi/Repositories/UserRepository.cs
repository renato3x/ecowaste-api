using EcoWasteApi.Data;
using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoWasteApi.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
  public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
  {
    return dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
  }

  public async Task AddAsync(User user, CancellationToken cancellationToken = default)
  {
    await dbContext.Users.AddAsync(user, cancellationToken);
  }

  public Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}

