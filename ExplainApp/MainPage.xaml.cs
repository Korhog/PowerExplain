using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace ExplainApp
{
    using Explainer.Core;
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnExplain(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            int asciiLine = 13;
            char line = (char)asciiLine;
            string[] lines = data.Text.Split(line);

            var parser = new Explainer.Core.ExplainParser();
            var tree = parser.Parse(lines);

            var collection = new ObservableCollection<TreeNode>();
            foreach (var node in tree.Nodes.Where(x => !string.IsNullOrEmpty(x.ToString())))
            { 
                collection.Add(new TreeNode(node));
            }



            Frame.Navigate(typeof(ExplainPage), collection);
            btn.IsEnabled = true;
        }
    }
}
