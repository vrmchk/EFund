namespace EFund.BLL.Extensions;

public static class StringExtensions
{
    public static string MimeTypeToFileExtension(this string mimeType)
    {
        var parts = mimeType.Split('/');
        if (parts.Length == 2)
            return "." + parts[1];

        return string.Empty;
    }

    public static string PathToUrl(this string path, string apiUrl)
    {
        var relativePath = path.Replace(@"\", "/").Split("wwwroot/")[1];

        var url = $"{apiUrl}/{relativePath}";

        return url;
    }}