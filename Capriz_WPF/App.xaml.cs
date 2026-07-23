using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Capriz_WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Обработчик исключений в UI-потоке
            this.DispatcherUnhandledException += (sender, args) =>
            {
                ShowError(args.Exception);
                args.Handled = true; // Предотвращаем завершение приложения
            };

            // Обработчик для всех остальных необработанных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    ShowError(ex);
                }
            };
        }

        private void ShowError(Exception ex)
        {
            //string errorMessage = $"Произошла ошибка:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}";

            //MessageBox.Show(
            //    errorMessage,
            //    "Ошибка",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Error
            //);
        }
    }
}
