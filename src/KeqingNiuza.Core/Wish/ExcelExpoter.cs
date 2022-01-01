using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace KeqingNiuza.Core.Wish
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
            ExcelPackage.Workbook.Worksheets.Add("统计分析");
            ExcelPackage.Workbook.Worksheets.Add("角色活动祈愿");
            ExcelPackage.Workbook.Worksheets.Add("武器活动祈愿");
            ExcelPackage.Workbook.Worksheets.Add("常驻祈愿");
            ExcelPackage.Workbook.Worksheets.Add("新手祈愿");
            ExcelPackage.Workbook.Worksheets.Add("原始数据");
            InitSheetAndFreezeFirstRow(2);
            InitSheetAndFreezeFirstRow(3);
            InitSheetAndFreezeFirstRow(4);
            InitSheetAndFreezeFirstRow(5);

        }

        private void InitSheetAndFreezeFirstRow(int sheetNum)
        {
            var sheet = ExcelPackage.Workbook.Worksheets[sheetNum];
            sheet.View.FreezePanes(2, 1);
            sheet.DefaultRowHeight = 18;
            sheet.Column(1).Width = 25;
            sheet.Column(2).Width = 20;
            sheet.Column(3).Width = 12;
            sheet.Column(4).Width = 8;
            sheet.Column(5).Width = 16;
            sheet.Column(6).Width = 8;
            sheet.Column(7).Width = 8;
            sheet.Column(8).Width = 28;
            sheet.Cells["A1"].Value = "时间";
            sheet.Cells["B1"].Value = "名称";
            sheet.Cells["C1"].Value = "物品类型";
            sheet.Cells["D1"].Value = "星级";
            sheet.Cells["E1"].Value = "祈愿类型";
            sheet.Cells["F1"].Value = "总次数";
            sheet.Cells["G1"].Value = "保底内";
            sheet.Cells["H1"].Value = "祈愿 Id";
        }


        public void AddWishData(List<WishData> list)
        {
            var list301 = list.Where(x => x.WishType == WishType.CharacterEvent || x.WishType == WishType.CharacterEvent_2).OrderBy(x => x.Id).ToList();
            AddWishDataToSheet(list301);
            var list302 = list.Where(x => x.WishType == WishType.WeaponEvent).OrderBy(x => x.Id).ToList();
            AddWishDataToSheet(list302);
            var list200 = list.Where(x => x.WishType == WishType.Permanent).OrderBy(x => x.Id).ToList();
            AddWishDataToSheet(list200);
            var list100 = list.Where(x => x.WishType == WishType.Novice).OrderBy(x => x.Id).ToList();
            AddWishDataToSheet(list100);
            var allList = list.OrderBy(x => x.Id).ToList();
            AddAllDataToOriginalDataSheet(allList);
        }


        public void SaveAs(string path)
        {
            var cells1 = ExcelPackage.Workbook.Worksheets[1].Cells;
            cells1[1, 1].Value = "这一页还没写完...";
            cells1[1, 1].Style.Font.Name = "微软雅黑";
            cells1[1, 1].Style.Font.Color.SetColor(Color.Red);
            for (int i = 2; i <= 6; i++)
            {
                var cells = ExcelPackage.Workbook.Worksheets[i].Cells;
                var range = cells[1, 1, cells.End.Row, cells.End.Column];
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "微软雅黑";
                range.Style.Numberformat.Format = "@";
                cells["A1:K1"].Style.Font.Color.SetColor(FirstRowColor);

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
                    sheetNum = 5;
                    break;
                case WishType.Permanent:
                    sheetNum = 4;
                    break;
                case WishType.CharacterEvent:
                    sheetNum = 2;
                    break;
                case WishType.WeaponEvent:
                    sheetNum = 3;
                    break;
                case WishType.CharacterEvent_2:
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
                cells[i + 2, 5].Value = datas[i].WishType.GetDescription();
                cells[i + 2, 6].Value = i + 1;
                cells[i + 2, 7].Value = datas[i].Guarantee;
                cells[i + 2, 8].Value = datas[i].Id.ToString();
                cells[i + 2, 1, i + 2, 8].Style.Font.Color.SetColor(Color.Gray);
                if (datas[i].Rank == 4)
                {
                    cells[i + 2, 1, i + 2, 8].Style.Font.Color.SetColor(Star4Color);
                }
                if (datas[i].Rank == 5)
                {
                    cells[i + 2, 1, i + 2, 8].Style.Font.Color.SetColor(Star5Color);
                }
            }
        }


        private void AddAllDataToOriginalDataSheet(List<WishData> datas)
        {
            var sheet = ExcelPackage.Workbook.Worksheets[6];
            sheet.View.FreezePanes(2, 1);
            sheet.DefaultRowHeight = 18;
            sheet.Column(1).Width = 8;
            sheet.Column(2).Width = 16;
            sheet.Column(3).Width = 28;
            sheet.Column(4).Width = 12;
            sheet.Column(5).Width = 12;
            sheet.Column(6).Width = 8;
            sheet.Column(7).Width = 20;
            sheet.Column(8).Width = 12;
            sheet.Column(9).Width = 25;
            sheet.Column(10).Width = 16;
            sheet.Column(11).Width = 18;
            sheet.Cells["A1"].Value = "count";
            sheet.Cells["B1"].Value = "gacha_type";
            sheet.Cells["C1"].Value = "id";
            sheet.Cells["D1"].Value = "item_id";
            sheet.Cells["E1"].Value = "item_type";
            sheet.Cells["F1"].Value = "lang";
            sheet.Cells["G1"].Value = "name";
            sheet.Cells["H1"].Value = "rank_type";
            sheet.Cells["I1"].Value = "time";
            sheet.Cells["J1"].Value = "uid";
            sheet.Cells["K1"].Value = "uigf_gacha_type";
            if (!datas.Any())
            {
                return;
            }
            var cells = sheet.Cells;
            for (int i = 0; i < datas.Count; i++)
            {
                cells[i + 2, 1].Value = "1";
                cells[i + 2, 2].Value = ((int)datas[i].WishType).ToString();
                cells[i + 2, 3].Value = datas[i].Id.ToString();
                cells[i + 2, 4].Value = string.Empty;
                cells[i + 2, 5].Value = datas[i].ItemType;
                cells[i + 2, 6].Value = datas[i].Language;
                cells[i + 2, 7].Value = datas[i].Name;
                //ToString() fix uigf2 incompat issue.
                cells[i + 2, 8].Value = datas[i].Rank.ToString();
                cells[i + 2, 9].Value = datas[i].Time.ToString("yyyy-MM-dd HH:mm:ss");
                cells[i + 2, 10].Value = datas[i].Uid.ToString();
#pragma warning disable CS0618 // 类型或成员已过时
                cells[i + 2, 11].Value = datas[i].QueryType;
#pragma warning restore CS0618 // 类型或成员已过时
                cells[i + 2, 1, i + 2, 11].Style.Font.Color.SetColor(Color.Gray);
                if (datas[i].Rank == 4)
                {
                    cells[i + 2, 1, i + 2, 11].Style.Font.Color.SetColor(Star4Color);
                }
                if (datas[i].Rank == 5)
                {
                    cells[i + 2, 1, i + 2, 11].Style.Font.Color.SetColor(Star5Color);
                }
            }
        }

    }
}
