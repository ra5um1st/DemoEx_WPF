using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories;
using DemoEx.Domain.Repositories.Base;
using DemoEx.WPF.Commands;
using DemoEx.WPF.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace DemoEx.WPF.ViewModels
{
    class AddServiceViewModel : ViewModel
    {
        public LambdaCommand AddImagesCommand { get; }
        public string[] ImagePaths { get; private set; }
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
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (fileDialog.ShowDialog() != true)
            {
                return;
            }
            ImagePaths = fileDialog.FileNames;
            ImagePaths.ToList().ForEach(imagePath => {
                File.Copy(imagePath, "..\\..\\..\\Resources\\Услуги школы\\");
                string imageName = Path.GetFileName(imagePath);
                imagePath = Path.Combine(imagePath, imageName);
            });
        }
    }
}
