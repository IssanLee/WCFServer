using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WCFServer.WPFApp.Models
{
    public class LockDialogContent : INotifyPropertyChanged
    {
        private string userName;
        private string password;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get { return closeCommand; }
        }

        public LockDialogContent(Action<LockDialogContent> closeAction)
        {
            closeCommand = new RelayCommand(p => closeAction(this));
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
