using Microsoft.AspNetCore.Identity;
using WebFiles.Domain.Entities;

namespace WebFiles.Services.Abstract;

public interface IUserService
{
    public Task<IdentityResult> AddUserAsync(User user, string password);
    public Task DeleteUserAsync(User user);
    public Task ChangeExtendedAsync(string userId);
}