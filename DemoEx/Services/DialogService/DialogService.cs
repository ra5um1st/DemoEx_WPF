using DemoEx.Domain;
using DemoEx.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DemoEx.WPF.Services
{
    class DialogService<T> : IDialogService where T : DialogViewModel
    {
        public DialogService(T dialog, Window owner)
        {
            this.dialog = dialog;
            this.dialog.Window.DataContext = dialog;
            this.dialog.Window.Owner = owner;
        }
        public object DialogResult => dialog.DialogResult;
        public bool? ShowDialog() => dialog.Window.ShowDialog();

        private T dialog;
    }
}
