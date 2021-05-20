using HandyControl.Controls;
using HandyControl.Tools.Extension;
using KeqingNiuza.CloudBackup;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.View;
using KeqingNiuza.Wish;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using static KeqingNiuza.Common.Const;

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
            _viewContentList = new List<object>();
            Directory.CreateDirectory(".\\UserData");
            if (File.Exists("UserData\\Config.json"))
            {
                LoadConfig();
                ChangeViewContent("GachaAnalysisView");
            }
            else
            {
                UserDataList = new List<UserData>();
                ViewContent = new WelcomeView();
            }
            if (File.Exists("resource\\ShowUpdateLog"))
            {
                ViewContent = new UpdateLogView();
                File.Delete("resource\\ShowUpdateLog");
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

        private List<object> _viewContentList;


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

        public static UserData GetSelectedUserData()
        {
            return _SelectedUserData;
        }


        private static UserData _SelectedUserData;
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
            try
            {
                var result = await updater.UpdateResourceFiles();
                if (true)
                {
                    Log.OutputLog(LogType.Info, "Resource update finished");
                }
            }
            catch (Exception ex)
            {
                Log.OutputLog(LogType.Info, "TestUpdate_Resource", ex);
            }
            try
            {

                var updateInfo = await updater.GetUpdateInfo();
                if (updateInfo == (true, true))
                {
                    await updater.PrepareUpdateFiles();
                    Log.OutputLog(LogType.Info, "Update files prepare finished");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.OutputLog(LogType.Error, "TestUpdate_AllFile", ex);
                return false;
            }

        }

        public void CallUpdate(object sender, EventArgs e)
        {
            Process.Start("update\\Update.exe", "KeqingNiuza.Update");
            Log.OutputLog(LogType.Info, "Has called Update.exe");
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


        public async Task LoadCloudAccount()
        {
            await Task.Run(() =>
            {
                if (File.Exists("UserData\\Account"))
                {
                    try
                    {
                        CloudClient = CloudClient.GetClientFromEncryption();
                    }
                    catch (Exception ex)
                    {
                        File.Delete("UserData\\Account");
                        Growl.Error("无法解密云备份账户文件，已删除");
                        Log.OutputLog(LogType.Error, "DecrypeCloudClient", ex);
                    }
                }
            });
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
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
                Log.OutputLog(LogType.Error, "LoadDataFromGenshinLogFile", ex);
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
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
                Log.OutputLog(LogType.Error, "UpdateDataFromUrl", ex);
            }
        }


        private async Task UpdateDataFromUrl(string url)
        {
            var exporter = new WishLogExporter(url);
            var uid = await exporter.GetUidByUrl();
            var userData = UserDataList.Find(x => x.Uid == uid);
            List<WishData> oldList, newList;
            if (userData == null || !File.Exists(userData?.WishLogFile))
            {
                oldList = new List<WishData>();
                newList = await exporter.GetAllLog();
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
                oldList = LocalWishLogLoader.Load(userData.WishLogFile);
                newList = await exporter.GetAllLog(lastId: oldList.Last().Id);
                userData.Url = url;
                userData.LastUpdateTime = DateTime.Now;
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
            var analyzer = new PieChartAnalyzer(data);
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
                catch (Exception ex)
                {
                    Growl.Error(ex.Message);
                    Log.OutputLog(LogType.Error, "ExportExcelFile", ex);
                }
            }
        }


        /// <summary>
        /// 以压缩包形式备份用户文件
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
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
                Log.OutputLog(LogType.Error, "CloudBackupFileArchive", ex);
            }
        }


        /// <summary>
        /// 更换页面内容
        /// </summary>
        /// <param name="className">页面类名</param>
        public void ChangeViewContent(string className)
        {
            var assembly = Assembly.GetAssembly(GetType());
            var type = assembly.GetType("KeqingNiuza.View." + className);
            if (ViewContent?.GetType().Name != type.Name)
            {
                if (_viewContentList.Any(x => x.GetType().Name == type.Name))
                {
                    ViewContent = _viewContentList.First(x => x.GetType().Name == type.Name);
                }
                else
                {
                    try
                    {
                        ViewContent = assembly.CreateInstance(type.FullName);
                        _viewContentList.Add(ViewContent);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "没有数据")
                        {
                            ViewContent = new NoUidView();
                        }
                        else
                        {
                            Growl.Warning(ex.Message);
                            Log.OutputLog(LogType.Warning, "ChangeViewContent", ex);
                        }
                    }

                }
            }
        }




        /// <summary>
        /// 重新加载内容页面
        /// </summary>
        private void ReloadViewContent()
        {
            var type = ViewContent.GetType();
            try
            {
                ViewContent = type.Assembly.CreateInstance(type.FullName);
                _viewContentList.Clear();
                _viewContentList.Add(ViewContent);

            }
            catch (Exception ex)
            {
                if (ex.Message == "没有数据")
                {
                    ViewContent = new NoUidView();
                }
                else
                {
                    Growl.Warning(ex.Message);
                    Log.OutputLog(LogType.Warning, "ReloadViewContent", ex);
                }
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
                ReloadViewContent();
            }
        }



    }
}
