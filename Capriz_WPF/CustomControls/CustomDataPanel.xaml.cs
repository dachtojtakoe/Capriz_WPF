using Capriz_WPF.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
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
using static System.Net.Mime.MediaTypeNames;

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
            var elements = new List<UIElement> { lblTemp, DataTemp, lblHum, DataHum};
            elements.ForEach(e => e.Visibility = _conf.DTVV1 == "1" || _conf.DTVV2 == "1" ? Visibility.Visible : Visibility.Collapsed);

            elements = new List<UIElement> { lblPressMm, DataPressMm, lblPressGPa, DataPressGPa, lblBarT, DataBarT, lblTrend, DataTrend };
            elements.ForEach(e => e.Visibility = _conf.DAD == "1" ? Visibility.Visible : Visibility.Collapsed);

            elements = new List<UIElement> { lblDMDV1, DataDMDV1, lblDMDV10, DataDMDV10 };
            elements.ForEach(e => e.Visibility = _conf.DMDV == "1" ? Visibility.Visible : Visibility.Collapsed);

            elements = new List<UIElement> { lblClouds, DataClouds, lblNGO1, DataNgo1, lblNgo2, DataNgo2, lblNgo3, DataNgo3 };
            elements.ForEach(e => e.Visibility = _conf.DVGO == "1" ? Visibility.Visible : Visibility.Collapsed);

            var columns = new List<Grid> { FirstColumn, SecondColumn, ThirdColumn, FourthColumn };

            bool hasVisibleChild = false;
            foreach (var column in columns)
            {
                hasVisibleChild = false;
                foreach (var child in column.Children)
                {
                    if (child is UIElement uiElement && uiElement.Visibility == Visibility.Visible)
                    {
                        hasVisibleChild = true;
                        break;
                    }
                };

                column.Visibility = hasVisibleChild ? Visibility.Visible : Visibility.Collapsed;
            }

            if (_conf.SEELEVEL == "1")
            {
                lblPressGPa.Text = "Атмосферное  \nдавление УМ, гПа";
                lblPressMm.Text = "Атмосферное  \nдавление УМ, мм рт. ст.";
            }
            else
            {
                lblPressGPa.Text = "Атмосферное  \nдавление, гПа";
                lblPressMm.Text = "Атмосферное  \nдавление, мм рт. ст.";
            }
        }
    }
}
