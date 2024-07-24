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
        public List<T> ImportExcel<T>(string filePath) where T : class, new()
        {
            return MiniExcel.Query<T>(filePath).ToList();
        }

        public void ExportExcel<T>(List<T> data, string filePath)
        {
            MiniExcel.SaveAs(filePath, data, true, "数据", overwriteFile: true);
        }
    }
}