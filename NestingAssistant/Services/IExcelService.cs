using System.Collections.Generic;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface IExcelService
    {
        Task Export<T>(List<T> data, string filePath);

        Task<IEnumerable<T>> Import<T>(string filePath) where T : class, new();
    }
}