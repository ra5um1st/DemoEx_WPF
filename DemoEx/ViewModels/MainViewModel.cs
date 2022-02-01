using DemoEx.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEx.WPF.ViewModels
{
    class MainViewModel : ViewModel
    {
        public MainViewModel() 
        {

        }

        private ViewModel currentViewModel;

        public ViewModel CurrentViewModel
        {
            get => currentViewModel;
            set => Set(ref currentViewModel, ref value);
        }
    }
}
