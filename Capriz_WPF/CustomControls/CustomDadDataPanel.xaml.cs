using Capriz_WPF.Common;
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
    /// Логика взаимодействия для CustomDadDataPanel.xaml
    /// </summary>
    public partial class CustomDadDataPanel : UserControl
    {
        List<TextBlock> textBlocks;

        public CustomDadDataPanel()
        {
            InitializeComponent();
            textBlocks = new List<TextBlock>() { DataPressMm, DataPressGPa, DataBarT, DataTrend, DataPressMmRes, DataPressGPaRes/*, DataBarTRes, DataTrendRes*/ };
        }

        public void SetDataToFields(List<string> data)
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = data[i];
        }

        public void ClearFields()
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = "Н.Д.";
        }

        public void SetConfig(Configuration _conf)
        {
            HeightLabel.Text = "Давление на высоте -" + _conf.HEIGHT + " м";
        }
    }
}
