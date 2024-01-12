namespace EFund.BLL.Extensions;

public static class StringExtensions
{
    public static string PathToUrl(this string path, string apiUrl)
    {
        var relativePath = path.Replace(@"\", "/").Split("wwwroot/")[1];

        var url = $"{apiUrl}/{relativePath}";

        return url;
    }

    public static string ToAbsolutePath(this string source)
    {
        var path = Directory.GetCurrentDirectory();
#if DEBUG
        const string solutionName = "EFund";
        var solutionPath = path[..path.LastIndexOf(solutionName, StringComparison.Ordinal)];
        return $"{solutionPath}{source}";
#else
        return Path.Combine(path, source);
#endif
    }
}