using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Services;
using DemoEx.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DemoEx
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost host;
        public static IHost Host => host;
        public App()
        {
            host = new HostBuilder().ConfigureServices(services =>
            {
                services.AddDbContext<LanguageCoursesDbContext>();

                services.AddSingleton<RoleService>();
                services.AddSingleton<MainWindow>();

                services.AddTransient<MainViewModel>();
                services.AddTransient<ServicePageViewModel>();
                services.AddTransient<ServiceRecordsViewModel>();

                services.AddTransient<IRepository<LanguageService>, Repository<LanguageService>>();
                services.AddTransient<IRepository<Gender>, Repository<Gender>>();
                services.AddTransient<IRepository<Person>, Repository<Person>>();
                services.AddTransient<IRepository<ServiceRecord>, Repository<ServiceRecord>>();
            })
            .Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = host.Services.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            host.StopAsync();
            host.Dispose();
            base.OnExit(e);
        }
    }
}
