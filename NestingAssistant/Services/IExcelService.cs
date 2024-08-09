using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface IExcelService
    {
        Task ExportAsync<T>(List<T> data, string filePath);

        Task<IEnumerable<T>> ImportAsync<T>(string filePath) where T : class, new();

        Task ExportMultipleSheetsAsync(Dictionary<string, object> sheets, string filePath);

        Task<Dictionary<string, IEnumerable>> ImportMultipleSheetsAsync(string filePath, params string[] sheetNames);

        Task ExportByTemplateAsync(string filePath, string templatePath, object data);
    }
}