using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomMessageBox : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        public CustomMessageBox()
        {
            InitializeComponent();
            PreviewKeyDown += OnPreviewKeyDown;
        }

        // ---------- Публичное API ----------
        public static MessageBoxResult Show(
            string messageBoxText,
            string caption = "",
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            Window owner = null)
        {
            var box = new CustomMessageBox();
            box.MessageText.Text = messageBoxText ?? "";
            box.TitleText.Text = caption ?? "";

            box.ConfigureButtons(button);
            box.ConfigureIcon(icon);

            box.Owner = owner ?? Application.Current?.MainWindow;

            box.ShowDialog();
            return box.Result;
        }

        // ---------- Настройка кнопок ----------
        private void ConfigureButtons(MessageBoxButton button)
        {
            // Скрываем все
            YesBtn.Visibility = Visibility.Collapsed;
            NoBtn.Visibility = Visibility.Collapsed;
            OkBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;

            switch (button)
            {
                case MessageBoxButton.OK:
                    OkBtn.Visibility = Visibility.Visible;
                    OkBtn.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    OkBtn.Visibility = Visibility.Visible;
                    CancelBtn.Visibility = Visibility.Visible;
                    OkBtn.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    YesBtn.Visibility = Visibility.Visible;
                    NoBtn.Visibility = Visibility.Visible;
                    YesBtn.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    YesBtn.Visibility = Visibility.Visible;
                    NoBtn.Visibility = Visibility.Visible;
                    CancelBtn.Visibility = Visibility.Visible;
                    YesBtn.Focus();
                    break;
            }
        }

        // ---------- Настройка иконки ----------
        private void ConfigureIcon(MessageBoxImage icon)
        {
            switch (icon)
            {
                case MessageBoxImage.Warning:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xA5, 0x00));
                    IconText.Text = "!";
                    break;
                case MessageBoxImage.Error:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0xD3, 0x2F, 0x2F));
                    IconText.Text = "✕";
                    break;
                case MessageBoxImage.Information:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0x3D, 0x60, 0x87));
                    IconText.Text = "i";
                    break;
                case MessageBoxImage.Question:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0x3D, 0x60, 0x87));
                    IconText.Text = "?";
                    break;
                default:
                    // Иконка не задана — скрываем её, текст растягивается на всю ширину
                    IconCircle.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(MessageText, 0);
                    Grid.SetColumnSpan(MessageText, 2);
                    break;
            }
        }

        // ---------- Обработчики ----------
        private void YesBtn_Click(object sender, RoutedEventArgs e) { Result = MessageBoxResult.Yes; Close(); }
        private void NoBtn_Click(object sender, RoutedEventArgs e) { Result = MessageBoxResult.No; Close(); }
        private void OkBtn_Click(object sender, RoutedEventArgs e) { Result = MessageBoxResult.OK; Close(); }
        private void CancelBtn_Click(object sender, RoutedEventArgs e) { Result = MessageBoxResult.Cancel; Close(); }
        private void CloseBtn_Click(object sender, RoutedEventArgs e) { Result = MessageBoxResult.Cancel; Close(); }

        // ---------- Перетаскивание ----------
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        // ---------- Горячие клавиши ----------
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Result = MessageBoxResult.Cancel;
                Close();
            }
            else if (e.Key == Key.Enter)
            {
                if (YesBtn.Visibility == Visibility.Visible) Result = MessageBoxResult.Yes;
                else if (OkBtn.Visibility == Visibility.Visible) Result = MessageBoxResult.OK;
                else if (NoBtn.Visibility == Visibility.Visible) Result = MessageBoxResult.No;
                else Result = MessageBoxResult.Cancel;
                Close();
            }
        }
    }
}