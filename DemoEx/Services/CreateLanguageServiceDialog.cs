using DemoEx.Domain;
using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DemoEx.WPF.Services
{
    class CreateLanguageServiceDialog : ViewModel, IDialogService, INotifyDataErrorInfo
    {

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public CreateLanguageServiceDialog(IRepository<LanguageService> languageServiceRepository)
        {
            this.languageServiceRepository = languageServiceRepository;

            childWindow = new AddServiceWindow()
            {
                Owner = App.Current.MainWindow,
                DataContext = this
            };

            SubmitCommand = new LambdaCommand(OnSubmitCommandExecute, CanSubmitCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecute);
            AddImageCommand = new LambdaCommand(OnAddImagesCommandExecute);
        }

        #region Fields

        private Window childWindow;
        private readonly IRepository<LanguageService> languageServiceRepository;

        #endregion Fields

        #region Properties
        public bool HasErrors => false;

        public IEntity DialogResult => new LanguageService()
        {
            ServiceName = serviceName,
            ImagePath = imagePath,
            Duration = duration,
            Cost = cost,
            Discount = discount
        };

        private string serviceName;
        public string ServiceName 
        {
            get => serviceName;
            set
            {
                Set(ref serviceName, ref value);
            }
        }
        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                Set(ref imagePath, ref value);
            }
        }
        private int duration;
        public int Duration
        {
            get => duration;
            set
            {
                Set(ref duration, ref value);
            }
        }

        private decimal cost;
        public decimal Cost
        {
            get => cost;
            set
            {
                Set(ref cost, ref value);
            }
        }

        private int discount;
        public int Discount
        {
            get => discount;
            set
            {
                Set(ref discount, ref value);
            }
        }
        #endregion Properties

        #region Commands
        public ICommand SubmitCommand { get; }
        private bool CanSubmitCommandExecute(object obj)
        {
            return true;
            //return languageService.ServiceName == null ? false : languageServiceRepository.Items
            //    .ToList()
            //    .Where(item => item.ServiceName.ToLower() == languageService.ServiceName.ToLower())
            //    .ToList().Count == 0;
        }
        private void OnSubmitCommandExecute(object obj)
        {
            if (!CanSubmitCommandExecute(obj))
            {
                return;
            }

            childWindow.DialogResult = true;
            childWindow.Close();
        }

        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecute(object obj)
        {
            childWindow.DialogResult = false;
            childWindow.Close();
        }

        public ICommand AddImageCommand { get; }

        private void OnAddImagesCommandExecute(object obj)
        {
            if (obj != null)
            {
                return;
            }
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Title = "Выбор изображения",
                Filter = "Изображения (*.PNG;*.JPEG;*.JPG)|*.PNG;*.JPEG;*.JPG",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };
            if (fileDialog.ShowDialog() == true)
            {
                ImagePath = fileDialog.FileName;
            }
            else
            {

            }
        }
        #endregion Commands

        #region Methods
        public bool? ShowDialog()
        {
            return childWindow.ShowDialog();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        #endregion Methods
    }
}