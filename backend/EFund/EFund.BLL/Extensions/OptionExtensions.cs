using EFund.Common.Enums;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.Utility;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EFund.BLL.Extensions;

public static class OptionExtensions
{
    public static Option<ErrorDTO> ToErrorDTO(this Option<ErrorModel> option, ErrorCode errorCode)
    {
        return option.Match<Option<ErrorDTO>>(
            Some: error => new ErrorDTO(errorCode, error.Message),
            None: () => None
        );
    }
}