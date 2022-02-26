using DemoEx.Domain;
using DemoEx.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DemoEx.WPF.Services
{
    public interface IDialogService
    {
        public bool? ShowDialog();
        public IEntity DialogResult { get; }
    }
}
