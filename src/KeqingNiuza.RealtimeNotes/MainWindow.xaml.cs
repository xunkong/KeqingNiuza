using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DGP.Genshin.MiHoYoAPI.Record;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using DGP.Genshin.MiHoYoAPI.User;
using DGP.Genshin.MiHoYoAPI.UserInfo;
using KeqingNiuza.RealtimeNotes.Models;
using KeqingNiuza.RealtimeNotes.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.StartScreen;
using static KeqingNiuza.RealtimeNotes.SparsePackageUtil;

namespace KeqingNiuza.RealtimeNotes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsRunningWithIdentity(out packageName))
            {
                isRunningWithIdentity = true;
                TextBlock_SparsePackageState.Text = "已注册程序包";
                TextBlock_SparsePackageState.Foreground = Brushes.Gray;
            }
            else
            {
                TextBlock_SparsePackageState.Text = "未注册程序包";
                TextBlock_SparsePackageState.Foreground = Brushes.Red;
            }
            try
            {
                var bytes = File.ReadAllBytes(Const.CookiesFile);
                cookie = Endecryption.Decrypt(bytes);
            }
            catch (Exception ex)
            {
                TextBlock_State.Text = $"无法读取已保存的Cookie：{ex.Message}";
            }
            await UpdateNoteAsync();
        }



        private bool isRunningWithIdentity;


        private string packageName;

        private string cookie;



        private List<RealtimeNotesInfo> _NoteList;
        public List<RealtimeNotesInfo> NoteList
        {
            get { return _NoteList; }
            set
            {
                _NoteList = value;
                OnPropertyChanged();
            }
        }


        private async Task UpdateNoteAsync()
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                TextBlock_State.Text = "没有Cookie";
            }
            else
            {
                Button_Refresh.IsEnabled = false;
                TextBlock_State.Text = "刷新中";
                NoteList = null;
                try
                {
                    var list = await RealtimeNotesService.GetRealtimeNotes(cookie);
                    if (isRunningWithIdentity)
                    {
                        var ids = await TileService.FindAllAsync();
                        foreach (var note in list)
                        {
                            if (ids.Any(x => x == note.Uid))
                            {
                                note.IsPinned = true;
                                TileService.UpdateTile(note);
                            }
                        }
                    }
                    NoteList = list;
                    TextBlock_State.Text = "";
                }
                catch (Exception ex)
                {
                    TextBlock_State.Text = ex.Message;
                }
                finally
                {
                    Button_Refresh.IsEnabled = true;
                }
            }
        }




        private void Button_RegisterPackage_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegisterSparsePackageWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private async void Button_SetCookie_Click(object sender, RoutedEventArgs e)
        {
            var window = new SetCookieWindow();
            window.Owner = this;
            if (window.ShowDialog() ?? false)
            {
                try
                {
                    var bytes = File.ReadAllBytes(Const.CookiesFile);
                    cookie = Endecryption.Decrypt(bytes);
                }
                catch (Exception ex)
                {
                    TextBlock_State.Text = $"无法读取已保存的Cookie：{ex.Message}";
                }
                await UpdateNoteAsync();
            }
        }



        private async void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            await UpdateNoteAsync();
        }

        private async void Button_PinToStart_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunningWithIdentity)
            {
                TextBlock_State.Text = "没有注册程序包";
                return;
            }
            try
            {
                var info = (sender as Button).Tag as RealtimeNotesInfo;
                var isPinned = await TileService.RequestPinTileAsync(info);
                if (isPinned)
                {
                    TileService.UpdateTile(info);
                }
                TextBlock_State.Text = "添加成功";
            }
            catch (Exception ex)
            {
                TextBlock_State.Text = ex.Message;
            }

        }

        private void Button_AddTask_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddTaskWindow();
            window.Owner = this;
            window.ShowDialog();
            window.Activate();
        }
    }
}
