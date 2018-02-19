using System;
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

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace App2
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            content.Navigate(typeof(PageForecast));
            header.Text = "Witaj!";
        }

        private void MenuButttonClick(object sender, RoutedEventArgs e)
        {
            var rb = sender as Button;
            if(rb !=null)
            {
                switch (rb.Tag.ToString())
                {
                    case "Stats":
                        content.Navigate(typeof(PageStats));
                        header.Text = "Zuużycie";
                        break;
                    case "Add":
                        content.Navigate(typeof (PageAdd));
                        header.Text = "Spisz prąd";
                        break;
                    case "See":
                        header.Text = "Prognoza";
                        content.Navigate(typeof(PageForecast));
                        break;
                    case "Add2":
                        header.Text = "Dodaj doładowanie";
                        content.Navigate(typeof(PageAdd2));
                        break;
                    case "Buy":
                        header.Text = "Kup prąd";
                        content.Navigate(typeof(PageBuy));
                        break;
                    case "Options":
                        header.Text = "Opcje";
                        content.Navigate(typeof(PageOptions));
                        break;
                    default: break;
                }
            }
            mySplitView.IsPaneOpen = false;
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }
    }
}
