using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WCFServer.WPFApp.Models.Data
{
    /// <summary>
    /// 服务接口信息
    /// </summary>
    public class ServiceInfo : INotifyPropertyChanged
    {
        private string serviceName;

        private string serviceAddress;

        private bool serviceStatus;

        /// <summary>
        /// 接口名称
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
            set
            {
                if (value == serviceName) return;
                serviceName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ServiceAddress
        {
            get { return serviceAddress; }
            set
            {
                if (value == serviceAddress) return;
                serviceAddress = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 接口状态
        /// </summary>
        public bool ServiceStatus
        {
            get { return serviceStatus; }
            set
            {
                if (value == serviceStatus) return;
                serviceStatus = value;
                OnPropertyChanged();
            }
        }

        public Action<ServiceInfo> Action;

        private ICommand serviceCheckCmd;
        public ICommand ServiceCheckCmd { get { return serviceCheckCmd ?? (serviceCheckCmd = new RelayCommand(p => Action(this))); }}

        public ServiceInfo()
        {
            //serviceCheckCmd = new RelayCommand(p => action(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
