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
    /// Логика взаимодействия для CustomWindDataNew.xaml
    /// </summary>
    public partial class CustomWindDataNew : UserControl
    {
        readonly List<TextBlock> textBlocks;

        public CustomWindDataNew()
        {
            InitializeComponent();

            textBlocks = new List<TextBlock>() { Data2Min1, Data10Min1, Data2Mid1, Data10Mid1, Data2Max1, Data10Max1, Data2Min2, Data10Min2, Data2Mid2, Data10Mid2, Data2Max2, Data10Max2 };
        }

        public void SetDataToFields(List<string> data)
        {
            for (int i = 0; i < textBlocks.Count; i++)
                textBlocks[i].Text = data[i];
        }
    }
}
