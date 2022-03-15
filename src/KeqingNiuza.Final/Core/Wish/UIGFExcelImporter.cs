using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.Wish
{
    public class UIGFExcelImporter
    {
        public static List<WishData> Import(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            var cells = package.Workbook.Worksheets["原始数据"].Cells;
            var list = new List<WishData>(cells.Rows - 1);
            for (int i = 2; ; i++)
            {
                if (cells[i, 1].Value is null)
                {
                    break;
                }
                var data = new WishData
                {
                    Count = 1,
                    WishType = (WishType)int.Parse(cells[i, 2].Value.ToString()),
                    Id = long.Parse(cells[i, 3].Value.ToString()),
                    ItemId = 0,
                    ItemType = cells[i, 5].Value.ToString(),
                    Language = cells[i, 6].Value.ToString(),
                    Name = cells[i, 7].Value.ToString(),
                    Rank = int.Parse(cells[i, 8].Value.ToString()),
                    Time = DateTime.Parse(cells[i, 9].Value.ToString()),
                    Uid = int.Parse(cells[i, 10].Value.ToString()),
                };
                list.Add(data);
            }
            return list;
        }
    }
}
