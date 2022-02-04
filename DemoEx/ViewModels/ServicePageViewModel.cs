using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoEx.WPF.ViewModels
{
    class ServicePageViewModel : ViewModel
    {
        public ServicePageViewModel(IRepository<LanguageService> languageServicesRepository)
        {
            this.languageServicesRepository = languageServicesRepository;

            var languageServices = languageServicesRepository.Items.Include(item => item.ServiceRecords).ToList();
            languageServices.ForEach(item => item.ImagePath = item.ImagePath.Insert(0, "..\\..\\..\\Resources\\"));
            this.languageServices = languageServices;

            CreateServiceCommand = new LambdaCommand(CreateServiceCommandExecute);
        }
        private readonly IRepository<LanguageService> languageServicesRepository;
        private IEnumerable<LanguageService> languageServices;
        public IEnumerable<LanguageService> LanguageServices => languageServices;
        public LambdaCommand CreateServiceCommand { get; set; }
        private void CreateServiceCommandExecute(object obj)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            addServiceWindow.Owner = App.Host.Services.GetRequiredService<MainWindow>();
            addServiceWindow.ShowDialog();
        }
    }
}
