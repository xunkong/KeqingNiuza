using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace KeqingNiuza.Wish
{
    public class ExcelExpoter
    {
        private static readonly Color Star5Color = Color.FromArgb(0xBD, 0x69, 0x32);
        private static readonly Color Star4Color = Color.FromArgb(0xA2, 0x56, 0xE1);
        private static readonly Color FirstRowColor = Color.FromArgb(0x00, 0x70, 0xC0);

        private readonly ExcelPackage ExcelPackage;

        public ExcelExpoter()
        {
            ExcelPackage = new ExcelPackage();
            ExcelPackage.Workbook.Worksheets.Add("角色活动祈愿");
            ExcelPackage.Workbook.Worksheets.Add("武器活动祈愿");
            ExcelPackage.Workbook.Worksheets.Add("常驻祈愿");
            ExcelPackage.Workbook.Worksheets.Add("新手祈愿");
            InitSheetAndFreezeFirstRow(1);
            InitSheetAndFreezeFirstRow(2);
            InitSheetAndFreezeFirstRow(3);
            InitSheetAndFreezeFirstRow(4);

        }

        private void InitSheetAndFreezeFirstRow(int sheetNum)
        {
            var sheet = ExcelPackage.Workbook.Worksheets[sheetNum];
            sheet.View.FreezePanes(2, 1);
            sheet.DefaultRowHeight = 18;
            sheet.Column(1).Width = 25;
            sheet.Column(2).Width = 20;
            sheet.Column(3).Width = 8;
            sheet.Column(4).Width = 8;
            sheet.Column(5).Width = 8;
            sheet.Column(6).Width = 8;
            sheet.Column(7).Width = 27;
            sheet.Cells["A1"].Value = "时间";
            sheet.Cells["B1"].Value = "名称";
            sheet.Cells["C1"].Value = "类别";
            sheet.Cells["D1"].Value = "星级";
            sheet.Cells["E1"].Value = "总次数";
            sheet.Cells["F1"].Value = "保底内";
            sheet.Cells["G1"].Value = "祈愿 Id";
        }


        public void AddWishData(List<WishData> list)
        {
            var groups = list.GroupBy(x => x.WishType);
            foreach (var group in groups)
            {
                var datas = group.OrderByDescending(x => x.Id).ToList();
                AddWishDataToSheet(datas);
            }
        }


        public void SaveAs(string path)
        {
            for (int i = 1; i < 5; i++)
            {
                var cells = ExcelPackage.Workbook.Worksheets[i].Cells;
                var range = cells[1, 1, cells.End.Row, cells.End.Column];
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "微软雅黑";
                range.Style.Numberformat.Format = "@";
                cells["A1:G1"].Style.Font.Color.SetColor(FirstRowColor);

            }
            var fileInfo = new FileInfo(path);
            ExcelPackage.SaveAs(fileInfo);
        }

        private void AddWishDataToSheet(List<WishData> datas)
        {
            if (!datas.Any())
            {
                return;
            }
            int sheetNum = 0, count = datas.Count;
            switch (datas[0].WishType)
            {
                case WishType.Novice:
                    sheetNum = 4;
                    break;
                case WishType.Permanent:
                    sheetNum = 3;
                    break;
                case WishType.CharacterEvent:
                    sheetNum = 1;
                    break;
                case WishType.WeaponEvent:
                    sheetNum = 2;
                    break;
            }
            var cells = ExcelPackage.Workbook.Worksheets[sheetNum].Cells;
            for (int i = 0; i < datas.Count; i++)
            {
#if INTERNAIONAL
                cells[i + 2, 1].Value = datas[i].Time.ToString("yyyy-MM-dd  HH:mm:ss");
#else
                var chinaTime = TimeZoneInfo.ConvertTime(datas[i].Time, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                cells[i + 2, 1].Value = chinaTime.ToString("yyyy-MM-dd  HH:mm:ss");
#endif
                cells[i + 2, 2].Value = datas[i].Name;
                cells[i + 2, 3].Value = datas[i].ItemType;
                cells[i + 2, 4].Value = datas[i].Rank;
                cells[i + 2, 5].Value = count;
                count--;
                cells[i + 2, 6].Value = datas[i].Guarantee;
                cells[i + 2, 7].Value = datas[i].Id.ToString();
                cells[i + 2, 1, i + 2, 7].Style.Font.Color.SetColor(Color.Gray);
                if (datas[i].Rank == 4)
                {
                    cells[i + 2, 1, i + 2, 7].Style.Font.Color.SetColor(Star4Color);
                }
                if (datas[i].Rank == 5)
                {
                    cells[i + 2, 1, i + 2, 7].Style.Font.Color.SetColor(Star5Color);
                }
            }
        }

    }
}
