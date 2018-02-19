using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class PageAdd2 : Page
    {
        KolekcjaWpis myKole;
        public PageAdd2()
        {
            this.InitializeComponent();
            myKole = new KolekcjaWpis();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            date.Date = DateTime.Today;
            i.Text = "";
            ii.Text = "";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => myKole.Wczytaj());
            myKole.MakeElse();
            try
            {
                var dates = date.Date;
                DateTime time = dates.Value.DateTime.Date;

                double a = 0;
                double b = 0;

                a = Convert.ToDouble(i.Text);
                b = Convert.ToDouble(ii.Text);

                if (!myKole.check2(time))
                {
                    if (DateTime.Compare(DateTime.Today, time) >= 0)
                    {
                        Wpis nowyWpis = new Wpis(a, b, time, 1);
                        nowyWpis.ZapiszWpis();

                        MessageDialog mesege = new MessageDialog("Dodano");
                        await mesege.ShowAsync();

                        date.Date = DateTime.Today;
                        i.Text = "";
                        ii.Text = "";
                    }
                    else
                    {
                        MessageDialog messege = new MessageDialog("Nie możesz dodać wpisu z datą późniejszą niż dzisiaj.");
                        await messege.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog messege = new MessageDialog("Już istnieje wpis z tego dnia.");
                    await messege.ShowAsync();
                }
            }
            catch
            {
                MessageDialog me = new MessageDialog("Błąd wprowadzania");
                await me.ShowAsync();
            }
        }
    }
}
