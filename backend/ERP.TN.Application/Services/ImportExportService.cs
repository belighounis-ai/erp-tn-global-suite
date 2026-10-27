using System.Text;
using ClosedXML.Excel;
using ERP.TN.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;

namespace ERP.TN.Application.Services;

public interface IImportExportService
{
    Task<ImportExportTemplateDto> GetTemplateAsync(string type, string format = "xlsx");
    Task<byte[]> ExportAsync(string type, string format = "xlsx");
    Task<ImportExportResultDto> ImportAsync(string type, IFormFile file);
}

public class ImportExportService : IImportExportService
{
    private readonly Dictionary<string, List<string>> _templates = new()
    {
        ["product"] = new() { "Code", "Name", "Unit", "BuyingPrice", "SellingPrice", "StockAlertLevel" },
        ["customer"] = new() { "Name", "Email", "Phone", "TaxNumber" },
        ["supplier"] = new() { "Name", "Email", "Phone", "TaxNumber" },
        ["chartaccount"] = new() { "Code", "Name", "Type" },
        ["voucher"] = new() { "Number", "JournalCode", "Reference", "AccountCode", "AccountName", "DebitCredit", "Amount" },
        ["employee"] = new() { "FirstName", "LastName", "Position", "MonthlySalary" },
        ["journal"] = new() { "Code", "Name" }
    };

    public Task<ImportExportTemplateDto> GetTemplateAsync(string type, string format = "xlsx")
    {
        type = NormalizeType(type);
        var headers = _templates.TryGetValue(type, out var values) ? values : _templates["product"];

        var sampleRows = new List<Dictionary<string, string>>
        {
            BuildSampleRow(type)
        };

        return Task.FromResult(new ImportExportTemplateDto
        {
            Type = type,
            Format = format,
            Headers = headers,
            SampleRows = sampleRows
        });
    }

    public async Task<byte[]> ExportAsync(string type, string format = "xlsx")
    {
        type = NormalizeType(type);
        var template = await GetTemplateAsync(type, format);

        if (string.Equals(format, "csv", StringComparison.OrdinalIgnoreCase))
        {
            return BuildCsv(template);
        }

        return BuildXlsx(template);
    }

    public async Task<ImportExportResultDto> ImportAsync(string type, IFormFile file)
    {
        var result = new ImportExportResultDto { Type = NormalizeType(type) };

        if (file is null || file.Length == 0)
        {
            result.Errors.Add("Aucun fichier fourni.");
            result.RejectedRows = 1;
            return result;
        }

        var fileName = file.FileName ?? "import";
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        List<Dictionary<string, string>> rows;

        if (extension == ".csv")
        {
            rows = ReadCsvRows(file.OpenReadStream());
        }
        else if (extension == ".xlsx" || extension == ".xls")
        {
            rows = ReadExcelRows(file.OpenReadStream());
        }
        else
        {
            result.Errors.Add("Format non pris en charge. Utilisez .csv ou .xlsx.");
            result.RejectedRows = 1;
            return result;
        }

        var expectedHeaders = _templates.TryGetValue(type, out var headers)
            ? headers
            : _templates["product"];

        result.TotalRows = rows.Count;

        foreach (var row in rows)
        {
            if (!IsValidRow(result.Type, row, expectedHeaders))
            {
                result.RejectedRows++;
                result.Errors.Add(BuildErrorMessage(row));
                continue;
            }

            result.ImportedRows++;
        }

        return result;
    }

    private static string NormalizeType(string type)
    {
        return (type ?? "product").Trim().ToLowerInvariant();
    }

