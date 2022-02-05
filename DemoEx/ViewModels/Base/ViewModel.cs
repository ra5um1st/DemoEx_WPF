using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DemoEx.WPF.ViewModels.Base
{
    class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool Set<T>(ref T field, ref T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool isDisposed = false;
        protected virtual void Dispose(bool isDisposing)
        {
            if(isDisposing && isDisposed)
            {
                return;
            }
            isDisposed = true;

        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
