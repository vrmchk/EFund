using EFund.Common.Models.Configs.Abstract;

namespace EFund.WebAPI.Utility;

public class ConfigurationServiceBuilder
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ConfigurationServiceBuilder(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    public ConfigurationServiceBuilder AddConfig<T>(string? sectionName = null,
        Action<T>? configureOptions = null)
        where T : ConfigBase, new()
    {
        CreateAndAddConfig(sectionName, configureOptions);
        return this;
    }

    public ConfigurationServiceBuilder AddConfig<T>(out T config, string? sectionName = null,
        Action<T>? configureOptions = null)
        where T : ConfigBase, new()
    {
        config = CreateAndAddConfig(sectionName, configureOptions);
        return this;
    }

    private T CreateAndAddConfig<T>(string? sectionName, Action<T>? configureOptions)
        where T : ConfigBase, new()
    {
        var config = new T();

        var configSection = _configuration.GetSection(sectionName ?? typeof(T).Name);

        if (!configSection.Exists())
            configSection = _configuration.GetSection(typeof(T).Name.Replace("Config", string.Empty));

        if (!configSection.Exists())
            throw new ArgumentException("Failed to find specified config section");

        configSection.Bind(config);

        configureOptions?.Invoke(config);

        _services.AddSingleton(config);

        return config;
    }
}