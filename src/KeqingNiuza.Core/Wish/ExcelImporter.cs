using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OfficeOpenXml;

namespace KeqingNiuza.Core.Wish
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


        public List<WishData> ImportedWishDataList { get; set; }



        public ExcelImporter() { }

        /// <summary>
        /// 从Excel文件导入数据
        /// </summary>
        /// <param name="path"></param>
        public static List<WishData> ImportFromExcel(string path)
        {
            var importer = new ExcelImporter();
            var package = new ExcelPackage(new System.IO.FileInfo(path));
            importer.characterCells = package.Workbook.Worksheets["角色活动祈愿"].Cells;
            importer.weaponCells = package.Workbook.Worksheets["武器活动祈愿"].Cells;
            importer.permanentCells = package.Workbook.Worksheets["常驻祈愿"].Cells;
            importer.noviceCells = package.Workbook.Worksheets["新手祈愿"].Cells;
            importer.SortImportedData();
            return importer.ImportedWishDataList;
        }



        /// <summary>
        /// 以祈愿时间为为顺序给导入数据排序
        /// </summary>
        private void SortImportedData()
        {
            var CharacterList = new List<WishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new WishData
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

            var WeaponList = new List<WishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new WishData
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

            var PermanentList = new List<WishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new WishData
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

            var NoviceList = new List<WishData>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var data = new WishData
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


            var characterQueue = new Queue<WishData>(CharacterList);
            var weaponQueue = new Queue<WishData>(WeaponList);
            var permanentQueue = new Queue<WishData>(PermanentList);
            var noviceQueue = new Queue<WishData>(NoviceList);

            ImportedWishDataList = new List<WishData>();
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

    }
}
