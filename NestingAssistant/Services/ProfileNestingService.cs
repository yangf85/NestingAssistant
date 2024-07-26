using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public class ProfileNestingService
    {
        public IExcelService Excel { get; private set; }

        public IStorageService Storage { get; private set; }

        public ProfileNestingService(IExcelService excel, IStorageService storage)
        {
            Excel = excel;
            Storage = storage;
        }
    }
}