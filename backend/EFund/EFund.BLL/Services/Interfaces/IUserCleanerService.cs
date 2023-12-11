namespace EFund.BLL.Services.Interfaces;

public interface IUserCleanerService
{
    Task ClearExpiredUsersAsync();
}