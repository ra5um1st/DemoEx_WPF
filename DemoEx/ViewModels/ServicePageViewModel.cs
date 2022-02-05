using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using DemoEx.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DemoEx.WPF.ViewModels
{
    class ServicePageViewModel : ViewModel
    {
        public ServicePageViewModel(IRepository<LanguageService> languageServicesRepository)
        {
            searchFilter = "";
            currentSortingProperty = nameof(LanguageService.Id);
            SortingProperties = new Dictionary<string, string>()
            {
                { nameof(LanguageService.Id), "идентификатора" },
                { nameof(LanguageService.ServiceName), "названия"},
                { nameof(LanguageService.DiscountCost), "цены"},
                { nameof(LanguageService.Discount), "скидки"},
                { nameof(LanguageService.Duration), "длительности"}
            };
            SortingDirections = new Dictionary<ListSortDirection, string>()
            {
                { ListSortDirection.Ascending, "По убыванию" },
                { ListSortDirection.Descending, "По возрастанию" }
            };

            this.languageServicesRepository = languageServicesRepository;

            var languageServices = languageServicesRepository.Items.Include(item => item.ServiceRecords).ToList();
            languageServices.ForEach(item => item.ImagePath = item.ImagePath.Insert(0, "..\\..\\..\\Resources\\"));
            languageServices.ForEach(item => item.DiscountCost = Convert.ToInt32(item.Discount > 0 ? item.Cost * (100 - item.Discount) / 100 : item.Cost));
            this.languageServices = languageServices;

            languageServiceSource = new CollectionViewSource()
            {
                Source = this.languageServices,
                IsLiveSortingRequested = true,
                IsLiveFilteringRequested = true,
                IsLiveGroupingRequested = true
            };
            languageServiceSource.Filter += OnLanguageServiceFilter;

            CreateServiceCommand = new LambdaCommand(CreateServiceCommandExecute);
        }

        #region Fields

        private readonly IRepository<LanguageService> languageServicesRepository;
        private readonly CollectionViewSource languageServiceSource;
        #endregion

        #region Functions
        private void OnLanguageServiceFilter(object sender, FilterEventArgs e)
        {
            if (e.Item == null || e.Item.GetType() != typeof(LanguageService))
            {
                return;
            }

            LanguageService service = (LanguageService)e.Item;
            if (!service.ServiceName.Contains(SearchFilter, StringComparison.CurrentCultureIgnoreCase))
            {
                e.Accepted = false;
            }
        }
        #endregion

        #region Properties

        public Dictionary<string, string> SortingProperties { get; }
        public Dictionary<ListSortDirection, string> SortingDirections { get; }
        private ListSortDirection currentSortingDirection;
        public ListSortDirection CurrentSortingDirection
        {
            get => currentSortingDirection;
            set
            {
                Set(ref currentSortingDirection, ref value);
                languageServiceSource.SortDescriptions.Clear();
                languageServiceSource.SortDescriptions.Add(new SortDescription(currentSortingProperty, CurrentSortingDirection));
                languageServiceSource.View.Refresh();
            }
        }

        private string currentSortingProperty;
        public string CurrentSortingProperty
        {
            get => currentSortingProperty;
            set
            {
                Set(ref currentSortingProperty, ref value);
                languageServiceSource.SortDescriptions.Clear();
                languageServiceSource.SortDescriptions.Add(new SortDescription(currentSortingProperty, CurrentSortingDirection));
                languageServiceSource.View.Refresh();
            }
        }

        private string searchFilter;
        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                if (Set(ref searchFilter, ref value))
                {
                    languageServiceSource.View.Refresh();
                }
            }
        }

        public ICollectionView LanguageServiceView => languageServiceSource.View;

        private IEnumerable<LanguageService> languageServices;
        public IEnumerable<LanguageService> LanguageServices => languageServices;
        public int TotalItemsCount => languageServices.Count();
        #endregion

        #region Commands
        public LambdaCommand CreateServiceCommand { get; set; }
        private void CreateServiceCommandExecute(object obj)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow()
            {
                Owner = App.Host.Services.GetRequiredService<MainWindow>()
            };
            addServiceWindow.ShowDialog();
        }
        #endregion
    }
}
