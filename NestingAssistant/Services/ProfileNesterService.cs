using Avalonia.Platform.Storage;
using NestingAssistant.Models;
using NestingAssistant.ViewModels;
using ProfileOptimizer.Nesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface IProfileNesterService
    {
        Task<ProfileNestingSummary> RunAsync(IEnumerable<ProfilePart> parts, IEnumerable<ProfileMaterial> materials, ProfileNestingOption option);

        Task<IEnumerable<ProfilePartModel>> ImportPartDataAsync(string filepath);

        Task<IEnumerable<ProfileMaterialModel>> ImportMaterialDataAsync(string filepath);

        Task ExportTemplateAsync(string type, string path);

        Task SaveNestingSummaryAsync(string filepath, ProfileNestingSummary data);

        Task<string> PickFolderAsync();

        Task<string> PickFileAsync(string title, string[] patterns);

        Task<string> SaveFileAsync(string title, string defaultFileName, string defaultExtension, string[] patterns);
    }

    public class ProfileNesterService : IProfileNesterService
    {
        public IExcelService Excel { get; private set; }
        public IStorageService Storage { get; private set; }

        public ProfileNesterService(IExcelService excel, IStorageService storage)
        {
            Excel = excel;
            Storage = storage;
        }

        public Task<ProfileNestingSummary> RunAsync(IEnumerable<ProfilePart> parts, IEnumerable<ProfileMaterial> materials, ProfileNestingOption option)
        {
            var nester = new GreedyProfileNester(parts.ToList(), materials.ToList(), option);
            return nester.NestAsync();
        }

        public async Task<IEnumerable<ProfilePartModel>> ImportPartDataAsync(string filepath)
        {
            return await Excel.ImportAsync<ProfilePartModel>(filepath);
        }

        public async Task<IEnumerable<ProfileMaterialModel>> ImportMaterialDataAsync(string filepath)
        {
            return await Excel.ImportAsync<ProfileMaterialModel>(filepath);
        }

        public async Task ExportTemplateAsync(string type, string path)
        {
            switch (type)
            {
                case "Part":
                    await Excel.ExportAsync(new List<ProfilePartModel>(), System.IO.Path.Combine(path, "ProfilePartTemplate.xlsx"));
                    break;

                case "Material":
                    await Excel.ExportAsync(new List<ProfileMaterialModel>(), System.IO.Path.Combine(path, "ProfileMaterialTemplate.xlsx"));
                    break;
            }
        }

        public async Task SaveNestingSummaryAsync(string filepath, ProfileNestingSummary data)
        {
            var templatePath = System.IO.Path.Combine(AppContext.BaseDirectory, "ExcelTemplates", "ProfileNestingSummaryTemplate.xlsx");
            await Excel.ExportByTemplateAsync(filepath, templatePath, data);
        }

        public async Task<string> PickFolderAsync()
        {
            var folder = await Storage.OpenFolderAsync();
            return folder?.TryGetLocalPath();
        }

        public async Task<string> PickFileAsync(string title, string[] patterns)
        {
            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = patterns.Select(pattern => new FilePickerFileType("Excel Files") { Patterns = [pattern] }).ToList()
            };

            var files = await Storage.OpenFilesAsync(options);
            return files.Count > 0 ? files[0].Path.LocalPath : null;
        }

        public async Task<string> SaveFileAsync(string title, string defaultFileName, string defaultExtension, string[] patterns)
        {
            var option = new FilePickerSaveOptions()
            {
                DefaultExtension = defaultExtension,
                ShowOverwritePrompt = true,
                Title = title,
                SuggestedFileName = defaultFileName,
                FileTypeChoices = patterns.Select(pattern => new FilePickerFileType("Excel Files") { Patterns = [pattern] }).ToList()
            };

            var file = await Storage.SaveFileAsync(option);
            return file?.TryGetLocalPath();
        }
    }
}