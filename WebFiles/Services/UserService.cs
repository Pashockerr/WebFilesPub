using Microsoft.AspNetCore.Identity;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private readonly INoteService _noteService;

    public UserService(UserManager<User> userManager, ApplicationDbContext dbContext, IConfiguration configuration, IFileService fileService, INoteService noteService)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
        _fileService = fileService;
        _noteService = noteService;
    }

    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        long defaultSpace = _configuration.GetValue<long>("DefaultSpace");
        user.AvailableSpace = defaultSpace;
        return await _userManager.CreateAsync(user, password);
    }

    public async Task DeleteUserAsync(User user)
    {
        await _fileService.DeleteUserFilesAsync(user.Id);
        await _noteService.DeleteUserNotesAsync(user.Id);

        await _userManager.DeleteAsync(await _userManager.FindByIdAsync(user.Id));
    }

    public async Task ChangeExtendedAsync(string userId)
    {
        var res = from u in _dbContext.Users
            where u.Id.Equals(userId)
            select u;

        if(res.Any())
        {
            var user = res.First();
            long extendedSpace = _configuration.GetValue<long>("ExtendedSpace");
            long defaultSpace = _configuration.GetValue<long>("DefaultSpace");
            if (user.IsExtendedAccount)
            {
                if ((defaultSpace - (extendedSpace - user.AvailableSpace)) < 0)
                {
                    throw new Exception("This user account can't be switched. He used more space, than default.");
                }
            }
            
            user.IsExtendedAccount = !user.IsExtendedAccount;
            user.AvailableSpace = user.IsExtendedAccount ? extendedSpace - (defaultSpace - user.AvailableSpace) : defaultSpace - (extendedSpace - user.AvailableSpace);
            
            await _dbContext.SaveChangesAsync();
        }
    }
}