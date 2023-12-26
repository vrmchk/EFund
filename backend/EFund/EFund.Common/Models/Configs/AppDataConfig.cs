using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class AppDataConfig : ConfigBase
{
    public string AppDataPath { get; set; } = string.Empty;
    public string SeedingDataPath { get; set; } = string.Empty;
    public string LogDirectory { get; set; } = string.Empty;
    public string WebRootPath { get; set; } = string.Empty;
    public string UploadsDirectory { get; set; } = string.Empty;
    public string UsersDirectory { get; set; } = string.Empty;
    public string DefaultDirectory { get; set; } = string.Empty;
    public Dictionary<string, string> AllowedFiles { get; set; } = new();
    public Dictionary<string, string> AllowedImages { get; set; } = new();
    public string AvatarFileName { get; set; } = string.Empty;
    public string DefaultAvatarFileExtension { get; set; } = string.Empty;
    public string FundraisingsDirectory { get; set; } = string.Empty;
    public string ReportsDirectory { get; set; } = string.Empty;
    public string AttachmentsDirectory { get; set; } = string.Empty;

    public string UserAvatarDirectoryPath => Path.Combine(WebRootPath, UploadsDirectory, UsersDirectory);
    public string FundraisingsDirectoryPath => Path.Combine(WebRootPath, UploadsDirectory, FundraisingsDirectory);
    public string DefaultUserAvatarPath => Path.Combine(WebRootPath, UploadsDirectory, DefaultDirectory, UsersDirectory, AvatarFileName + DefaultAvatarFileExtension);
    public string DefaultFundraisingAvatarPath => Path.Combine(WebRootPath, UploadsDirectory, DefaultDirectory, FundraisingsDirectory, AvatarFileName + DefaultAvatarFileExtension);
}