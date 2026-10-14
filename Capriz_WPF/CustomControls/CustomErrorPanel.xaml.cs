using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomErrorPanel : UserControl
    {
        public CustomErrorPanel()
        {
            InitializeComponent();

            Loaded += (_, __) =>
            {
                var w = Window.GetWindow(this);
                if (w != null)
                {
                    w.LocationChanged += (s, e) => RepositionPopup();
                    w.SizeChanged += (s, e) => RepositionPopup();
                    w.Deactivated += (s, e) => HideErrors();
                }
            };
        }

        public void ShowErrors(string rawText)
        {
            RefreshErrors(rawText);

            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
                ErrorPopup.PlacementTarget = parentWindow;

            ErrorPopup.IsOpen = true;
        }

        public void RefreshErrors(string rawText)
        {
            var lines = (rawText ?? "")
                .Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.TrimEnd(';', ' ', '\r', '\n'))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            if (lines.Count == 0)
                lines.Add("Ошибок не обнаружено");

            ErrorsList.ItemsSource = lines;   // просто обновляем
        }

        public void HideErrors()
        {
            ErrorPopup.IsOpen = false;
        }

        public bool IsOpen => ErrorPopup.IsOpen;

        private void RepositionPopup()
        {
            if (!ErrorPopup.IsOpen) return;
            ErrorPopup.HorizontalOffset += 0.1;
            ErrorPopup.HorizontalOffset -= 0.1;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => HideErrors();
        private void btnOk_Click(object sender, RoutedEventArgs e) => HideErrors();
    }
}