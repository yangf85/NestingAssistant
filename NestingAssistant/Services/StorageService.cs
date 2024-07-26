using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface IStorageService
    {
        Task<IReadOnlyList<IStorageFile>> OpenFilesAsync(FilePickerOpenOptions options = null);

        Task<IStorageFolder> OpenFolderAsync(FolderPickerOpenOptions options = null);

        Task<IStorageFile> SaveFileAsync(FilePickerSaveOptions options = null);
    }

    public class StorageService : IStorageService
    {
        private readonly TopLevel _topLevel;

        public StorageService(TopLevel topLevel)
        {
            _topLevel = topLevel;
        }

        public async Task<IReadOnlyList<IStorageFile>> OpenFilesAsync(FilePickerOpenOptions options = null)
        {
            options ??= new FilePickerOpenOptions
            {
                Title = "Select Files",
                AllowMultiple = true,
                FileTypeFilter = new[] { new FilePickerFileType("All Files") { Patterns = new[] { "*" } } }
            };

            var filePicker = await _topLevel.StorageProvider.OpenFilePickerAsync(options);
            return filePicker;
        }

        public async Task<IStorageFolder> OpenFolderAsync(FolderPickerOpenOptions options = null)
        {
            options ??= new FolderPickerOpenOptions
            {
                Title = "Select Folder"
            };

            var folderPicker = await _topLevel.StorageProvider.OpenFolderPickerAsync(options);
            return folderPicker.FirstOrDefault();
        }

        public async Task<IStorageFile> SaveFileAsync(FilePickerSaveOptions options = null)
        {
            options ??= new FilePickerSaveOptions
            {
                Title = "Save File",
                SuggestedFileName = "NewFile",
                FileTypeChoices = new[]
                {
                     new FilePickerFileType("Text File")
                     {
                         Patterns = new[] { "*.txt"}
                     }
                }
            };

            var savePicker = await _topLevel.StorageProvider.SaveFilePickerAsync(options);
            return savePicker;
        }
    }
}