using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class PageOptions : Page
    {
        Settings mySettings;

        public PageOptions()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            mySettings = new Settings();
            InitValues();

        }

        private void InitValues()
        {
            moc.Text = Convert.ToString(mySettings.mocLicznika);
            oplata.IsChecked = mySettings.oplataHandlowa;

            cOplataH.Text = Convert.ToString(mySettings.ceny[0]);
            cDysS.Text = Convert.ToString(mySettings.ceny[1]);
            cDysP.Text = Convert.ToString(mySettings.ceny[2]);
            coplaA.Text = Convert.ToString(mySettings.ceny[3]);
            cOZE.Text = Convert.ToString(mySettings.ceny[4]);
            cIc.Text = Convert.ToString(mySettings.ceny[5]);
            cIp.Text = Convert.ToString(mySettings.ceny[6]);
            cIIc.Text = Convert.ToString(mySettings.ceny[7]);
            cIIp.Text = Convert.ToString(mySettings.ceny[8]);

            if (oplata.IsChecked.HasValue && !oplata.IsChecked.Value)
                toHide.Visibility = Visibility.Collapsed;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Task.Run(() => mySettings.WczytajUstawienia());
            InitValues();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                Convert.ToUInt32(moc.Text);
                Convert.ToDouble(cOplataH.Text);
                Convert.ToDouble(cDysS.Text);
                Convert.ToDouble(cDysP.Text);
                Convert.ToDouble(coplaA.Text);
                Convert.ToDouble(cOZE.Text);
                Convert.ToDouble(cIc.Text);
                Convert.ToDouble(cIp.Text);
                Convert.ToDouble(cIIc.Text);
                Convert.ToDouble(cIIc.Text);

                mySettings.mocLicznika = Convert.ToUInt32(moc.Text);
                mySettings.oplataHandlowa = Convert.ToBoolean(oplata.IsChecked);

                mySettings.ceny[0] = Convert.ToDouble(cOplataH.Text);
                mySettings.ceny[1] = Convert.ToDouble(cDysS.Text);
                mySettings.ceny[2] = Convert.ToDouble(cDysP.Text);
                mySettings.ceny[3] = Convert.ToDouble(coplaA.Text);
                mySettings.ceny[4] = Convert.ToDouble(cOZE.Text);
                mySettings.ceny[5] = Convert.ToDouble(cIc.Text);
                mySettings.ceny[6] = Convert.ToDouble(cIp.Text);
                mySettings.ceny[7] = Convert.ToDouble(cIIc.Text);
                mySettings.ceny[8] = Convert.ToDouble(cIIc.Text);

                mySettings.ZapiszUstawienia();
            }
            catch(Exception)
            {
                MessageDialog me = new MessageDialog("Niektóre dane nie były liczbami.\n\r Opcje nie zostały zapisane!");
                await me.ShowAsync();
            }
            base.OnNavigatingFrom(e);
        }

        private void oplata_Click(object sender, RoutedEventArgs e)
        {
            if (oplata.IsChecked.HasValue && oplata.IsChecked.Value)
            {
                oplata.IsChecked = true;
                toHide.Visibility = Visibility.Visible;
            }
            else
            {
                oplata.IsChecked = false;
                toHide.Visibility = Visibility.Collapsed;
            }
        }
    }
}
