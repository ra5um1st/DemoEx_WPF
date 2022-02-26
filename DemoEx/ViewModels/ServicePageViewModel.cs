using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.Services;
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
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DemoEx.WPF.ViewModels
{
    class ServicePageViewModel : ViewModel
    {
        public ServicePageViewModel(IRepository<LanguageService> languageServiceRepository, RoleService roleService)
        {
            RoleService = roleService;
            this.languageServiceRepository = languageServiceRepository;
            this.languageServices = languageServiceRepository.Items.Include(item => item.ServiceRecords).ToList(); ;

            searchFilter = "";
            currentSortingProperty = nameof(LanguageService.Id);
            SortingPropertiesDictionary = new Dictionary<string, string>()
            {
                { nameof(LanguageService.Id), "идентификатора" },
                { nameof(LanguageService.ServiceName), "названия"},
                { nameof(LanguageService.DiscountCost), "цены"},
                { nameof(LanguageService.Discount), "скидки"},
                { nameof(LanguageService.Duration), "длительности"}
            };
            SortingDirectionsDictionary = new Dictionary<ListSortDirection, string>()
            {
                { ListSortDirection.Descending, "По убыванию" },
                { ListSortDirection.Ascending, "По возрастанию" }
            };
            DiscountFilterDictionary = new Dictionary<(int, int), string>() 
            {
                { (0, 100), "Любая" },
                { (0, 5), "0% - 5%" },
                { (5, 15), "5% - 15%" },
                { (15, 30), "15% - 30%" },
                { (30, 70), "30% - 70%" },
                { (70, 100), "70% - 100%" }
            };

            languageServiceSource = new CollectionViewSource()
            {
                Source = this.languageServices,
                IsLiveSortingRequested = true,
                IsLiveFilteringRequested = true,
                IsLiveGroupingRequested = true
            };
            languageServiceSource.Filter += OnLanguageServiceNameFilter;
            languageServiceSource.Filter += OnLanguageServiceDiscountFilter;

            CreateServiceCommand = new LambdaCommand(OnCreateServiceCommandExecute);
            UpdateServiceCommand = new LambdaCommand(OnUpdateServiceCommandExecute, CanUpdateServiceCommandExecute);
            RemoveServiceCommand = new LambdaCommand(OnRemoveServiceCommandExecute, CanRemoveServiceCommandExecute);
        }

        #region Fields

        private readonly IRepository<LanguageService> languageServiceRepository;
        private readonly CollectionViewSource languageServiceSource;

        #endregion

        #region Properties

        public RoleService RoleService { get; }

        private LanguageService selectedLanguageService;
        public LanguageService SelectedLanguageService
        {
            get => selectedLanguageService;
            set => Set(ref selectedLanguageService, ref value);
        }

        public Dictionary<(int, int), string> DiscountFilterDictionary { get; }

        private (int, int) currentDiscountFilter;
        public (int, int) CurrentDiscountFilter
        {
            get => currentDiscountFilter;
            set
            {
                Set(ref currentDiscountFilter, ref value);
                languageServiceSource.View.Refresh();
            }
        }

        public Dictionary<string, string> SortingPropertiesDictionary { get; }
        public Dictionary<ListSortDirection, string> SortingDirectionsDictionary { get; }

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
        private void OnCreateServiceCommandExecute(object obj)
        {
            CreateLanguageServiceDialog dialog = new CreateLanguageServiceDialog(languageServiceRepository);
            if (dialog.ShowDialog() == true)
            {
                LanguageService serviceToAdd = (LanguageService)dialog.DialogResult;
                languageServiceRepository.AddAsync(serviceToAdd);
                ((List<LanguageService>)languageServices).Add(serviceToAdd);
                OnPropertyChanged(nameof(TotalItemsCount));
                languageServiceSource.View.Refresh();
            }
        }

        public LambdaCommand UpdateServiceCommand { get; set; }
        private async void OnUpdateServiceCommandExecute(object obj)
        {
            LanguageService service = (LanguageService)obj;
            var serviceToUpdate = service ?? SelectedLanguageService;
            serviceToUpdate = await languageServiceRepository.GetAsync(serviceToUpdate.Id);

            UpdateLanguageServiceDialog dialog = new UpdateLanguageServiceDialog(serviceToUpdate, languageServiceRepository);
            if (dialog.ShowDialog() == true)
            {
                serviceToUpdate = (LanguageService)dialog.DialogResult;
                await languageServiceRepository.UpdateAsync(serviceToUpdate);
                languageServiceSource.View.Refresh();
            }
        }
        private bool CanUpdateServiceCommandExecute(object obj) => obj != null || SelectedLanguageService != null;

        public LambdaCommand RemoveServiceCommand { get; set; }
        private void OnRemoveServiceCommandExecute(object obj)
        {
            LanguageService service = (LanguageService)obj;
            var serviceToDelete = service ?? SelectedLanguageService;
            var dialogResult = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(dialogResult == MessageBoxResult.Yes)
            {
                languageServiceRepository.RemoveAsync(serviceToDelete.Id);
                ((List<LanguageService>)languageServices).Remove(serviceToDelete);
                OnPropertyChanged(nameof(TotalItemsCount));
                languageServiceSource.View.Refresh();
            }
        }
        private bool CanRemoveServiceCommandExecute(object obj) => obj != null || SelectedLanguageService != null;
        #endregion

        #region Functions
        private void OnLanguageServiceNameFilter(object sender, FilterEventArgs e)
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

        private void OnLanguageServiceDiscountFilter(object sender, FilterEventArgs e)
        {
            if (e.Item == null || e.Item.GetType() != typeof(LanguageService))
            {
                return;
            }

            LanguageService service = (LanguageService)e.Item;
            if (service.Discount < currentDiscountFilter.Item1 || service.Discount > currentDiscountFilter.Item2)
            {
                e.Accepted = false;
            }
        }
        #endregion

    }
}
