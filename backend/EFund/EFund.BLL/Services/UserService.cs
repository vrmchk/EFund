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

    public async Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id, string apiUrl)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        return ToDto<UserDTO>(user, apiUrl);
    }

    public async Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid userId, UpdateUserDTO dto, string apiUrl)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
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

        return ToDto<UserDTO>(user, apiUrl);
    }

    public async Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var either = await _userRegistrationService.GenerateEmailConfirmationCodeAsync(userId);
        return await either.Match<Task<Option<ErrorDTO>>>(
            Right: async code =>
            {
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
            },
            Left: error =>
            {
                _logger.LogError("Unable to generate email confirmation code for user {0}", user.Id);
                return Task.FromResult<Option<ErrorDTO>>(new IncorrectParametersErrorDTO(error.Message));
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

        if (user.AvatarPath != null)
            File.Delete(user.AvatarPath);

        var directory = Path.Combine(_appDataConfig.UserAvatarDirectoryPath, user.Id.ToString("N"));

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        user.AvatarPath = Path.Combine(directory,
            $"{_appDataConfig.AvatarFileName}{_appDataConfig.AllowedImages[file.ContentType]}");

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
        var encodedToken = HttpUtility.UrlEncode(token);
        var callbackUri = string.Format(_callbackUrisConfig.InviteUserUriTemplate, user.Email, encodedToken);

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
        string apiUrl)
    {
        IQueryable<User> queryable = _userManager.Users;
        if (dto.UserIds?.Count > 0)
            queryable = queryable.Where(u => dto.UserIds.Contains(u.Id));

        if (dto.Emails?.Count > 0)
            queryable = queryable.Where(u => dto.Emails.Contains(u.Email!));

        if (dto.UserNames?.Count > 0)
            queryable = queryable.Where(u => dto.UserNames.Contains(u.DisplayName));

        if (dto.CreatedByAdmin != null)
            queryable = queryable.Where(u => u.CreatedByAdmin == dto.CreatedByAdmin);

        if (dto.IsBlocked != null)
            queryable = queryable.Where(u => u.IsBlocked == dto.IsBlocked);

        if (dto.EmailConfirmed != null)
            queryable = queryable.Where(u => u.EmailConfirmed == dto.EmailConfirmed);

        var users = await queryable.ToPagedListAsync(pagination.PageNumber, pagination.PageSize);
        var dtos = _mapper.Map<PagedListDTO<UserExtendedDTO>>(users);
        var usersWithDtos = users
            .Zip(dtos.Items, (user, userDto) => (user, userDto));

        foreach (var (user, userDto) in usersWithDtos)
        {
            user.AvatarPath = (user.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
            userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();
        }

        return dtos;
    }

    private T ToDto<T>(User user, string apiUrl)
    {
        user.AvatarPath = (user.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
        return _mapper.Map<T>(user);
    }
}