    private static Dictionary<string, string> BuildSampleRow(string type)
    {
        return type switch
        {
            "customer" => new Dictionary<string, string>
            {
                ["Name"] = "Client Test",
                ["Email"] = "client@test.com",
                ["Phone"] = "+21600000000",
                ["TaxNumber"] = "12345678"
            },
            "supplier" => new Dictionary<string, string>
            {
                ["Name"] = "Fournisseur Test",
                ["Email"] = "fournisseur@test.com",
                ["Phone"] = "+21600000000",
                ["TaxNumber"] = "98765432"
            },
            "chartaccount" => new Dictionary<string, string>
            {
                ["Code"] = "401",
                ["Name"] = "Fournisseurs",
                ["Type"] = "liability"
            },
            "voucher" => new Dictionary<string, string>
            {
                ["Number"] = "PC-001",
                ["JournalCode"] = "ACH",
                ["Reference"] = "Achat fournisseur",
                ["AccountCode"] = "601",
                ["AccountName"] = "Achats",
                ["DebitCredit"] = "DEBIT",
                ["Amount"] = "2500"
            },
            "employee" => new Dictionary<string, string>
            {
                ["FirstName"] = "Ali",
                ["LastName"] = "Ben Ali",
                ["Position"] = "Comptable",
                ["MonthlySalary"] = "2500"
            },
            "journal" => new Dictionary<string, string>
            {
                ["Code"] = "GEN",
                ["Name"] = "Journal Général"
            },
            _ => new Dictionary<string, string>
            {
                ["Code"] = "P001",
                ["Name"] = "Produit Test",
                ["Unit"] = "UN",
                ["BuyingPrice"] = "12.50",
                ["SellingPrice"] = "18.00",
                ["StockAlertLevel"] = "10"
            }
        };
    }

    private static bool IsValidRow(string type, Dictionary<string, string> row, List<string> expectedHeaders)
    {
        foreach (var header in expectedHeaders)
        {
            if (!row.ContainsKey(header))
                return false;

            var value = row[header]?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (header == "Type" && type == "chartaccount")
                    return false;
                if (header == "Amount" && type == "voucher")
                    return false;
                if (header == "MonthlySalary" && type == "employee")
                    return false;
            }
        }

        return true;
    }

    private static string BuildErrorMessage(Dictionary<string, string> row)
    {
        var details = string.Join("; ", row.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        return details.Length > 200 ? details[..197] + "..." : details;
    }

    private static byte[] BuildCsv(ImportExportTemplateDto template)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", template.Headers));

        foreach (var row in template.SampleRows)
        {
            sb.AppendLine(string.Join(",", template.Headers.Select(h => EscapeCsv(row.GetValueOrDefault(h, "")))));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static byte[] BuildXlsx(ImportExportTemplateDto template)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Template");

        for (var i = 0; i < template.Headers.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = template.Headers[i];
        }

        for (var r = 0; r < template.SampleRows.Count; r++)
        {
            var row = template.SampleRows[r];
            for (var c = 0; c < template.Headers.Count; c++)
            {
                worksheet.Cell(r + 2, c + 1).Value = row.GetValueOrDefault(template.Headers[c], string.Empty);
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static List<Dictionary<string, string>> ReadCsvRows(Stream stream)
    {
        var rows = new List<Dictionary<string, string>>();

        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, leaveOpen: true);
        var line = reader.ReadLine();
        if (line is null)
            return rows;

        var headers = ParseCsvLine(line);
        while (!reader.EndOfStream)
        {
            var nextLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(nextLine)) continue;
            var values = ParseCsvLine(nextLine);
            if (values.Count == 0) continue;

            var row = new Dictionary<string, string>();
            for (var i = 0; i < headers.Count && i < values.Count; i++)
            {
                row[headers[i]] = values[i];
            }

            rows.Add(row);
        }

        return rows;
    }

    private static List<Dictionary<string, string>> ReadExcelRows(Stream stream)
    {
        var rows = new List<Dictionary<string, string>>();

        stream.Position = 0;
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();
        var rowCount = worksheet.LastRowUsed()?.RowNumber ?? 0;
        if (rowCount < 2)
            return rows;

        var headers = new List<string>();
        var firstRow = worksheet.Row(1);
        foreach (var cell in firstRow.Cells())
        {
            headers.Add(cell.GetString());
        }

        for (var rowIndex = 2; rowIndex <= rowCount; rowIndex++)
        {
            var row = worksheet.Row(rowIndex);
            var dict = new Dictionary<string, string>();
            for (var i = 0; i < headers.Count; i++)
            {
                dict[headers[i]] = row.Cell(i + 1).GetString();
            }

            if (dict.Values.All(v => string.IsNullOrWhiteSpace(v)))
                continue;

            rows.Add(dict);
        }

        return rows;
    }

    private static List<string> ParseCsvLine(string line)
    {
        using var parser = new TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(line)))
        {
            TextFieldType = FieldType.Delimited,
            Delimiters = new[] { "," }
        };

        var values = new List<string>();
        while (!parser.EndOfData)
        {
            var field = parser.ReadFields();
            if (field is not null)
            {
                values.AddRange(field.Select(f => f.Trim()));
            }
        }

        return values;
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        return value;
    }
}
