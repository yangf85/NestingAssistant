using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface IExcelService
    {
        Task Export<T>(List<T> data, string filePath);

        Task<IEnumerable<T>> Import<T>(string filePath) where T : class, new();

        Task ExportMultipleSheets(Dictionary<string, object> sheets, string filePath);

        Task<Dictionary<string, IEnumerable>> ImportMultipleSheets(string filePath, params string[] sheetNames);
    }
}