using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEx.WPF.ViewModels
{
    class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            currentViewModel = App.Host.Services.GetRequiredService<ServicePageViewModel>();
        }

        #region Commands
        #endregion

        #region Properties
        private string title = "Демоэкзамен";
        public string Title
        {
            get => title;
            set => Set(ref title, ref value);
        }

        private ViewModel currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => currentViewModel;
            set => Set(ref currentViewModel, ref value);
        }
        #endregion
    }
}
