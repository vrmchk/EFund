namespace EFund.WebAPI.Extensions;

public static class StringExtensions
{
    public static string ToAbsolutePath(this string source)
    {
        const string solutionName = "EFund";
        var path = Directory.GetCurrentDirectory();
        var solutionPath = path[..path.LastIndexOf(solutionName, StringComparison.Ordinal)];
        return $"{solutionPath}{source}";
    }
}