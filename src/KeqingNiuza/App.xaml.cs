using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KeqingNiuza.Common;

namespace KeqingNiuza
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 0)
            {
                if (e.Args[0] == "ExportFileList")
                {
                    Util.ExportUpdateFileList();
                    Util.ExportResourceFileList();
                }
                Shutdown();
            }
        }
    }
}
