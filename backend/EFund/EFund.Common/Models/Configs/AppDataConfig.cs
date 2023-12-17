using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class AppDataConfig : ConfigBase
{
    public string AppDataPath { get; set; } = string.Empty;
    public string LogDirectory { get; set; } = string.Empty;
    public string WebRootPath { get; set; } = string.Empty;
    public string UploadsDirectory { get; set; } = string.Empty;
    public string UsersDirectory { get; set; } = string.Empty;
    public string DefaultDirectory { get; set; } = string.Empty;
    public List<string> AllowedFileTypes { get; set; } = new();
    public string AvatarFileName { get; set; } = string.Empty;
    public string DefaultAvatarFileExtension { get; set; } = string.Empty;

    public string UserAvatarDirectory => Path.Combine(WebRootPath, UploadsDirectory, UsersDirectory);
    public string DefaultAvatarPath => Path.Combine(WebRootPath, UploadsDirectory, DefaultDirectory, UsersDirectory, AvatarFileName + DefaultAvatarFileExtension);
}