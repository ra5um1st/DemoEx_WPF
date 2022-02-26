using DemoEx.WPF.Commands;
using DemoEx.WPF.Services;
using DemoEx.WPF.Services.DialogService;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DemoEx.WPF.ViewModels
{
    class MainViewModel : ViewModel
    {
        public MainViewModel(RoleService roleService)
        {
            RoleService = roleService;

            administratorCode = "0000";
            ActivateAdministratorModeCommand = new LambdaCommand(OnActivateAdministratorModeCommandExecute);
        }

        #region Fields

        private readonly string administratorCode;

        #endregion

        #region Properties

        private string title = "Демоэкзамен";
        public string Title
        {
            get => title;
            set => Set(ref title, ref value);
        }

        public RoleService RoleService { get; }

        public ViewModel Services => App.Host.Services.GetRequiredService<ServicePageViewModel>();
        public ViewModel ServiceRecords => App.Host.Services.GetRequiredService<ServiceRecordsViewModel>();

        #endregion

        #region Commdans

        public ICommand ActivateAdministratorModeCommand { get; }
        private void OnActivateAdministratorModeCommandExecute(object obj)
        {
            if (!RoleService.AdministratorMode)
            {
                AdministratorModeDialog dialog = new AdministratorModeDialog();
                DialogService<AdministratorModeDialog> dialogService = new DialogService<AdministratorModeDialog>(dialog, App.Current.MainWindow);
                if (dialogService.ShowDialog() == true)
                {
                    if (string.Equals(administratorCode, (string)dialogService.DialogResult))
                    {
                        RoleService.AdministratorMode = true;
                    }
                    else
                    {
                        MessageBox.Show("Код неверный", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBoxResult dialogResult =  MessageBox.Show("Вы уверены, что хотите выйти из режима администратора?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(dialogResult == MessageBoxResult.Yes)
                {
                    RoleService.AdministratorMode = false;
                }
            }
        }

        #endregion
    }
}