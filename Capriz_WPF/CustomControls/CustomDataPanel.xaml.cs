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

        public CustomDataPanel()
        {
            InitializeComponent();
            textBlocks = new List<TextBlock>() { DataTemp, DataHum, /*DataPressMm, DataPressGPa, DataBarT, DataTrend,*/ DataClouds, DataDMDV1,
            DataDMDV10, DataNgo1, DataNgo2, DataNgo3};

        }

        public void SetDataToFields(List<string> data, int skydexIndex)
        {
            for (int i = 0; i < textBlocks.Count; i++)
            {
                textBlocks[i].Text = data[i];
            }

            switch (skydexIndex)
            {
                case 0:
                    HideSkydexDataShowIndex("Облака не обнаружены");
                    break;
                case 1:
                    ShowSkydexDataHideIndex();
                    break;
                case 2:
                    ShowSkydexDataHideIndex();
                    break;
                case 3:
                    ShowSkydexDataHideIndex();
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
            ShowSkydexDataHideIndex();
        }

        private void HideSkydexDataShowIndex(string indexText)
        {
            ThirdColumn.Visibility = Visibility.Hidden;
            FourthColumn.Visibility = Visibility.Hidden;

            SkydexIndex.Visibility = Visibility.Visible;
            SkydexIndex.Text = indexText;
        }

        private void ShowSkydexDataHideIndex()
        {
            ThirdColumn.Visibility = Visibility.Visible;
            FourthColumn.Visibility = Visibility.Visible;

            SkydexIndex.Visibility = Visibility.Hidden;
        }
    }
}
