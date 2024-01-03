var resultFilePath = @"D:\KPI_raw\5\CourseWork\EFund\helpers\TextFileHelper\TextFileHelper\result.txt";
var directory = @"D:\KPI_raw\5\CourseWork\EFund\backend\EFund\EFund.BLL";
var acceptedFileExtensions = new[] { ".cs" };

var filesPaths = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
    .Where(path => acceptedFileExtensions.Contains(Path.GetExtension(path)))
    .ToList();

File.Create(resultFilePath).Close();

foreach(var filePath in filesPaths)
{
    var fileContent = File.ReadAllText(filePath);
    var fileName = Path.GetFileName(filePath);
    var result = $"{fileName}\n\n{fileContent}\n\n";
    File.AppendAllText(resultFilePath, result);
}