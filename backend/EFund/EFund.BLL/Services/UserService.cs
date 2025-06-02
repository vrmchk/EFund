using System.Web;
using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly IEmailSender _emailSender;
    private readonly AppDataConfig _appDataConfig;
    private readonly CallbackUrisConfig _callbackUrisConfig;

    public UserService(IMapper mapper,
        ILogger<UserService> logger,
        UserManager<User> userManager,
        IUserRegistrationService userRegistrationService,
        IEmailSender emailSender,
        AppDataConfig appDataConfig,
        CallbackUrisConfig callbackUrisConfig)
    {
        _mapper = mapper;
        _userManager = userManager;
        _userRegistrationService = userRegistrationService;
        _emailSender = emailSender;
        _appDataConfig = appDataConfig;
        _callbackUrisConfig = callbackUrisConfig;
        _logger = logger;
    }

    public async Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id, string apiUrl, bool withNotifications)
    {
        var user = await IncludeRelations(_userManager.Users, withNotifications).FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        return await ToDto(user, apiUrl);
    }

    public async Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid id, UpdateUserDTO dto, string apiUrl)
    {
        var user = await _userManager.Users.Include(u => u.UserMonobanks).FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        _mapper.Map(dto, user);
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return new IdentityErrorDTO("Unable to update user. Please try again later");
        }

        _logger.LogInformation("User updated: {0}", user.Id);

        return await ToDto(user, apiUrl);
    }

    public async Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.Email == dto.NewEmail)
            return new IncorrectParametersErrorDTO("New email has to differ from the old one");

        if (await _userManager.FindByEmailAsync(dto.NewEmail) != null)
            return new IncorrectParametersErrorDTO("User with this email already exists");

        var code = await _userRegistrationService.RegenerateEmailConfirmationCodeAsync(userId);
        var emailSent = await _emailSender.SendEmailAsync(dto.NewEmail,
            new EmailConfirmationMessage { Code = code });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Email confirmation code sent to user {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send email confirmation code to user {0}", user.Id);
                return new ExternalErrorDTO(error.Message);
            }
        );
    }

    public async Task<Option<ErrorDTO>> ChangeEmailAsync(Guid userId, ConfirmChangeEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.Email == dto.NewEmail)
            return new IncorrectParametersErrorDTO("New email has to differ from the old one");

        if (await _userManager.FindByEmailAsync(dto.NewEmail) != null)
            return new IncorrectParametersErrorDTO("User with this email already exists");

        var option = await _userRegistrationService.CanConfirmEmailAsync(userId, dto.Code);
        return await option.Match<Task<Option<ErrorDTO>>>(
            None: async () =>
            {
                var emailChanged = await _userManager.ChangeEmailAsync(user, dto.NewEmail);
                if (!emailChanged.Succeeded)
                {
                    _logger.LogIdentityErrors(user, emailChanged);
                    return new IdentityErrorDTO("Unable to change email. Please try again later");
                }

                _logger.LogInformation("Email changed for user {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to confirm email for user {0}", user.Id);
                return Task.FromResult<Option<ErrorDTO>>(new IncorrectParametersErrorDTO(error.Message));
            }
        );
    }

    public async Task<Option<ErrorDTO>> UploadAvatarAsync(Guid userId, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (!_appDataConfig.AllowedImages.ContainsKey(file.ContentType))
            return new IncorrectParametersErrorDTO("This type of files is not allowed");

        if (user.AvatarPath != null && File.Exists(user.AvatarPath))
            File.Delete(user.AvatarPath);

        var directory = Path.Combine(_appDataConfig.UserAvatarDirectoryPath, user.Id.ToString("N"));

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        user.AvatarPath = Path.Combine(directory,
            $"{_appDataConfig.AvatarFileName}{Guid.NewGuid():N}{_appDataConfig.AllowedImages[file.ContentType]}");

        await using var outputStream = File.Create(user.AvatarPath);
        await using var inputStream = file.OpenReadStream();
        await inputStream.CopyToAsync(outputStream);

        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return new IdentityErrorDTO("Unable to update user. Please try again later");
        }

        return None;
    }

    public async Task<Option<ErrorDTO>> DeleteAvatarAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.AvatarPath is null)
            return None;

        if (File.Exists(user.AvatarPath))
            File.Delete(user.AvatarPath);

        user.AvatarPath = null;
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return new IdentityErrorDTO("Unable to update user. Please try again later");
        }

        return None;
    }

    public async Task<Option<ErrorDTO>> MakeAdminAsync(MakeAdminDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            return None;

        var roleAdded = await _userManager.AddToRoleAsync(user, Roles.Admin);
        if (!roleAdded.Succeeded)
        {
            _logger.LogIdentityErrors(user, roleAdded);
            return new IdentityErrorDTO("Unable to make user admin. Please try again later");
        }

        return None;
    }

    public async Task<Option<ErrorDTO>> InviteAdminAsync(InviteAdminDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user != null)
            return new NotFoundErrorDTO("User with this email already exists");

        user = _mapper.Map<User>(dto);
        user.CreatedByAdmin = true;
        user.DisplayName = "Invited by admin";

        var userCreated = await _userManager.CreateAsync(user);
        if (!userCreated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userCreated);
            return new IdentityErrorDTO("Unable to create user. Please try again later");
        }

        var token = await _userManager.GenerateAdminInvitationTokenAsync(user);
        var callbackUri = string.Format(_callbackUrisConfig.InviteUserUriTemplate, token);

        var emailSent = await _emailSender.SendEmailAsync(dto.Email,
            new AdminInvitationMessage { InvitationUri = callbackUri });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Admin invitation sent to email {0}", user.Email);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send admin invitation to email {0}", user.Email);
                return new ExternalErrorDTO(error.Message);
            });
    }

    public async Task<Option<ErrorDTO>> PerformUserActionAsync(UserActionDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var userAction = dto.Action.ToEnum<UserAction>();
        if (user.IsBlocked && userAction == UserAction.Block || !user.IsBlocked && userAction == UserAction.Unblock)
            return None;

        user.IsBlocked = userAction == UserAction.Block;
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return new IdentityErrorDTO("Unable to block user. Please try again later");
        }

        return None;
    }

    public async Task<Either<ErrorDTO, PagedListDTO<UserExtendedDTO>>> SearchAsync(SearchUserDTO dto,
        PaginationDTO pagination,
        string apiUrl,
        Guid providerId)
    {
        var queryable = IncludeRelations(_userManager.Users.Where(u => u.Id != providerId), withNotifications: false);

        if (!string.IsNullOrEmpty(dto.Query))
            queryable = queryable.Where(u => u.DisplayName.ToLower().Contains(dto.Query.ToLower()) || u.Email!.Contains(dto.Query));

        var users = await queryable.ToPagedListAsync(pagination.Page, pagination.PageSize);
        var dtos = _mapper.Map<PagedListDTO<UserExtendedDTO>>(users);
        var usersWithDtos = users
            .Zip(dtos.Items, (user, userDto) => (user, userDto));

        foreach (var (user, userDto) in usersWithDtos)
        {
            userDto.AvatarUrl = (user.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
            userDto.IsAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);
        }

        return dtos;
    }

    public async Task<List<UserMinimizedDTO>> SearchMinimizedAsync(SearchUserDTO dto, string apiUrl)
    {
        var queryable = _userManager.Users;

        if (!string.IsNullOrEmpty(dto.Query))
            queryable = queryable.Where(u => u.DisplayName.ToLower().Contains(dto.Query.ToLower()) || u.Email!.Contains(dto.Query));

        var users = await queryable.ToListAsync();
        var dtos = _mapper.Map<List<UserMinimizedDTO>>(users);
        var usersWithDtos = users
            .Zip(dtos, (user, userDto) => (user, userDto));

        foreach (var (user, userDto) in usersWithDtos)
        {
            userDto.AvatarUrl = (user.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
        }

        return dtos;
    }

    private async Task<UserDTO> ToDto(User user, string apiUrl)
    {
        user.AvatarPath = (user.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
        user.Notifications = user.Notifications.OrderByDescending(n => n.CreatedAt).ToList();
        var dto = _mapper.Map<UserDTO>(user);
        dto.IsAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);
        return dto;
    }

    private IQueryable<User> IncludeRelations(IQueryable<User> queryable, bool withNotifications)
    {
        IQueryable<User> result = queryable
            .Include(u => u.UserMonobanks)
            .Include(u => u.Badges);

        if (withNotifications)
            result = result.Include(u => u.Notifications.Where(n => !n.IsRead)).AsSplitQuery();

        return result;
    }
}