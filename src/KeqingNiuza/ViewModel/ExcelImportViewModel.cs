using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using HandyControl.Tools.Extension;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.Wish;

namespace KeqingNiuza.ViewModel
{
    public class ExcelImportViewModel : IDialogResultable<(bool, UserData, List<WishData>)>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public (bool, UserData, List<WishData>) Result { get; set; }
        public Action CloseAction { get; set; }


        public ExcelImportViewModel() { }

        public UserData SelectedUserData { get; set; }

        private UserData ImportUserData;

        private ExcelImporter _ExcelImporter;
        public ExcelImporter ExcelImporter
        {
            get { return _ExcelImporter; }
            set
            {
                _ExcelImporter = value;
                OnPropertyChanged();
            }
        }

        private ICollectionView _CollectionView;
        public ICollectionView CollectionView
        {
            get { return _CollectionView; }
            set
            {
                _CollectionView = value;
                OnPropertyChanged();
            }
        }


        private bool _ShowOnlyError;
        public bool ShowOnlyError
        {
            get { return _ShowOnlyError; }
            set
            {
                _ShowOnlyError = value;
                OnPropertyChanged();
                if (value)
                {
                    CollectionView.Filter = x => (x as ImportedWishData).IsError;
                }
                else
                {
                    CollectionView.Filter = null;
                }
            }
        }



        public void ImportExcelFile(string path)
        {
            var list = LocalWishLogLoader.Load(SelectedUserData.WishLogFile);
            ExcelImporter = new ExcelImporter(list);
            ExcelImporter.ImportFromExcel(path);
            CollectionView = CollectionViewSource.GetDefaultView(ExcelImporter.ShownWishDataCollection);
            ImportUserData = SelectedUserData;
        }


        public void ExportMergedDataList()
        {
            try
            {
                var list = ExcelImporter.ExportMergedDataList();
                list.ForEach(x => x.Uid = ImportUserData.Uid);
                Result = (true, ImportUserData, list);
            }
            catch (Exception ex)
            {
                Result = (false, ImportUserData, null);
                Log.OutputLog(LogType.Error, "ExportMergedDataList", ex);
            }
        }


        public void MatchOriginalData()
        {
            ExcelImporter?.MatchOriginalData();
        }

    }
}
