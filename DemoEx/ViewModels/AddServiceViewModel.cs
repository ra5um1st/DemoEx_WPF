using DemoEx.Domain.Models;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEx.WPF.ViewModels
{
    class AddServiceViewModel : ViewModel
    {
        public AddServiceViewModel(LanguageService languageService)
        {
            this.languageService = languageService;

            AddImagesCommand = new LambdaCommand(OnAddImagesCommandExecute);
            AddCommand = new LambdaCommand(OnAddCommandExecute, CanAddCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecute);
        }

        private IEnumerable<string> imagePaths;
        public LambdaCommand AddImagesCommand { get; set; }
        public LambdaCommand AddCommand { get; set; }
        private bool CanAddCommandExecute(object arg)
        {
            throw new NotImplementedException();
        }
        private void OnAddCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        public LambdaCommand CancelCommand { get; set; }
        private void OnCancelCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }


        private void OnAddImagesCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private LanguageService languageService;
        public LanguageService LanguageService
        {
            get => languageService;
            set => Set(ref languageService, ref value);
        }

    }
}
