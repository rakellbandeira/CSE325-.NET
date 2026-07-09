using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");

Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);
var fileTotals = new Dictionary<string, double>();
var salesTotal = CalculateSalesTotal(salesFiles, fileTotals);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(fileTotals, salesTotal, Path.Combine(salesTotalDir, "salesSummary.txt"));

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles, Dictionary<string, double> fileTotals)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        double fileTotal = data?.Total ?? 0;
        fileTotals[file] = fileTotal;

        salesTotal += fileTotal;
    }

    return salesTotal;
}

void GenerateSalesSummaryReport(Dictionary<string, double> fileTotals, double salesTotal, string outputPath)
{
    StringBuilder report = new StringBuilder();

    report.AppendLine("Sales Summary Report");
    report.AppendLine("=====================");
    report.AppendLine();
    report.AppendLine($"Total Sales: {salesTotal:C}");
    report.AppendLine();
    report.AppendLine("Details");

    foreach (var entry in fileTotals)
    {
        report.AppendLine($"filename: {entry.Key}: {entry.Value:C}");
    }

    report.AppendLine("---------------------------");

    File.WriteAllText(outputPath, report.ToString());
}

record SalesData(double Total);