using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomDataPanel.xaml
    /// </summary>
    public partial class CustomDataPanel : UserControl
    {
        List<TextBlock> textBlocks;

        List<TextBlock> skydexTextBlocks;

        List<TextBlock> ngoTextBlocks;

        private Style _basicStyle;
        private Style _smallStyle;

        private int _lastCount = -1;

        public CustomDataPanel()
        {
            InitializeComponent();
            _basicStyle = (Style)FindResource("CellValue");
            _smallStyle = (Style)FindResource("CellValueSmall");

            textBlocks = new List<TextBlock>() { DataTemp, DataHum, DataPressMm, DataPressGPa, DataBarT, DataTrend, DataClouds, DataDMDV1,
            DataDMDV10, DataNgo1, DataNgo2, DataNgo3, DataHm0, DataHmax};

            skydexTextBlocks = new List<TextBlock>() { LabelAmountClouds, LabelNgo3, DataClouds, DataNgo3, LabelNgo2, LabelNgo1, DataNgo2, DataNgo1 };
            ngoTextBlocks = new List<TextBlock>() { DataNgo1, DataNgo2, DataNgo3 };
        }

        public void SetDataToFields(List<string> data, int skydexIndex)
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = data[i];

            switch (skydexIndex)
            {
                case 0:
                    HideSkydexDataShowIndex("Облака не обнаружены");
                    break;
                case 1:
                    ShowSkydexDataHideIndex(1);
                    break;
                case 2:
                    ShowSkydexDataHideIndex(2);
                    break;
                case 3:
                    ShowSkydexDataHideIndex(3);
                    break;
                case 4:
                    HideSkydexDataShowIndex("Сплошной покров и не обнаружено\nнижнего края облака");
                    break;
                case 5:
                    HideSkydexDataShowIndex("Обнаружен прозрачный покров");
                    break;
            }
        }

        public void ClearFields()
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = "Н.Д.";

            ShowSkydexDataHideIndex(3, true);
        }

        private void HideSkydexDataShowIndex(string indexText)
        {
            foreach (var block in skydexTextBlocks)
                block.Visibility = Visibility.Collapsed;

            SkydexIndex.Visibility = Visibility.Visible;
            SkydexIndex.Text = indexText;
        }

        private void ShowSkydexDataHideIndex(int count, bool clear = false)
        {
            if(!clear)
            if (count == _lastCount) return;
            _lastCount = count;

            foreach (var block in skydexTextBlocks)
                block.Visibility = Visibility.Visible;

            for (int i = 0; i < 3; i++)
            {
                if (i >= count)
                {
                    ngoTextBlocks[i].Text = "Не опред.";
                    ngoTextBlocks[i].Style = _smallStyle;
                }
                else
                {
                    ngoTextBlocks[i].Style = _basicStyle;
                }
            }

            SkydexIndex.Visibility = Visibility.Collapsed;
        }

    }
}
