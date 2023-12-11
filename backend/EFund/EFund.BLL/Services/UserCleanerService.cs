using EFund.BLL.Services.Interfaces;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class UserCleanerService : IUserCleanerService
{
    private readonly IRepository<UserRegistration> _userRegistrationRepository;
    private readonly UserManager<User> _userManager;

    public UserCleanerService(IRepository<UserRegistration> userRegistration, UserManager<User> userManager)
    {
        _userRegistrationRepository = userRegistration;
        _userManager = userManager;
    }

    public async Task ClearExpiredUsersAsync()
    {
        var expiredUsers = _userRegistrationRepository.Where(user =>
            user.IsCodeRegenerated && user.ExpiresAt <= DateTimeOffset.UtcNow);

        var users = await expiredUsers.Include(ur => ur.User).Select(ur => ur.User).ToListAsync();
        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }

        await _userRegistrationRepository.DeleteManyAsync(await expiredUsers.ToListAsync());
    }
}