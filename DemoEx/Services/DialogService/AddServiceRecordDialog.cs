using DemoEx.Domain;
using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DemoEx.WPF.Services
{
    class AddServiceRecordDialog : DialogViewModel
    {
        public AddServiceRecordDialog(IRepository<ServiceRecord> serviceRecordRepository, IRepository<LanguageService> languageServiceRecordRepository, IRepository<Person> personRecordRepository)
        {
            this.window = new AddServiceRecord();
            this.selectedStartDate = DateTime.Now;
            this.serviceRecordRepository = serviceRecordRepository;
            this.languageServiceRecordRepository = languageServiceRecordRepository;
            this.personRecordRepository = personRecordRepository;

            SubmitCommand = new LambdaCommand(OnSubmitCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecute);
        }

        #region Fields

        private IRepository<ServiceRecord> serviceRecordRepository;
        private IRepository<LanguageService> languageServiceRecordRepository;
        private IRepository<Person> personRecordRepository;

        #endregion

        #region Properties

        public List<ServiceRecord> ServiceRecords => serviceRecordRepository.Items.Include(item => item.Service).Include(item => item.Person).ToList();
        public List<LanguageService> LanguageServices => languageServiceRecordRepository.Items.ToList();
        public List<Person> Persons => personRecordRepository.Items.ToList();

        private int personId;
        public int PersonId
        {
            get => personId;
            set => Set(ref personId, ref value);
        }

        private int languageServiceId;
        public int LanguageServiceId
        {
            get => languageServiceId;
            set => Set(ref languageServiceId, ref value);
        }

        private Person selectedPerson;
        public Person SelectedPerson
        {
            get => selectedPerson;
            set => Set(ref selectedPerson, ref value);
        }

        private LanguageService selectedLanguageService;
        public LanguageService SelectedLanguageService
        {
            get => selectedLanguageService;
            set => Set(ref selectedLanguageService, ref value);
        }

        private DateTime? selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get => selectedStartDate;
            set
            {
                if(value < DateTime.Now)
                {
                    MessageBox.Show("Запись не может осуществлятся на прошедшее время");
                    return;
                }
                Set(ref selectedStartDate, ref value); 
            }
        }

        private string startDateTime;
        public string StartDateTime
        {
            get => selectedStartDate.Value.ToString("t");
            set
            {
                DateTime tempDate;
                if(!DateTime.TryParse(value, out tempDate))
                {
                    return;
                }
                SelectedStartDate = new DateTime(selectedStartDate.Value.Year, selectedStartDate.Value.Month, selectedStartDate.Value.Day, tempDate.Hour, tempDate.Minute, 0);
                Set(ref startDateTime, ref value);
            }
        }

        public override IEntity DialogResult => new ServiceRecord
        {
            Service = selectedLanguageService,
            Person = selectedPerson,
            StartDate = selectedStartDate
        };

        private Window window;
        public override Window Window => window;

        #endregion

        #region Methods


        #endregion

        #region Commands

        public override ICommand SubmitCommand { get; }
        private void OnSubmitCommandExecute(object obj)
        {
            if(selectedLanguageService == null || selectedPerson == null)
            {
                MessageBox.Show("Заполните поля");
                return;
            }
            window.DialogResult = true;
            window.Close();
        }

        public override ICommand CancelCommand { get; }
        private void OnCancelCommandExecute(object obj)
        {
            window.DialogResult = false;
            window.Close();
        }
        #endregion
    }
}
