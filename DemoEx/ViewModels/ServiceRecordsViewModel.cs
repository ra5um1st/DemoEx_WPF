using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DemoEx.WPF.ViewModels
{
    class ServiceRecordsViewModel : ViewModel
    {
        public ServiceRecordsViewModel(IRepository<ServiceRecord> serviceRecordsRepository)
        {
            this.serviceRecordsRepository = serviceRecordsRepository;
            serviceRecordList = serviceRecordsRepository.Items.Include(item => item.Service).ToList();
            serviceRecordsViewSource = new CollectionViewSource
            {
                Source = serviceRecordList
            };
        }

        #region Fields

        private IRepository<ServiceRecord> serviceRecordsRepository;
        private CollectionViewSource serviceRecordsViewSource;
        private List<ServiceRecord> serviceRecordList;

        #endregion

        #region Properties

        public ICollectionView ServiceRecordsView => serviceRecordsViewSource.View;

        #endregion

        #region Commands



        #endregion
    }
}
