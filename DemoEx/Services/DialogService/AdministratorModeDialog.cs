using DemoEx.Domain;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemoEx.WPF.Services.DialogService
{
    class AdministratorModeDialog : DialogViewModel
    {
        public AdministratorModeDialog()
        {
            window = new AdministratorMode();

            SubmitCommand = new LambdaCommand(OnSubmitCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecute);
        }

        private string code;
        public string Code
        {
            get => code;
            set => Set(ref code, ref value);
        }

        private Window window;
        public override Window Window => window;

        public override ICommand SubmitCommand { get; }
        private void OnSubmitCommandExecute(object obj)
        {
            PasswordBox passwordBox = (PasswordBox)obj;
            code = passwordBox.Password;

            window.DialogResult = true;
            window.Close();
        }

        public override ICommand CancelCommand { get; }
        private void OnCancelCommandExecute(object obj)
        {
            window.DialogResult = false;
            window.Close();
        }

        public override object DialogResult => code;
    }
}
