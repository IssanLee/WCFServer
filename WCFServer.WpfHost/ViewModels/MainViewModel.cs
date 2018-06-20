using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WCFServer.Manager.Config;
using WCFServer.Manager.Provider;
using WCFServer.WPFApp.Models;
using WCFServer.WPFApp.Models.Data;
using WCFServer.WPFApp.Views;

namespace WCFServer.WPFApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// MahApps.Metro 风格dialog
        /// </summary>
        private readonly IDialogCoordinator _dialogCoordinator;

        public List<ServiceInfo> ServiceInfos { get; set; }

        public TextBox TextBoxCmd;

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            DataProvider.Instance.InitData(ServiceSwitch);

            ServiceInfos = DataProvider.ServiceInfos;
        }

        #region 命令

        #region 锁定程序
        private ICommand lockAppCmd;
        public ICommand LockAppCmd { get { return lockAppCmd ?? (lockAppCmd = new RelayCommand(p => LockApp() )); }}
        #endregion

        #region 退出程序
        private ICommand closeAppCmd;
        public ICommand CloseAppCmd { get { return closeAppCmd ?? (closeAppCmd = new RelayCommand(p => ExitApp() ));}}
        #endregion

        #region 加载完毕事件
        private ICommand windowsLoaded;
        public ICommand WindowsLoaded { get { return windowsLoaded ?? (windowsLoaded = new RelayCommand(p => Loaded((TextBox)p))); } }
        #endregion

        #region 服务刷新按钮
        private ICommand refreshServiceCmd;
        public ICommand RefreshServiceCmd { get { return refreshServiceCmd ?? (refreshServiceCmd = new RelayCommand(p => RefreshService())); } }
        #endregion

        #region 服务全部开启按钮
        private ICommand openAllServiceCmd;
        public ICommand OpenAllServiceCmd { get { return openAllServiceCmd ?? (openAllServiceCmd = new RelayCommand(p => OpenAllService())); } }
        #endregion

        #region 服务全部关闭按钮
        private ICommand closeAllServiceCmd;
        public ICommand CloseAllServiceCmd { get { return closeAllServiceCmd ?? (closeAllServiceCmd = new RelayCommand(p => CloseAllService())); } }
        #endregion

        #endregion

        #region 方法

        #region 锁定程序
        private async void LockApp()
        {
            var lockDialog = new CustomDialog() { Title = "程序已锁定,请输入密码" };
            var dataContext = new LockDialogContent(x =>
            {
                if (x.UserName == "admin" && x.Password == "password")
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, lockDialog);
                }
            });
            lockDialog.Content = new LockDialog { DataContext = dataContext };
            await _dialogCoordinator.ShowMetroDialogAsync(this, lockDialog);
        }
        #endregion

        #region 退出程序
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region 服务开关
        private void ServiceSwitch(ServiceInfo serviceInfo)
        {
            List<ServiceType> serviceTypeList = new List<ServiceType>();
            ServiceType serviceType = new ServiceType
            {
                IntfType = typeof(Service.Demo.Contract.IService),
                ImplType = typeof(Service.Demo.Implement.Service),
                WcfConfig = new WcfConfig
                {
                    IP = "localhost",
                    Port = "1028",
                    Endpoint = "json"
                },
                LogAction = UICmd
            };
            if (serviceInfo.ServiceStatus)
            {
                //ServiceProvider.Instance.
            }
            else
            {
                serviceTypeList.Add(serviceType);
                ServiceProvider.Instance.AddService(serviceTypeList);
            }
        }
        #endregion

        #region
        private void Loaded(TextBox textBox)
        {
            TextBoxCmd = textBox;
        }
        #endregion

        #region
        private void RefreshService()
        {

        }
        #endregion

        #region
        private void OpenAllService()
        {

        }
        #endregion

        #region
        private void CloseAllService()
        {

        }
        #endregion

        private void UICmd(string s)
        {
            TextBoxCmd.Text += s;
            TextBoxCmd.Text += Environment.NewLine;
            TextBoxCmd.Select(TextBoxCmd.Text.Length, 0);
        }

        #endregion

    }
}
