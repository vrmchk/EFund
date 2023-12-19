namespace EFund.BLL.Extensions;

public static class StringExtensions
{
    public static string PathToUrl(this string path, string apiUrl)
    {
        var relativePath = path.Replace(@"\", "/").Split("wwwroot/")[1];

        var url = $"{apiUrl}/{relativePath}";

        return url;
    }}