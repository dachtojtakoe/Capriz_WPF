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
            textBlocks = new List<TextBlock>() { DataTemp, DataHum, DataPressMm, DataPressGPa, DataBarT, DataTrend, DataClouds, DataDMDV1,
            DataDMDV10, DataNgo1, DataNgo2, DataNgo3};
        }

        public void SetDataToFields(List<string> data)
        {
            for (int i = 0; i < data.Count; i++)
                textBlocks[i].Text = data[i];
        }
    }
}
