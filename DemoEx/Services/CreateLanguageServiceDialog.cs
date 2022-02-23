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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
            Duration = this.Duration,
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

        private int durationHours = 0;
        public int DurationHours
        {
            get => durationHours;
            set
            {
                if (value < 0)
                {
                    durationHours = 0;
                }
                else if (value > 3)
                {
                    durationHours = 3;
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
                if(value < 0)
                {
                    durationMinutes = 0;
                }
                else if(value > 59)
                {
                    durationMinutes = 59;
                }
                else
                {
                    Set(ref durationMinutes, ref value);
                }
            }
        }

        public int Duration
        {
            get => durationHours * 60 + durationMinutes;
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
                }
                else if (value > 100)
                {
                    discount = 100;
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
            if (languageServiceRepository.Items
               .ToList()
               .Where(item => item.ServiceName.Trim().ToLower() == ServiceName.Trim().ToLower())
               .ToList().Count != 0)
            {
                MessageBox.Show("Запись с таким названием уже существует");
                return;
            }
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
                if(Regex.IsMatch(fileName, "[а-яА-Я]"))
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
            throw new NotImplementedException();
        }
        #endregion Methods
    }
}