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
        private readonly IProfileNesterService _service;

        [ObservableProperty]
        private ProfileNestingOptionViewModel _option = new();

        public ObservableCollection<ProfilePartViewModel> ProfileParts { get; private set; } = [];
        public ObservableCollection<ProfileMaterialViewModel> ProfileMaterials { get; private set; } = [];

        [ObservableProperty]
        private ProfileNestingSummaryViewModel _summary = new();

        public ProfileNesterViewModel(IMessageBoxService messageBox, INotificationService notification, IMapper mapper, IProfileNesterService service)
            : base(messageBox, notification, mapper)
        {
            _service = service;
        }

        [RelayCommand]
        private async Task ExportTemplate(string type)
        {
            var path = await _service.PickFolderAsync();
            if (path == null) return;

            try
            {
                await _service.ExportTemplateAsync(type, path);
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

            var filepath = await _service.PickFileAsync(title, new[] { "*.xlsx" });
            if (filepath == null) return;

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
            var parts = await _service.ImportPartDataAsync(filepath);
            ProfileParts.Clear();
            foreach (var item in parts)
            {
                ProfileParts.Add(Mapper.Map<ProfilePartViewModel>(item));
            }
        }

        private async Task ImportMaterialData(string filepath)
        {
            var materials = await _service.ImportMaterialDataAsync(filepath);
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

            var summary = await _service.RunAsync(parts, materials, option);
            Summary = Mapper.Map<ProfileNestingSummaryViewModel>(summary);

            OnPropertyChanged(nameof(Summary));
        }

        [RelayCommand]
        private async Task ExportNestingSummary()
        {
            var filepath = await _service.SaveFileAsync("保存套裁数据", "ProfileNestingResult", "xlsx", ["*.xlsx"]);
            if (filepath == null) return;

            var data = Mapper.Map<ProfileNestingSummary>(Summary);
            await _service.SaveNestingSummaryAsync(filepath, data);
        }
    }
}