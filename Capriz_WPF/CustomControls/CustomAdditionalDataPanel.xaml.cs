using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomAdditionalDataPanel : UserControl
    {
        List<TextBlock> textBlocks;

        public CustomAdditionalDataPanel()
        {
            InitializeComponent();
            textBlocks = new List<TextBlock>()
            {
                DataPrecip, DataLat, DataLon, DataHm0, DataHmax
            };
        }

        public void SetDataToFields(List<string> data)
        {
            if (data == null) return;

            int n = Math.Min(textBlocks.Count, data.Count);
            for (int i = 0; i < n; i++)
                textBlocks[i].Text = string.IsNullOrEmpty(data[i]) ? "Н.Д." : data[i];

            for (int i = n; i < textBlocks.Count; i++)
                textBlocks[i].Text = "Н.Д.";
        }

        public void ClearFields()
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = "Н.Д.";
        }
    }
}