using DemoEx.Domain;
using DemoEx.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DemoEx.WPF.ViewModels.Base
{
    abstract class DialogViewModel : ViewModel
    {
        public abstract Window Window { get; }
        public abstract ICommand SubmitCommand { get; }
        public abstract ICommand CancelCommand { get; }
        public abstract IEntity DialogResult { get; }
    }
}
