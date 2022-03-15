using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using KeqingNiuza.Core.Wish;
using KeqingNiuza.Core.XunkongApi;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using System.Drawing.Imaging;
using Mapster;

namespace KeqingNiuza.View
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        static AboutView()
        {
            TypeAdapterConfig<Core.XunkongApi.CharacterInfo, Core.Wish.CharacterInfo>.NewConfig()
                                                                                     .Map(d => d.Rank, s => s.Rarity)
                                                                                     .Map(d => d.ElementType, s => s.Element)
                                                                                     .Map(d => d.Thumb, s => s.ToThumb)
                                                                                     .Map(d => d.Portrait, s => s.ToPortrait);
            TypeAdapterConfig<Core.XunkongApi.WeaponInfo, Core.Wish.WeaponInfo>.NewConfig()
                                                                               .Map(d => d.Rank, s => s.Rarity)
                                                                               .Map(d => d.Thumb, s => s.ToThumb)
                                                                               .Map(d => d.Portrait, s => s.ToPortrait);
        }


        public AboutView()
        {
            InitializeComponent();
            TextBlock_Version.Text = "版本：" + Service.Const.FileVersion;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

        private async void Button_ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            if (await Dialog.Show(new ExcelImportDialog()).GetResultAsync<bool>())
            {
                Growl.Success("导入数据成功");
                (Application.Current.MainWindow as MainWindow).ViewModel.ChangeViewContent("WishSummaryView");
            }
        }

        private async void Button_InputUrl_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            await mainWindow.ViewModel.UpdateWishData(TextBox_InputUrl.Text);
        }

        private void Button_WishlogBackup_Click(object sender, RoutedEventArgs e)
        {
            var window = new WishlogBackupWindow();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }


        private bool _OverWriteExistedFile;
        public bool OverWriteExistedFile
        {
            get { return _OverWriteExistedFile; }
            set
            {
                _OverWriteExistedFile = value;
                OnPropertyChanged();
            }
        }

        private async void _Button_Download_Click(object sender, RoutedEventArgs e)
        {
            _Button_Download.IsEnabled = false;
            try
            {
                int errorCount = 0;
                _Text_DownloadProcess.Text = "获取数据中";
                var client = new XunkongApiClient();
                var characters = await client.GetCharacterInfosAsync();
                var localCharacters = characters.Adapt<List<Core.Wish.CharacterInfo>>();
                Directory.CreateDirectory("Resource/List");
                File.WriteAllText("Resource/List/CharacterInfoList.json", JsonSerializer.Serialize(localCharacters, Core.CloudBackup.Const.JsonOptions));
                var weapons = await client.GetWeaponInfosAsync();
                var localWeapons = weapons.Adapt<List<Core.Wish.WeaponInfo>>();
                File.WriteAllText("Resource/List/WeaponInfoList.json", JsonSerializer.Serialize(localWeapons, Core.CloudBackup.Const.JsonOptions));
                var events = await client.GetWishEventInfosAsync();
                var localEvents = new List<WishEvent>();
                foreach (var items in events.GroupBy(x => new { x.QueryType, x.StartTime }))
                {
                    var wishEvent = new WishEvent
                    {
                        WishType = items.First().QueryType,
                        Name = string.Join(" ", items.Select(x => x.Name)),
                        Version = items.First().Version,
                        StartTime = items.First().StartTime,
                        EndTime = items.First().EndTime,
                        Rank4UpItems = items.First().Rank4UpItems,
                        Rank5UpItems = items.SelectMany(x => x.Rank5UpItems).ToList(),
                    };
                    localEvents.Add(wishEvent);
                }
                File.WriteAllText("Resource/List/WishEventList.json", JsonSerializer.Serialize(localEvents, Core.CloudBackup.Const.JsonOptions));
                Directory.CreateDirectory("Resource/Character");
                int count = characters.Count();
                int index = 1;
                _Text_DownloadProcess.Text = $"正在下载角色图片 {index} / {count}";
                var taskList = characters.Select(async item =>
                {
                    try
                    {
                        if (OverWriteExistedFile || !File.Exists(item.ToThumb))
                        {
                            var bytes = await client.HttpClient.GetByteArrayAsync(item.FaceIcon);
                            File.WriteAllBytes(item.ToThumb, bytes);
                        }
                        if (OverWriteExistedFile || !File.Exists(item.ToPortrait))
                        {
                            var bytes = await client.HttpClient.GetByteArrayAsync(item.GachaSplash);
                            SaveClippedImage(item.ToPortrait, bytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.OutputLog(LogType.Error, $"Download character image {item.Name}", ex);
                        Interlocked.Increment(ref errorCount);
                    }
                    finally
                    {
                        Interlocked.Increment(ref index);
                        _Text_DownloadProcess.Text = $"正在下载角色图片 {index} / {count}";
                    }
                });
                await Task.WhenAll(taskList);
                Directory.CreateDirectory("Resource/Weapon");
                count = weapons.Count();
                index = 1;
                _Text_DownloadProcess.Text = $"正在下载武器图片 {index} / {count}";
                taskList = weapons.Select(async item =>
                {
                    try
                    {
                        if (OverWriteExistedFile || !File.Exists(item.ToThumb))
                        {
                            var bytes = await client.HttpClient.GetByteArrayAsync(item.AwakenIcon);
                            File.WriteAllBytes(item.ToThumb, bytes);
                        }
                        if (OverWriteExistedFile || !File.Exists(item.ToPortrait))
                        {
                            var bytes = await client.HttpClient.GetByteArrayAsync(item.GachaIcon);
                            File.WriteAllBytes(item.ToPortrait, bytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.OutputLog(LogType.Error, $"Download weapon image {item.Name}", ex);
                        Interlocked.Increment(ref errorCount);
                    }
                    finally
                    {
                        Interlocked.Increment(ref index);
                        _Text_DownloadProcess.Text = $"正在下载武器图片 {index} / {count}";
                    }
                });
                await Task.WhenAll(taskList);
                if (errorCount > 0)
                {
                    _Text_DownloadProcess.Text = $"已完成，有 {errorCount} 张图片下载失败，相关信息保存在日志中";
                }
                else
                {
                    _Text_DownloadProcess.Text = $"已完成";
                }
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
                _Text_DownloadProcess.Text = ex.Message;
            }
            finally
            {
                _Button_Download.IsEnabled = true;
            }
        }



        private void SaveClippedImage(string fileName, byte[] originBytes)
        {
            var ms = new MemoryStream(originBytes);
            var bitmap = new Bitmap(ms);
            var rect = GetUntransparentRect(bitmap);
            var newBitmap = bitmap.Clone(rect, bitmap.PixelFormat);
            newBitmap.Save(fileName, ImageFormat.Png);
            newBitmap.Dispose();
            bitmap.Dispose();
        }


        private unsafe Rectangle GetUntransparentRect(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int left = width, top = height, right = 0, bottom = 0;
            var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var ptr = (byte*)data.Scan0;
            var stride = data.Stride;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if ((*(ptr + 3) & 0xF0) != 0)
                    {
                        if (x < left)
                        {
                            left = x;
                        }
                        if (x > right)
                        {
                            right = x;
                        }
                        if (y < top)
                        {
                            top = y;
                        }
                        if (y > bottom)
                        {
                            bottom = y;
                        }
                    }
                    ptr += 4;
                }
                ptr += stride - width * 4;
            }
            bitmap.UnlockBits(data);
            return new Rectangle(left, top, right - left + 1, bottom - top + 1);
        }


    }
}
