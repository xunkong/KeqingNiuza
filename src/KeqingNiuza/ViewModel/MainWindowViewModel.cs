using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KeqingNiuza.Model;
using KeqingNiuza.View;
using KeqingNiuza.Wish;
using HandyControl.Controls;
using Microsoft.Win32;
using static KeqingNiuza.Common.Const;
using KeqingNiuza.CloudBackup;
using HandyControl.Data;
using GenshinHelper.Desktop.View;
using HandyControl.Tools.Extension;
using System.Diagnostics;
using KeqingNiuza.Common;

namespace KeqingNiuza.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            Directory.CreateDirectory(".\\UserData");
            if (File.Exists("UserData\\Config.json"))
            {
                LoadConfig();
                ViewContent = new GachaAnalysisView(SelectedUserData);
            }
            else
            {
                UserDataList = new List<UserData>();
                ViewContent = new WelcomeView();
            }
            if (File.Exists("UserData\\Account"))
            {
                CloudClient = CloudClient.GetClientFromEncryption();
            }
            if (File.Exists("Resource\\ShowUpdateLog"))
            {
                ViewContent = new UpdateLogView();
                File.Delete("Resource\\ShowUpdateLog");
            }
        }


        #region ControlProperty

        private object _ViewContent;
        public object ViewContent
        {
            get { return _ViewContent; }
            set
            {
                _ViewContent = value;
                OnPropertyChanged();
            }
        }



        #endregion



        #region ItemSource

        private List<UserData> _UserDataList;
        public List<UserData> UserDataList
        {
            get { return _UserDataList; }
            set
            {
                _UserDataList = value;
                OnPropertyChanged();
            }
        }


        private UserData _SelectedUserData;
        public UserData SelectedUserData
        {
            get { return _SelectedUserData; }
            set
            {
                _SelectedUserData = value;
                OnPropertyChanged();
            }
        }

        private CloudClient _CloudClient;
        public CloudClient CloudClient
        {
            get { return _CloudClient; }
            set
            {
                _CloudClient = value;
                OnPropertyChanged();
            }
        }


        #endregion


        public async Task<bool> TestUpdate()
        {
            var updater = new Updater();
            if (updater.IsDeleteUpdateDirectory())
            {
                Directory.Delete(".\\Update", true);
            }
            try
            {
                var updateInfo = await updater.GetUpdateInfo();
                if (updateInfo == (true, true))
                {
                    await updater.PrepareUpdatedFiles();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public void CallUpdate(object sender, EventArgs e)
        {
            Process.Start("Assembly\\Update.exe", "KeqingNiuza.Update");
        }




        private void LoadConfig()
        {
            var setting = JsonSerializer.Deserialize<Config>(File.ReadAllText("UserData\\Config.json"), JsonOptions);
            UserDataList = setting.UserDataList ?? new List<UserData>();
            SelectedUserData = UserDataList.Find(x => x.Uid == setting.LatestUid);
        }


        public void SaveConfig()
        {
            if (SelectedUserData == null)
            {
                return;
            }
            var setting = new Config()
            {
                LatestUid = SelectedUserData.Uid,
                UserDataList = UserDataList,
            };
            File.WriteAllText("UserData\\Config.json", JsonSerializer.Serialize(setting, JsonOptions));
        }


        /// <summary>
        /// 重新加载内容页面
        /// </summary>
        private void ReloadViewContent()
        {
            if (ViewContent is WishSummaryView)
            {
                ViewContent = new WishSummaryView(SelectedUserData);
            }
            if (ViewContent is GachaAnalysisView || ViewContent is WelcomeView)
            {
                ViewContent = new GachaAnalysisView(SelectedUserData);
            }
        }


        /// <summary>
        /// 加载祈愿数据
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataFromGenshinLogFile()
        {
            try
            {
                Growl.Info("正在加载");
                var url = GenshinLogLoader.FindUrlFromLogFile();
                await UpdateDataFromUrl(url);
                ReloadViewContent();
                Growl.Success("加载成功");
            }
            catch (Exception e)
            {
                Growl.Error(e.Message);
            }

        }

        /// <summary>
        /// 更新祈愿数据
        /// </summary>
        /// <returns></returns>
        public async Task UpdateDataFromUrl()
        {
            if (SelectedUserData == null)
            {
                Growl.Info("请先加载数据");
                return;
            }
            try
            {
                Growl.Info("正在更新");
                await UpdateDataFromUrl(SelectedUserData.Url);
                ReloadViewContent();
                Growl.Success("更新成功");
            }
            catch (Exception e)
            {
                Growl.Error(e.Message);
            }
        }


        private async Task UpdateDataFromUrl(string url)
        {
            var exporter = new WishLogExporter(url);
            var newList = await exporter.GetAllLog();
            var uid = 0;
            if (newList.Any())
            {
                uid = newList[0].Uid;
            }
            else
            {
                throw new Exception("没有祈愿记录");
            }
            var index = UserDataList.FindIndex(x => x.Uid == uid);
            UserData userData;
            if (index == -1)
            {
                userData = new UserData()
                {
                    Uid = uid,
                    Url = url,
                    LastUpdateTime = DateTime.Now
                };
                UserDataList.Add(userData);
            }
            else
            {
                userData = UserDataList[index];
                userData.Url = url;
                userData.LastUpdateTime = DateTime.Now;
            }
            var oldList = new List<WishData>();
            if (File.Exists(userData.WishLogFile))
            {
                oldList = JsonSerializer.Deserialize<List<WishData>>(File.ReadAllText(userData.WishLogFile), JsonOptions);
            }
            var list = newList.Union(oldList).ToList();
            File.WriteAllText(userData.WishLogFile, JsonSerializer.Serialize(list, JsonOptions));
            SelectedUserData = userData;
            SaveConfig();
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void ExportExcelFile()
        {
            if (SelectedUserData == null)
            {
                Growl.Info("请选择Uid");
                return;
            }
            if (!File.Exists(SelectedUserData.WishLogFile))
            {
                Growl.Warning("祈愿记录文件不存在");
                return;
            }
            var json = File.ReadAllText(SelectedUserData.WishLogFile);
            var data = JsonSerializer.Deserialize<List<WishData>>(json, JsonOptions);
            var analyzer = new WishAnalyzer(data);
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = $"原神祈愿记录_{SelectedUserData.Uid}.xlsx",
                Filter = "Excel worksheets|*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    analyzer.ExportExcelFile(saveFileDialog.FileName);
                    Growl.Success("导出成功");
                }
                catch (Exception e)
                {
                    Growl.Error(e.Message);
                }
            }
        }


        /// <summary>
        /// 备份用户文件
        /// </summary>
        /// <returns></returns>
        public async Task CloudBackupFileArchive()
        {
            try
            {
                Growl.Info("正在备份");
                await CloudClient.BackupFileArchive();
                Growl.Success("备份成功");
            }
            catch (Exception e)
            {
                Growl.Error(e.Message);
            }
        }

        /// <summary>
        /// 侧边菜单栏点击
        /// </summary>
        /// <param name="header">按钮名称(x:Name)</param>
        public void SideMenuChangeContent(string header)
        {
            if (header == "SideMenu_WishSummary" && !(ViewContent is GachaAnalysisView))
            {
                if (SelectedUserData == null)
                {
                    ViewContent = new NoUidView();
                }
                else
                {
                    ViewContent = new GachaAnalysisView(SelectedUserData);
                }
            }
            if (header == "SideMenu_WishOriginalData" && !(ViewContent is WishOriginalDataView))
            {
                if (SelectedUserData == null)
                {
                    ViewContent = new NoUidView();
                }
                else
                {
                    ViewContent = new WishOriginalDataView(SelectedUserData);
                }
            }
            if (header == "SideMenu_About")
            {

                ViewContent = new AboutView();
            }
        }

        public async Task ChangeAvatar()
        {
            var result = await Dialog.Show(new ChangeAvatarDialog()).Initialize<ChangeAvatarDialog>(x => { }).GetResultAsync<string>();
            if (!string.IsNullOrEmpty(result))
            {
                if (SelectedUserData != null)
                {
                    SelectedUserData.Avatar = result;
                }
                else
                {
                    Growl.Info("请先加载数据");
                }
            }
        }

        public async Task ChangeUid()
        {
            var result = await Dialog.Show(new ChangeUidDialog(UserDataList)).Initialize<ChangeUidDialog>(x => { }).GetResultAsync<UserData>();
            if (result != null)
            {
                SelectedUserData = result;
            }
        }

    }
}
