using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
using WCFServer.WPFApp.ViewModels;

namespace WCFServer.WPFApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(DialogCoordinator.Instance);
            DataContext = mainViewModel;
            //bool ss = IsAdministrator();
            //int i = 0;
        }

        /// <summary>
        /// 是否是管理打开cc/
        /// </summary>
        /// <returns></returns>
        public bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
