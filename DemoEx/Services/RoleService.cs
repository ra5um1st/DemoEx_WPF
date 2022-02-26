using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DemoEx.WPF.Services
{
    class RoleService : INotifyPropertyChanged
    {
        private bool administratorMode;
        public bool AdministratorMode
        {
            get => administratorMode;
            set
            {
                if(Equals(administratorMode, value))
                {
                    return;
                }
                administratorMode = value;
                OnPropertyChanged(nameof(AdministratorMode));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
