using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.Services;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace DemoEx.WPF.ViewModels
{
    class ServiceRecordsViewModel : ViewModel
    {
        public ServiceRecordsViewModel(IRepository<ServiceRecord> serviceRecordsRepository, IRepository<LanguageService> languageServicesRepository, IRepository<Person> personsRepository)
        {
            this.serviceRecordsRepository = serviceRecordsRepository;
            this.languageServicesRepository = languageServicesRepository;
            this.personsRepository = personsRepository;
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();
            timer.Tick += Timer_Tick;

            serviceRecordList = serviceRecordsRepository.Items.Include(item => item.Service).ToList();
            serviceRecordsViewSource = new CollectionViewSource
            {
                Source = serviceRecordList
            };

            AddServiceRecordCommand = new LambdaCommand(OnAddServiceRecordExecute);
            RemoveServiceRecordCommand = new LambdaCommand(OnRemoveServiceRecordExecute);
        }

        #region Fields

        private DispatcherTimer timer;
        private IRepository<ServiceRecord> serviceRecordsRepository;
        private IRepository<LanguageService> languageServicesRepository;
        private IRepository<Person> personsRepository;
        private CollectionViewSource serviceRecordsViewSource;
        private List<ServiceRecord> serviceRecordList;

        #endregion

        #region Properties

        public ICollectionView ServiceRecordsView => serviceRecordsViewSource.View;

        #endregion

        #region Commands

        public ICommand AddServiceRecordCommand { get; }
        private void OnAddServiceRecordExecute(object obj)
        {
            AddServiceRecordDialog dialog = new AddServiceRecordDialog(serviceRecordsRepository, languageServicesRepository, personsRepository);
            DialogService<AddServiceRecordDialog> dialogService = new DialogService<AddServiceRecordDialog>(dialog, App.Current.MainWindow);

            if (dialogService.ShowDialog() == true)
            {
                var serviceRecord = (ServiceRecord)dialog.DialogResult;
                serviceRecordsRepository.AddAsync(serviceRecord);
                serviceRecordList.Add(serviceRecord);
                ServiceRecordsView.Refresh();
            }
        }

        public ICommand RemoveServiceRecordCommand { get; }
        private void OnRemoveServiceRecordExecute(object obj)
        {
            ServiceRecord serviceRecord = (ServiceRecord)obj;
            var dialogResult = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialogResult == MessageBoxResult.Yes)
            {
                serviceRecordsRepository.RemoveAsync(serviceRecord.Id);
                serviceRecordList.Remove(serviceRecord);
            }
        }
        #endregion

        #region Methods
        private void Timer_Tick(object sender, EventArgs e)
        {
            ServiceRecordsView.Refresh();
        }

        #endregion
    }
}
