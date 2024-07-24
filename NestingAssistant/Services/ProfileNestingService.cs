using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public class ProfileNestingService
    {
        private readonly IExcelService _excel;

        public ProfileNestingService(IExcelService excel)
        {
            _excel = excel;
        }
    }
}