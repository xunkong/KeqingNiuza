using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Wish
{
    public class ExcelImporter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ExcelPackage ExcelPackage;
        private ExcelRange chracterCells;
        private ExcelRange weaponCells;
        private ExcelRange permanentCells;
        private ExcelRange noviceCells;


        public bool HasIntersectData { get; set; }

        public bool IsMatchOriginalData { get; set; }

        public DateTime MatchErrorTime { get; set; }


        private bool _CanExport;
        public bool CanExport
        {
            get { return _CanExport && (IsMatchOriginalData || !HasIntersectData); }
            set
            {
                _CanExport = value;
                OnPropertyChanged();
            }
        }



        public List<WishData> OriginalWishDataList { get; set; }

        public List<ImportedWishData> ImportedWishDataList { get; set; }

        public List<DateTime> DuplicateDateTimeList { get; set; }


        private ObservableCollection<ImportedWishData> _ShownWishDataCollection;
        public ObservableCollection<ImportedWishData> ShownWishDataCollection
        {
            get { return _ShownWishDataCollection; }
            set
            {
                _ShownWishDataCollection = value;
                OnPropertyChanged();
            }
        }


        public ExcelImporter(List<WishData> originalWishDataList)
        {
            OriginalWishDataList = originalWishDataList.OrderBy(x => x.Id).ToList();
        }

        public ExcelImporter() { }


        public void ImportFromExcel(string path)
        {
            ExcelPackage = new ExcelPackage(new System.IO.FileInfo(path));
            chracterCells = ExcelPackage.Workbook.Worksheets["角色活动祈愿"].Cells;
            weaponCells = ExcelPackage.Workbook.Worksheets["武器活动祈愿"].Cells;
            permanentCells = ExcelPackage.Workbook.Worksheets["常驻祈愿"].Cells;
            noviceCells = ExcelPackage.Workbook.Worksheets["新手祈愿"].Cells;
            SortImportedData();
            ShownWishDataCollection = new ObservableCollection<ImportedWishData>(ImportedWishDataList);
            MatchOriginalData();
            DectectDuplicate();
            ShownWishDataCollection.CollectionChanged += ShownWishDataCollection_CollectionChanged;
            if (!ShownWishDataCollection.Any(x => x.IsError))
            {
                CanExport = true;
            }
        }

        private void ShownWishDataCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Task.Run(() =>
            {
                CanExport = true;
                foreach (var item in ShownWishDataCollection)
                {
                    item.IsError = false;
                    item.Comment = "";
                }
                MatchOriginalData();
                DectectDuplicate();
            });
        }

        private void SortImportedData()
        {
            var CharacterList = new List<ImportedWishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new ImportedWishData
                    {
                        Time = DateTime.Parse(chracterCells[i, 1].Value as string),
                        Name = chracterCells[i, 2].Value as string,
                        ItemType = chracterCells[i, 3].Value as string,
                        RankType = int.Parse(chracterCells[i, 4].Value.ToString()),
                        WishType = WishType.CharacterEvent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    CharacterList.Add(data);
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (CharacterList.Any())
            {
                if (CharacterList[0].Time > CharacterList.Last().Time)
                {
                    CharacterList.Reverse();
                }
            }

            var WeaponList = new List<ImportedWishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new ImportedWishData
                    {
                        Time = DateTime.Parse(weaponCells[i, 1].Value as string),
                        Name = weaponCells[i, 2].Value as string,
                        ItemType = weaponCells[i, 3].Value as string,
                        RankType = int.Parse(weaponCells[i, 4].Value.ToString()),
                        WishType = WishType.WeaponEvent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    WeaponList.Add(data);
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (WeaponList.Any())
            {
                if (WeaponList[0].Time > WeaponList.Last().Time)
                {
                    WeaponList.Reverse();
                }
            }

            var PermanentList = new List<ImportedWishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new ImportedWishData
                    {
                        Time = DateTime.Parse(permanentCells[i, 1].Value as string),
                        Name = permanentCells[i, 2].Value as string,
                        ItemType = permanentCells[i, 3].Value as string,
                        RankType = int.Parse(permanentCells[i, 4].Value.ToString()),
                        WishType = WishType.Permanent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    PermanentList.Add(data);
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (PermanentList.Any())
            {
                if (PermanentList[0].Time > PermanentList.Last().Time)
                {
                    PermanentList.Reverse();
                }
            }

            var NoviceList = new List<ImportedWishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new ImportedWishData
                    {
                        Time = DateTime.Parse(noviceCells[i, 1].Value as string),
                        Name = noviceCells[i, 2].Value as string,
                        ItemType = noviceCells[i, 3].Value as string,
                        RankType = int.Parse(noviceCells[i, 4].Value.ToString()),
                        WishType = WishType.Novice,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    NoviceList.Add(data);
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (NoviceList.Any())
            {
                if (NoviceList[0].Time > NoviceList.Last().Time)
                {
                    NoviceList.Reverse();
                }
            }


            var characterQueue = new Queue<ImportedWishData>(CharacterList);
            var weaponQueue = new Queue<ImportedWishData>(WeaponList);
            var permanentQueue = new Queue<ImportedWishData>(PermanentList);
            var noviceQueue = new Queue<ImportedWishData>(NoviceList);

            ImportedWishDataList = new List<ImportedWishData>();
            do
            {
                WishData a = null, b = null, c = null, d = null, result;
                if (characterQueue.Any())
                {
                    a = characterQueue.Peek();
                }
                if (weaponQueue.Any())
                {
                    b = weaponQueue.Peek();
                }
                if (permanentQueue.Any())
                {
                    c = permanentQueue.Peek();
                }
                if (noviceQueue.Any())
                {
                    d = noviceQueue.Peek();
                }
                result = a;
                if ((b?.Time ?? DateTime.Now) < (result?.Time ?? DateTime.Now))
                {
                    result = b;
                }
                if ((c?.Time ?? DateTime.Now) < (result?.Time ?? DateTime.Now))
                {
                    result = c;
                }
                if ((d?.Time ?? DateTime.Now) < (result?.Time ?? DateTime.Now))
                {
                    result = d;
                }
                switch (result.WishType)
                {
                    case WishType.Novice:
                        ImportedWishDataList.Add(noviceQueue.Dequeue());
                        break;
                    case WishType.Permanent:
                        ImportedWishDataList.Add(permanentQueue.Dequeue());
                        break;
                    case WishType.CharacterEvent:
                        ImportedWishDataList.Add(characterQueue.Dequeue());
                        break;
                    case WishType.WeaponEvent:
                        ImportedWishDataList.Add(weaponQueue.Dequeue());
                        break;
                }
            } while (noviceQueue.Any() || permanentQueue.Any() || characterQueue.Any() || weaponQueue.Any());


        }



        // 检测重复数据

        public void DectectDuplicate()
        {
            var groups = ShownWishDataCollection.GroupBy(x => x.Time);
            foreach (var group in groups)
            {
                if (group.Count() != 1 && group.Count() != 10)
                {
                    foreach (var item in group)
                    {
                        item.IsError = true;
                        item.Comment = "该时间点数据存在重复或缺失";
                    }
                }
            }
        }




        // 与现有数据匹配

        public void MatchOriginalData()
        {
            var time = OriginalWishDataList.First().Time;
            var matches = ShownWishDataCollection.Where(x => x.Time >= time).Select(x => x).ToList();
            if (!matches.Any())
            {
                HasIntersectData = false;
                return;
            }
            else
            {
                HasIntersectData = true;
            }
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Time != OriginalWishDataList[i].Time || matches[i].Name != OriginalWishDataList[i].Name)
                {
                    IsMatchOriginalData = false;
                    MatchErrorTime = matches[i].Time;
                    matches[i].IsError = true;
                    matches[i].Comment = "与原始数据不匹配";
                    return;
                }
            }
            IsMatchOriginalData = true;
        }


        public List<WishData> ExportMergedDataList()
        {
            var mergedDataList = new List<WishData>();
            var time = OriginalWishDataList.First().Time;
            var foreList = ImportedWishDataList.Where(x => x.Time < time).Select(x => x).ToList();
            for (int i = 0; i < foreList.Count; i++)
            {
                foreList[i].Id = 1000000000000000001 + i;
            }
            mergedDataList.AddRange(foreList);
            mergedDataList.AddRange(OriginalWishDataList);
            return mergedDataList;
        }
    }
}
