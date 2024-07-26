using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public class ExcelService : IExcelService
    {
        public Task<IEnumerable<T>> Import<T>(string filePath) where T : class, new()
        {
            return MiniExcel.QueryAsync<T>(filePath);
        }

        public Task Export<T>(List<T> data, string filePath)
        {
            return MiniExcel.SaveAsAsync(filePath, data, true, "数据", overwriteFile: true);
        }
    }
}