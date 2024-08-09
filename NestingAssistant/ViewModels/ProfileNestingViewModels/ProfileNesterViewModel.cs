using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapsterMapper;
using NestingAssistant.Models;
using NestingAssistant.Services;
using ProfileOptimizer.Nesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNesterViewModel : BasicViewModel
    {
        private ProfileNestingService _service;

        public ProfileNestingOptionViewModel Option { get; set; } = new();

        public ObservableCollection<ProfilePartViewModel> ProfileParts { get; private set; } = [];

        public ObservableCollection<ProfileMaterialViewModel> ProfileMaterials { get; private set; } = [];

        public ProfileNestingSummaryViewModel Summary { get; set; } = new();

        public ProfileNesterViewModel(IMessageBoxService messageBox, INotificationService notification, IMapper mapper, ProfileNestingService service) : base(messageBox, notification, mapper)
        {
            _service = service;
        }

        [RelayCommand]
        private async Task ExportTemplate(string type)
        {
            var folder = await _service.Storage.OpenFolderAsync();
            var path = folder?.TryGetLocalPath();
            if (path == null)
            {
                return;
            }

            try
            {
                switch (type)
                {
                    case "Part":
                        await _service.Excel.ExportAsync(new List<ProfilePartModel>(), Path.Combine(path, "ProfilePartTemplate.xlsx"));
                        break;

                    case "Material":
                        await _service.Excel.ExportAsync(new List<ProfileMaterialModel>(), Path.Combine(path, "ProfileMaterialTemplate.xlsx"));
                        break;
                }

                Notification.ShowNotification("导出成功", type: NotificationType.Success);
            }
            catch (Exception ex)
            {
                await MessageBox.ShowOverlayAsync($"导出失败{ex.Message}", "错误提示", MessageBoxIcon.Error, MessageBoxButton.OK);
            }
        }

        [RelayCommand]
        private async Task ImportBasicData(string type)
        {
            var title = type switch
            {
                "Part" => "选择零件数据",
                "Material" => "选择材料数据",
                _ => "选择文件数据",
            };

            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("Excel Files") { Patterns = ["*.xlsx"] }
                ]
            };

            var files = await _service.Storage.OpenFilesAsync(options);

            if (files.Count == 0)
            {
                return;
            }
            var filepath = files[0].Path.LocalPath;

            try
            {
                switch (type)
                {
                    case "Part":
                        await ImportPartData(filepath);
                        break;

                    case "Material":
                        await ImportMaterialData(filepath);
                        break;

                    default:
                        return;
                }
                Notification.ShowNotification("导入成功", type: NotificationType.Success);
            }
            catch (Exception ex)
            {
                await MessageBox.ShowOverlayAsync($"导入失败{ex.Message}", "错误提示", MessageBoxIcon.Error, MessageBoxButton.OK);
            }
        }

        private async Task ImportPartData(string filepath)
        {
            var parts = await _service.Excel.ImportAsync<ProfilePartModel>(filepath);
            ProfileParts.Clear();

            foreach (var item in parts)
            {
                ProfileParts.Add(Mapper.Map<ProfilePartViewModel>(item));
            }
        }

        private async Task ImportMaterialData(string filepath)
        {
            var materials = await _service.Excel.ImportAsync<ProfileMaterialModel>(filepath);
            ProfileMaterials.Clear();

            foreach (var item in materials)
            {
                ProfileMaterials.Add(Mapper.Map<ProfileMaterialViewModel>(item));
            }
        }

        [RelayCommand]
        private async Task Run()
        {
            var parts = ProfileParts.Select(i => Mapper.Map<ProfilePart>(i)).ToList();
            var materials = ProfileMaterials.Select(i => Mapper.Map<ProfileMaterial>(i)).ToList();
            var option = Mapper.Map<ProfileNestingOption>(Option);

            var summary = await _service.Run(parts, materials, option);

            Summary = Mapper.Map<ProfileNestingSummaryViewModel>(summary);

            OnPropertyChanged(nameof(Summary));
        }

        [RelayCommand]
        private async Task ExportNestingSummary()
        {
            var option = new FilePickerSaveOptions()
            {
                DefaultExtension = "xlsx",
                ShowOverwritePrompt = true,
                Title = "保存套裁数据",
                SuggestedFileName = "ProfileNestingResult",
                FileTypeChoices = [new FilePickerFileType("Excel Files") { Patterns = ["*.xlsx"] }]
            };

            var file = await _service.Storage.SaveFileAsync(option);
            if (file == null)
            {
                return;
            }

            var filepath = file.TryGetLocalPath();

            var data = Mapper.Map<ProfileNestingSummary>(Summary);

            //dict["数据汇总"] = NestingResult.Summaries.Select(i => Mapper.Map<ProfileNestingSummaryModel>(i));
            //dict["排版汇总"] = NestingResult.Materials.Select(i => Mapper.Map<UsageProfileMaterialModel>(i));

            await _service.Excel.ExportByTemplateAsync(filepath, @"F:\OneDrive\桌面\ProfileNestingSummaryTemplate.xlsx", data);
        }
    }
}