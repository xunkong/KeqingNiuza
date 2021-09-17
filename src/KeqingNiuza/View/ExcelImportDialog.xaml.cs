using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using HandyControl.Controls;
using HandyControl.Interactivity;
using KeqingNiuza.Service;
using KeqingNiuza.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace KeqingNiuza.View
{
    /// <summary>
    /// ExcelImportView.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelImportDialog : UserControl
    {



        public ExcelImportViewModel ViewModel { get; set; }

        public ExcelImportDialog()
        {
            InitializeComponent();
            ViewModel = new ExcelImportViewModel();
            DataContext = ViewModel;
        }



        private void Button_SelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedUserData == null)
            {
                TextBlock_Info.Text = "请选择 Uid";
                TextBlock_Info.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel worksheets|*.xlsx|All|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ViewModel.ImportExcelFile(openFileDialog.FileName);
                    TextBlock_Info.Foreground = new SolidColorBrush(Colors.Gray);
                    TextBlock_Info.Text = openFileDialog.SafeFileName;
                }
                catch (Exception ex)
                {
                    TextBlock_Info.Text = "无法解析文件";
                    TextBlock_Info.Foreground = new SolidColorBrush(Colors.Red);
                    Growl.Warning(ex.Message);
                    Log.OutputLog(LogType.Error, "Button_SelectFile_Click", ex);
                }
            }
        }

        private void DataGrid_ImportedData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var row = e.Row.GetIndex();
            e.Row.Header = row + 1;
        }

        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExportMergedDataList();
            ControlCommands.Close.Execute(null, this);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ControlCommands.Close.Execute(null, this);
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MatchOriginalData();
        }
    }
}
