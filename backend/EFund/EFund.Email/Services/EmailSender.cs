using System.Reflection;
using EFund.Common.Models.Configs;
using EFund.Common.Models.Utility;
using EFund.Email.Models.Base;
using EFund.Email.Services.Interfaces;
using FluentEmail.Core;
using LanguageExt;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.Email.Services;

public class EmailSender : IEmailSender
{
    private readonly IFluentEmail _email;
    private readonly EmailConfig _emailConfig;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IFluentEmail email, EmailConfig emailConfig, ILogger<EmailSender> logger)
    {
        _email = email;
        _emailConfig = emailConfig;
        _logger = logger;
    }

    public async Task<Option<ErrorModel>> SendEmailAsync<T>(string to, T message)
        where T : EmailMessageBase
    {
        var path = $@"{_emailConfig.TemplatesPath}\{message.TemplateName}.cshtml";
        var response = await _email
            .To(to)
            .Subject(message.Subject)
            .UsingTemplateFromFile(path, message)
            .SendAsync();

        if (!response.Successful)
        {
            _logger.LogError("Errors while sending email: {0}", string.Join("\n", response.ErrorMessages));
            return new ErrorModel("Unable to send an email. Please try again later");
        }

        return None;
    }
}