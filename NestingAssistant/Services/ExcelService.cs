using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public class ExcelService : IExcelService
    {
        public Task<IEnumerable<T>> ImportAsync<T>(string filePath) where T : class, new()
        {
            return MiniExcel.QueryAsync<T>(filePath);
        }

        public Task ExportAsync<T>(List<T> data, string filePath)
        {
            var config = new OpenXmlConfiguration()
            {
                //TableStyles = TableStyles.None,
                AutoFilter = false,
            };
            return MiniExcel.SaveAsAsync(filePath, data, true, "数据", overwriteFile: true, configuration: config);
        }

        public Task ExportMultipleSheetsAsync(Dictionary<string, object> sheets, string filePath)
        {
            var config = new OpenXmlConfiguration()
            {
                //TableStyles = TableStyles.None,
                AutoFilter = false,
            };
            return MiniExcel.SaveAsAsync(filePath, sheets, true, overwriteFile: true, configuration: config);
        }

        public async Task<Dictionary<string, IEnumerable>> ImportMultipleSheetsAsync(string filePath, params string[] sheetNames)
        {
            var dictionary = new Dictionary<string, IEnumerable>();

            foreach (var sheetName in sheetNames)
            {
                dictionary[sheetName] = await MiniExcel.QueryAsync<object>(filePath, sheetName: sheetName);
            }

            return dictionary;
        }

        public Task ExportByTemplateAsync(string filePath, string templatePath, object data)
        {
            return MiniExcel.SaveAsByTemplateAsync(filePath, templatePath, data);
        }
    }
}