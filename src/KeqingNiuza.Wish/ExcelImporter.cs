using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OfficeOpenXml;

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
        private ExcelRange characterCells;
        private ExcelRange weaponCells;
        private ExcelRange permanentCells;
        private ExcelRange noviceCells;



        /// <summary>
        /// 导入数据与现有数据是否匹配
        /// </summary>
        public bool IsMatchOriginalData { get; set; }


        private bool _CanExport;
        /// <summary>
        /// 能否导出合并数据
        /// </summary>
        public bool CanExport
        {
            get { return _CanExport; }
            set
            {
                _CanExport = value;
                OnPropertyChanged();
            }
        }



        public List<WishData> OriginalWishDataList { get; set; }

        public List<ImportedWishData> ImportedWishDataList { get; set; }


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

        /// <summary>
        /// 从Excel文件导入数据
        /// </summary>
        /// <param name="path"></param>
        public void ImportFromExcel(string path)
        {
            ExcelPackage = new ExcelPackage(new System.IO.FileInfo(path));
            characterCells = ExcelPackage.Workbook.Worksheets["角色活动祈愿"].Cells;
            weaponCells = ExcelPackage.Workbook.Worksheets["武器活动祈愿"].Cells;
            permanentCells = ExcelPackage.Workbook.Worksheets["常驻祈愿"].Cells;
            noviceCells = ExcelPackage.Workbook.Worksheets["新手祈愿"].Cells;
            SortImportedData();
            ShownWishDataCollection = new ObservableCollection<ImportedWishData>(ImportedWishDataList);
            MatchOriginalData();
        }

        /// <summary>
        /// 当数据发生更改时重新校验
        /// </summary>

        private void OnCellChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Task.Run(() =>
            {
                foreach (var item in ShownWishDataCollection)
                {
                    item.IsError = false;
                    item.Comment = "";
                }
                MatchOriginalData();
                CanExport = true;
            });
        }

        /// <summary>
        /// 以祈愿时间为为顺序给导入数据排序
        /// </summary>
        private void SortImportedData()
        {
            var CharacterList = new List<ImportedWishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new ImportedWishData
                    {
#if INTERNAIONAL
                        Time = DateTime.Parse(characterCells[i, 1].Value as string),
#else
                        Time = DateTime.Parse(characterCells[i, 1].Value as string + " +08:00"),
#endif
                        Name = characterCells[i, 2].Value as string,
                        ItemType = characterCells[i, 3].Value as string,
                        Rank = int.Parse(characterCells[i, 4].Value.ToString()),
                        WishType = WishType.CharacterEvent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    var id = characterCells[i, 7].Value as string;
                    if (id != null)
                    {
                        data.Id = long.Parse(id);
                    }
                    CharacterList.Add(data);
                }
                catch (Exception ex)
                {
                    // 读取到没有数据的行时，会发生错误，使用break跳出循环
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
#if INTERNAIONAL
                        Time = DateTime.Parse(weaponCells[i, 1].Value as string),
#else
                        Time = DateTime.Parse(weaponCells[i, 1].Value as string + " +08:00"),
#endif
                        Name = weaponCells[i, 2].Value as string,
                        ItemType = weaponCells[i, 3].Value as string,
                        Rank = int.Parse(weaponCells[i, 4].Value.ToString()),
                        WishType = WishType.WeaponEvent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    var id = weaponCells[i, 7].Value as string;
                    if (id != null)
                    {
                        data.Id = long.Parse(id);
                    }
                    WeaponList.Add(data);
                }
                catch (Exception ex)
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
#if INTERNAIONAL
                        Time = DateTime.Parse(permanentCells[i, 1].Value as string),
#else
                        Time = DateTime.Parse(permanentCells[i, 1].Value as string + " +08:00"),
#endif
                        Name = permanentCells[i, 2].Value as string,
                        ItemType = permanentCells[i, 3].Value as string,
                        Rank = int.Parse(permanentCells[i, 4].Value.ToString()),
                        WishType = WishType.Permanent,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    var id = permanentCells[i, 7].Value as string;
                    if (id != null)
                    {
                        data.Id = long.Parse(id);
                    }
                    PermanentList.Add(data);
                }
                catch (Exception ex)
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
#if INTERNAIONAL
                        Time = DateTime.Parse(noviceCells[i, 1].Value as string),
#else
                        Time = DateTime.Parse(noviceCells[i, 1].Value as string + " +08:00"),
#endif
                        Name = noviceCells[i, 2].Value as string,
                        ItemType = noviceCells[i, 3].Value as string,
                        Rank = int.Parse(noviceCells[i, 4].Value.ToString()),
                        WishType = WishType.Novice,
                        Count = 1,
                        IsLostId = true,
                        Language = "zh-cn"
                    };
                    var id = noviceCells[i, 7].Value as string;
                    if (id != null)
                    {
                        data.Id = long.Parse(id);
                    }
                    NoviceList.Add(data);
                }
                catch (Exception ex)
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


        // 因为很多未知Bug，取消此条
#if false

        /// <summary>
        /// 检测重复数据，同一时间点只能由1条或10条数据
        /// </summary>
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

#endif


        /// <summary>
        /// 检测导入数据与现有数据匹配
        /// </summary>
        public void MatchOriginalData()
        {
            foreach (var item in ShownWishDataCollection)
            {
                item.IsError = false;
                item.Comment = "";
            }
            var list = ShownWishDataCollection.Except(OriginalWishDataList, new ImportDataComparer());
            var time = OriginalWishDataList.First().Time;
            var errorlist = list.Where(x => x.Time >= time).Select(x => x).ToList();
            if (errorlist.Any())
            {
                foreach (ImportedWishData data in errorlist)
                {
                    data.IsError = true;
                    data.Comment = "与现有数据不匹配";
                }
                IsMatchOriginalData = false;
                CanExport = false;
            }
            else
            {
                IsMatchOriginalData = true;
                CanExport = true;
            }
        }





        /// <summary>
        /// 导出合并数据
        /// </summary>
        /// <returns></returns>
        public List<WishData> ExportMergedDataList()
        {
            var mergedDataList = new List<WishData>();
            var time = OriginalWishDataList.First().Time;
            var foreList = ImportedWishDataList.Where(x => x.Time < time).Select(x => x).ToList();
            long i = 1000000000000000000;
            foreach (var data in foreList)
            {
                if (data.Id == 0)
                {
                    i++;
                    data.Id = i;
                }
            }
            mergedDataList.AddRange(foreList);
            mergedDataList.AddRange(OriginalWishDataList);
            return mergedDataList;
        }
    }

    class ImportDataComparer : IEqualityComparer<WishData>
    {
        public bool Equals(WishData x, WishData y)
        {
            return (x.Time, x.Name) == (y.Time, y.Name);
        }

        public int GetHashCode(WishData obj)
        {
            return (obj.Time, obj.Name).GetHashCode();
        }
    }
}
