using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace NestingAssistant.Services
{
    public interface IMessageBoxService
    {
        Task<MessageBoxResult> ShowAsync(string message, string? title = "提示", MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxButton button = MessageBoxButton.OKCancel);

        Task<MessageBoxResult> ShowOverlayAsync(string message, string? title = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxButton button = MessageBoxButton.OKCancel);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public Task<MessageBoxResult> ShowAsync(string message, string? title = "提示", MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxButton button = MessageBoxButton.OKCancel)
        {
            return MessageBox.ShowAsync(message, title, icon, button);
        }

        public Task<MessageBoxResult> ShowOverlayAsync(string message, string? title = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxButton button = MessageBoxButton.OKCancel)
        {
            return MessageBox.ShowOverlayAsync(message, title, null, icon, button, null);
        }
    }
}