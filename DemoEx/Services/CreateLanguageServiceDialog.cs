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

            languageService = new LanguageService();
            childWindow = new AddServiceWindow()
            {
                Owner = App.Current.MainWindow,
                DataContext = this
            };

            SubmitCommand = new LambdaCommand(OnAcceptCommandExecute, CanCreateCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecute);
            AddImageCommand = new LambdaCommand(OnAddImagesCommandExecute);
        }

        #region Fields
        private Window childWindow;
        private readonly IRepository<LanguageService> languageServiceRepository;
        private LanguageService languageService;
        #endregion Fields

        #region Properties
        public bool HasErrors => throw new NotImplementedException();

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
                languageService.ServiceName = serviceName;
                Set(ref serviceName, ref value);
            }
        }
        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                languageService.ImagePath = imagePath;
                Set(ref imagePath, ref value);
            }
        }
        private int duration;
        public int Duration
        {
            get => duration;
            set
            {
                languageService.Duration = duration;
                Set(ref duration, ref value);
            }
        }

        private decimal cost;
        public decimal Cost
        {
            get => cost;
            set
            {
                languageService.Cost = cost;
                Set(ref cost, ref value);
            }
        }

        private int discount;
        public int Discount
        {
            get => discount;
            set
            {
                languageService.Discount = discount;
                Set(ref discount, ref value);
            }
        }
        #endregion Properties

        #region Commands
        public ICommand SubmitCommand { get; }
        private bool CanCreateCommandExecute(object obj)
        {
            return languageService.ServiceName == null ? false : languageServiceRepository.Items
                .ToList()
                .Where(item => item.ServiceName.ToLower() == languageService.ServiceName.ToLower())
                .ToList().Count == 0;
        }
        private void OnAcceptCommandExecute(object obj)
        {
            if (!CanCreateCommandExecute(obj))
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
            if (fileDialog.ShowDialog() != true)
            {
                return;
            }
            ImagePath = fileDialog.FileName;
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