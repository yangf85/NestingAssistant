using System.Collections.Generic;

namespace NestingAssistant.Services
{
    public interface IExcelService
    {
        void ExportExcel<T>(List<T> data, string filePath);

        List<T> ImportExcel<T>(string filePath) where T : class, new();
    }
}