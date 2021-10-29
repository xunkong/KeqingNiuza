using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeqingNiuza.Core.Wish;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.ViewModel;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishlogBackupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WishlogBackupWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WishlogBackupWindow()
        {
            InitializeComponent();
            UserDataList = MainWindowViewModel.GetUserDataList();
        }


        private List<WishData> _wishlogList;


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
                SelectedUserData_Changed();
                OnPropertyChanged();
            }
        }


        private string _StateInfoText;
        public string StateInfoText
        {
            get { return _StateInfoText; }
            set
            {
                _StateInfoText = value;
                OnPropertyChanged();
            }
        }


        private string _RequestInfoText;
        public string RequestInfoText
        {
            get { return _RequestInfoText; }
            set
            {
                _RequestInfoText = value;
                OnPropertyChanged();
            }
        }

        private bool _ButtonEnable = true;
        public bool ButtonEnable
        {
            get { return _ButtonEnable; }
            set
            {
                _ButtonEnable = value;
                OnPropertyChanged();
            }
        }



        private void SelectedUserData_Changed()
        {
            if (_SelectedUserData is null)
            {
                return;
            }
            if (!File.Exists(_SelectedUserData.WishLogFile))
            {
                StateInfoText = $"没有找到祈愿记录文件：{_SelectedUserData.WishLogFile}";
            }
            try
            {
                var json = File.ReadAllText(_SelectedUserData.WishLogFile);
                _wishlogList = JsonSerializer.Deserialize<List<WishData>>(json);
                _wishlogList.ForEach(wishlog => wishlog.Uid = _SelectedUserData.Uid);
            }
            catch (Exception ex)
            {
                StateInfoText = $"发生了错误：{ex.Message}";
                return;
            }
            StateInfoText = $"选择了Uid：{_SelectedUserData.Uid}，本地共有祈愿记录{_wishlogList.Count}条";
        }


        private async void Button_Upload_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUserData == null || _wishlogList == null)
            {
                StateInfoText = "请选择Uid";
                return;
            }
            var client = new WishlogBackupService();
            try
            {
                ButtonEnable = false;
                StateInfoText = "正在上传，请稍等（最多30s）";
                var result = await client.ExecuteAsync(SelectedUserData.Uid, SelectedUserData.Url, "put", _wishlogList);
                if (result != null)
                {
                    StateInfoText = $"服务器上现有Uid{result.Uid}的祈愿记录{result.CurrentCount}条，此次上传新增{result.PutCount}条";
                    return;
                }
                throw new Exception("Result is null");
            }
            catch (Exception ex)
            {
                if (ex.Message == "authkey timeout")
                {
                    StateInfoText = $"祈愿记录网址已过期，请重新加载数据";
                }
                else
                {
                    StateInfoText = $"发生错误：{ex.Message}";
                }
            }
            finally
            {
                ButtonEnable = true;
                RequestInfoText = client.RequestInfo;
            }
        }

        private async void Button_Get_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUserData == null || _wishlogList == null)
            {
                StateInfoText = "请选择Uid";
                return;
            }
            var client = new WishlogBackupService();
            try
            {
                ButtonEnable = false;
                StateInfoText = "正在下载，请稍等（最多30s）";
                var result = await client.ExecuteAsync(SelectedUserData.Uid, SelectedUserData.Url, "get", null);
                if (result != null)
                {
                    var dialog = new SaveFileDialog();
                    dialog.Filter = "json file|*.json";
                    dialog.AddExtension = true;
                    dialog.DefaultExt = ".json";
                    dialog.FileName = $"WishLog_{result.Uid}_{DateTime.Now:yyyyMMddHHmmss}.json";
                    dialog.OverwritePrompt = true;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var json = JsonSerializer.Serialize(result.List, Service.Const.JsonOptions);
                        File.WriteAllText(dialog.FileName, json);
                        StateInfoText = $"服务器上现有Uid{result.Uid}的祈愿记录{result.CurrentCount}条，此次下载{result.GetCount}条，保存成功";
                    }
                    else
                    {
                        StateInfoText = $"服务器上现有Uid{result.Uid}的祈愿记录{result.CurrentCount}条，此次下载{result.GetCount}条，但未保存文件";
                    }
                    return;
                }
                throw new Exception("Result is null");
            }
            catch (Exception ex)
            {
                if (ex.Message == "authkey timeout")
                {
                    StateInfoText = $"祈愿记录网址已过期，请重新加载数据";
                }
                else
                {
                    StateInfoText = $"发生错误：{ex.Message}";
                }
            }
            finally
            {
                ButtonEnable = true;
                RequestInfoText = client.RequestInfo;
            }
        }

        private async void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUserData == null || _wishlogList == null)
            {
                StateInfoText = "请选择Uid";
                return;
            }
            var client = new WishlogBackupService();
            try
            {
                ButtonEnable = false;
                StateInfoText = "正在删除，请稍等（最多30s）";
                var result = await client.ExecuteAsync(SelectedUserData.Uid, SelectedUserData.Url, "delete", null);
                if (result != null)
                {
                    StateInfoText = $"服务器上现有Uid{result.Uid}的祈愿记录{result.CurrentCount}条，此次删除{result.DeleteCount}条";
                    return;
                }
                throw new Exception("Result is null");
            }
            catch (Exception ex)
            {
                if (ex.Message == "authkey timeout")
                {
                    StateInfoText = $"祈愿记录网址已过期，请重新加载数据";
                }
                else
                {
                    StateInfoText = $"发生错误：{ex.Message}";
                }
            }
            finally
            {
                ButtonEnable = true;
                RequestInfoText = client.RequestInfo;
            }
        }
    }
}
