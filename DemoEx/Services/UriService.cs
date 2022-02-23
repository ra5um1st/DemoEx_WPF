using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DemoEx.WPF.Services
{
    static class UriService
    {
        public static Uri CreateCurrentAssemblyUri(string recourseFolderPath)
        {
            return new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{recourseFolderPath}");
        }
    }
}
