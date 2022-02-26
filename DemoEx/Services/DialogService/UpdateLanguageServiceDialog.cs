using DemoEx.Domain;
using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using ICSharpCode.Decompiler.Util;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace DemoEx.WPF.Services
{
    class UpdateLanguageServiceDialog : ViewModel, IDialogService, INotifyDataErrorInfo
    {

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public UpdateLanguageServiceDialog(LanguageService service, IRepository<LanguageService> languageServiceRepository)
        {
            this.languageServiceRepository = languageServiceRepository;
            this.service = service;

            id = service.Id;
            serviceName = service.ServiceName;
            Duration = (int)service.Duration;
            cost = (decimal)service.Cost;
            discount = (int)service.Discount;
            imagePath = service.ImagePath;

            errorsDictionary = new Dictionary<string, List<string>>();
            childWindow = new UpdateLanguageServiceWindow()
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
        private Dictionary<string, List<string>> errorsDictionary;
        private LanguageService service;

        #endregion Fields

        #region Properties
        public bool HasErrors => errorsDictionary.Any();

        public IEntity DialogResult
        {
            get
            {
                service.ServiceName = serviceName;
                service.Duration = Duration;
                service.Cost = cost;
                service.Discount = discount;
                service.ImagePath = imagePath;
                return service;
            }
        }

        private int id;
        public int Id => id;

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

        private int durationHours = 0;
        public int DurationHours
        {
            get => durationHours;
            set
            {
                if (value < 0)
                {
                    durationHours = 0;
                    AddError(nameof(DurationHours), "Время не может быть отрицательным");
                }
                else if (value > 3)
                {
                    durationHours = 3;
                    AddError(nameof(DurationHours), "Услуга не может быть долше 4 часов");
                }
                else
                {
                    Set(ref durationHours, ref value);
                }
            }
        }

        private int durationMinutes = 0;
        public int DurationMinutes
        {
            get => durationMinutes;
            set
            {
                if (value < 0)
                {
                    durationMinutes = 0;
                    AddError(nameof(DurationHours), "Время не может быть отрицательным");
                }
                else if (value > 59)
                {
                    durationMinutes = 59;
                    AddError(nameof(DurationHours), "Минут не более 59");
                }
                else
                {
                    Set(ref durationMinutes, ref value);
                }
            }
        }

        public int Duration
        {
            get => (durationHours * 60) + durationMinutes;
            set
            {
                durationHours = value / 60;
                durationMinutes = value % 60;
            }
        }

        private decimal cost = 0;
        public decimal Cost
        {
            get => cost;
            set
            {
                if (value < 0)
                {
                    cost = 0;
                    AddError(nameof(DurationHours), "Цена не может быть отрицательной");
                }
                else
                {
                    Set(ref cost, ref value);
                }
            }
        }

        private int discount = 0;
        public int Discount
        {
            get => discount;
            set
            {
                if (value < 0)
                {
                    discount = 0;
                    AddError(nameof(DurationHours), "Скидка не может быть отрицательной");
                }
                else if (value > 100)
                {
                    discount = 100;
                    AddError(nameof(DurationHours), "Скидка не может быть больше 100%");
                }
                else
                {
                    Set(ref discount, ref value);
                }
            }
        }

        #endregion Properties

        #region Commands
        public ICommand SubmitCommand { get; }
        private bool CanSubmitCommandExecute(object obj)
        {
            if (ServiceName == string.Empty)
            {
                return false;
            }
            return true;
        }
        private void OnSubmitCommandExecute(object obj)
        {
            if (ImagePath != null)
            {
                string imageName = Path.GetFileName(ImagePath);
                string applicationResourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Language Service Images/";
                string newImagePath = Path.Combine(applicationResourceDirectory, imageName);
                if (!File.Exists(newImagePath))
                {
                    File.Copy(ImagePath, newImagePath, false);
                }
                ImagePath = newImagePath;
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
        public object ResXResourceWriter { get; private set; }

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
                string fileName = Path.GetFileName(fileDialog.FileName);
                if (Regex.IsMatch(fileName, "[а-яА-Я]"))
                {
                    MessageBox.Show("Имя выбранного файла не должно содержать русских символов. Приложение их не поддерживает. Измените их на английские.", "Неверное имя файла", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ImagePath = fileDialog.FileName;
                }
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
            return errorsDictionary.GetValueOrDefault(propertyName, null);
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!errorsDictionary.ContainsKey(propertyName))
            {
                errorsDictionary.Add(propertyName, new List<string>());
            }
            if (!errorsDictionary[propertyName].Contains(errorMessage))
            {
                errorsDictionary[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
        #endregion Methods
    }
}