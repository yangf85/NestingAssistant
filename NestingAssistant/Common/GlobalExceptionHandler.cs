using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace NestingAssistant.Common
{
    public static class GlobalExceptionHandler
    {
        public static void Register()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            Dispatcher.UIThread.UnhandledException += OnUIThreadUnhandledException;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.SetObserved();
        }

        private static void OnUIThreadUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.Handled = true;
        }

        private static void HandleException(Exception exception)
        {
            if (exception != null)
            {
                LogException(exception);
                ShowExceptionMessage(exception);
            }
        }

        private static void LogException(Exception exception)
        {
            // 日志记录异常信息
            Console.WriteLine(exception.ToString());
        }

        private static void ShowExceptionMessage(Exception exception)
        {
            // 显示异常信息给用户
            MessageBox.ShowAsync("未捕获的异常: " + exception.Message, "错误提示", MessageBoxIcon.Error, MessageBoxButton.OK);
        }
    }
}