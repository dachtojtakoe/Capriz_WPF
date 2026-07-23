using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Capriz_WPF.CustomControls
{
    public partial class WinFormsOxyPlot : UserControl
    {
        public OxyPlot.WindowsForms.PlotView PlotView { get; private set; }

        public WinFormsOxyPlot()
        {
            InitializeComponent();

            PlotView = new OxyPlot.WindowsForms.PlotView
            {
                Dock = DockStyle.Fill
            };

            //PlotView.BackColor = Color.LightBlue;

            this.Controls.Add(PlotView);
        }
    }
}
