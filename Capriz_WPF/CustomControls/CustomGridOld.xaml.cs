using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capriz_WPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomGridOld.xaml
    /// </summary>
    public partial class CustomGridOld : System.Windows.Controls.UserControl
    {
        DataTable dt = null;

        public DataTable Dt
        {
            set
            {
                dt = value;
            }
        }

        public int GetCountRecords
        {
            get
            {
                return dataGridView1.Rows.Count;
            }
        }

        private bool FilterData(DateTime date, DateTime[] d)
        {
            return (DateTime.Compare(date, d[0]) >= 0)
                && (DateTime.Compare(date, d[1]) <= 0);
        }



        public void Filter(string[] filterDates)
        {
            DateTime[] dates = new DateTime[2] { Convert.ToDateTime(filterDates[0]), Convert.ToDateTime(filterDates[1]) };

            if (dt.Rows.Count > 0)
            {
                var query = from myRow in dt.AsEnumerable()
                            where
                            FilterData(Convert.ToDateTime(string.Concat(myRow.Field<string>("Date") + " " +
                            myRow.Field<string>("Time"))), dates) == true
                            select myRow;
                DataTable dtresult = new DataTable();
                if (query != null && query.AsDataView().Count > 0)
                    dtresult = query.CopyToDataTable();
                else
                    dtresult = dt;
                query = null;
                int count = dtresult.Rows.Count;

                dataGridView1.DataSource = dtresult;
                dataGridView1.Refresh();
            }
        }

        public string GetMinDate
        {
            get
            {
                var dateTimes = dataGridView1.Rows.Cast<DataGridViewRow>().Select(x => Convert.ToDateTime(x.Cells["Date"].Value + " " + x.Cells["Time"].Value));
                return dateTimes.Min().ToString();
            }
        }

        public string GetMaxDate
        {
            get
            {
                var dateTimes = dataGridView1.Rows.Cast<DataGridViewRow>().Select(x => Convert.ToDateTime(x.Cells["Date"].Value + " " + x.Cells["Time"].Value));
                return dateTimes.Max().ToString();
            }
        }

        public CustomGridOld()
        {
            InitializeComponent();

            dataGridView1.MouseWheel += new System.Windows.Forms.MouseEventHandler(DataGridView1_MouseWheel);


            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(4, 44, 86);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(129)))), ((int)(((byte)(16)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(82)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            //this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            //| System.Windows.Forms.AnchorStyles.Left)
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(13, 52, 93);
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(13, 52, 93);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(129)))), ((int)(((byte)(16)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(82)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 80;

            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(57, 92, 132);
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(129)))), ((int)(((byte)(16)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(82)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;

            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(13, 52, 93); 
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 50;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;

            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
        }

        void DataGridView1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int currentIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            int scrollLines = SystemInformation.MouseWheelScrollLines;
            if (e.Delta > 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex
                    = Math.Max(0, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex
                    = currentIndex + scrollLines;
            }
        }



        public void ShowDataGrid(string from, string to)
        {

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                DataTable newdt = Data.DataToTable.CopyDt(dt);
                dataGridView1.DataSource = newdt;

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderText = Data.ConvertData.GetNameColumn(col.Name);
                    col.HeaderText = col.HeaderText.ToUpper();
                    if (col.HeaderText.Contains("ЗА 10"))
                        col.HeaderText = col.HeaderText.Replace("ЗА 10", "\r\nЗА 10");
                }

                dataGridView1.Columns[0].Width = 94;
                dataGridView1.Columns[1].Width = 74;
                dataGridView1.Columns[2].Width = 106;
                dataGridView1.Columns[3].Width = 98;
                dataGridView1.Columns[4].Width = 122;
                dataGridView1.Columns[5].Width = 92;
                dataGridView1.Columns[6].Width = 164;
            }
        }

        public void ShowDataGridFilter()
        {
            if (dt == null) return;


            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderText = Data.ConvertData.GetNameColumn(col.Name);
                    col.HeaderText = col.HeaderText.ToUpper();
                    if (col.HeaderText.Contains("ЗА 10"))
                        col.HeaderText = col.HeaderText.Replace("ЗА 10", "\r\nЗА 10");
                }

                dataGridView1.Columns[0].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
                dataGridView1.Columns[0].Width = 164;
                dataGridView1.Columns[1].Width = 102;
                dataGridView1.Columns[2].Width = 94;
                dataGridView1.Columns[3].Width = 118;
                dataGridView1.Columns[4].Width = 88;
                dataGridView1.Columns[5].Width = 161;

                //dataGridView1.Columns[0].Width = 92;
                //dataGridView1.Columns[1].Width = 72;
                //dataGridView1.Columns[2].Width = 102;
                //dataGridView1.Columns[3].Width = 94;
                //dataGridView1.Columns[4].Width = 118;
                //dataGridView1.Columns[5].Width = 88;
                //dataGridView1.Columns[6].Width = 161;

            }
        }

        public void ShowDataGridFilterOLD(string[] filterDates)
        {
            if (dt == null) return;

            DateTime[] dates = new DateTime[2] { Convert.ToDateTime(filterDates[0]), Convert.ToDateTime(filterDates[1]) };

            if (dt.Rows.Count > 0)
            {
                DataTable newdt = Data.DataToTable.CopyDt(dt);

                dataGridView1.DataSource = newdt;

                var query = from myRow in newdt.AsEnumerable()
                            where
                            FilterData(Convert.ToDateTime(string.Concat(myRow.Field<string>("Date") + " " +
                            myRow.Field<string>("Time"))), dates) == true
                            select myRow;
                DataTable dtresult = new DataTable();
                if (query != null && query.AsDataView().Count > 0)
                    dtresult = query.CopyToDataTable();
                else
                    dtresult = newdt;
                query = null;
                int count = dtresult.Rows.Count;

                dataGridView1.DataSource = dtresult;
                dataGridView1.Refresh();

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderText = Data.ConvertData.GetNameColumn(col.Name);
                    col.HeaderText = col.HeaderText.ToUpper();
                    if (col.HeaderText.Contains("ЗА 10"))
                        col.HeaderText = col.HeaderText.Replace("ЗА 10", "\r\nЗА 10");
                }

                dataGridView1.Columns[0].Width = 92;
                dataGridView1.Columns[1].Width = 72;
                dataGridView1.Columns[2].Width = 102;
                dataGridView1.Columns[3].Width = 94;
                dataGridView1.Columns[4].Width = 118;
                dataGridView1.Columns[5].Width = 88;
                dataGridView1.Columns[6].Width = 161;

            }
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == "YourNumericColumnName") // Замените на имя вашего столбца с числами
            {
                // Преобразуем значения в числа и сравниваем
                int num1 = int.Parse(e.CellValue1.ToString());
                int num2 = int.Parse(e.CellValue2.ToString());

                e.SortResult = num1.CompareTo(num2);
                e.Handled = true; // Указываем, что сортировка была обработана
            }
            else
            {
                // Для других столбцов используем стандартную сортировку
                e.Handled = false;
            }
        }
    }
}
