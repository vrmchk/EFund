using Microsoft.AspNetCore.Identity;

namespace EFund.BLL.Extensions;

public static class UserManagerExtensions
{
    public static async Task<IdentityResult> ChangeEmailAsync<T>(this UserManager<T> userManager, T user,
        string newEmail)
        where T : class
    {
        var userNameChanged = await userManager.SetUserNameAsync(user, newEmail);
        if (!userNameChanged.Succeeded)
            return userNameChanged;

        var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        return await userManager.ChangeEmailAsync(user, newEmail, token);
    }
}