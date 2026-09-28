using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для CustomWindDataSmall.xaml
    /// </summary>
    public partial class CustomWindDataSmall : UserControl
    {
        List<TextBlock> textBlocks;

        public CustomWindDataSmall()
        {
            InitializeComponent();
            textBlocks = new List<TextBlock>() { DataMinSpeed, DataMidSpeed, DataMaxSpeed, DataMinDir, DataMidDir, DataMaxDir};
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
    }
}
