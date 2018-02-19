using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
    public sealed partial class PageForecast : Page
    {
        KolekcjaWpis myKole;
        public PageForecast()
        {
            this.InitializeComponent();
            myKole = new KolekcjaWpis();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Task.Run(() => myKole.Wczytaj());

            myKole.MakeElse();
            if (myKole.licznik.Count != 0)
            {


                String naw = myKole.licznik[myKole.licznik.Count - 1].dzien.DayOfWeek.ToString() + " ";
                naw += Convert.ToString(myKole.licznik[myKole.licznik.Count - 1].dzien.Day) + "/";
                naw += Convert.ToString(myKole.licznik[myKole.licznik.Count - 1].dzien.Month) + "/";
                naw += Convert.ToString(myKole.licznik[myKole.licznik.Count - 1].dzien.Year);
                dzien.Text = Convert.ToString(naw);

                double licz = myKole.licznik[myKole.licznik.Count - 1].taryfapierwsza;

                if (myKole.licznik[myKole.licznik.Count - 1].dzien ==
                    myKole.doladowania[myKole.doladowania.Count - 1].dzien)
                    licz = myKole.licznik[myKole.licznik.Count - 1].taryfapierwsza + myKole.doladowania[myKole.doladowania.Count - 1].taryfapierwsza;

                ow1.Text = Convert.ToString(licz) + " kW";

                double licz2 = myKole.licznik[myKole.licznik.Count - 1].taryfadruga;
                if (myKole.licznik[myKole.licznik.Count - 1].dzien ==
                    myKole.doladowania[myKole.doladowania.Count - 1].dzien)
                    licz2 = myKole.licznik[myKole.licznik.Count - 1].taryfadruga + myKole.doladowania[myKole.doladowania.Count - 1].taryfadruga;

                ow2.Text = Convert.ToString(licz2) + " kW";

                if (myKole.licznik.Count > 1)
                {
                    double daystotoday = (DateTime.Today - myKole.licznik[myKole.licznik.Count - 1].dzien).TotalDays;
                    if (daystotoday < 0) daystotoday = 0;
                    
                    double max1 = myKole.zuuzycie1.Max();
                    double averige1 = myKole.zuuzycie1.Sum() / myKole.zuuzycie1.Count;

                    double estym1 = (max1 + 4 * averige1) / 5;

                    double end1 = (licz) / (estym1);

                    wjn1.Text = end1.ToString("0.00") + " dni";
                    if (end1 > 7) wjn1.Foreground = new SolidColorBrush(Colors.Black);
                    if (end1 < 7) wjn1.Foreground = new SolidColorBrush(Colors.Orange);
                    if (end1 < 3) wjn1.Foreground = new SolidColorBrush(Colors.Red);

                    double max2 = myKole.zuuzycie2.Max();
                    double averige2 = myKole.zuuzycie2.Sum() / myKole.zuuzycie2.Count;

                    double estym2 = (max2 + 4 * averige2) / 5;
                    double end2 = licz2 / (estym2);

                    wjn2.Text = end2.ToString("0.00") + " dni";
                    if (end2 > 7) wjn2.Foreground = new SolidColorBrush(Colors.Black);
                    if (end2 < 7) wjn2.Foreground = new SolidColorBrush(Colors.Orange);
                    if (end2 < 3) wjn2.Foreground = new SolidColorBrush(Colors.Red);

                    double dzis1 = licz - (daystotoday+1) * estym1;
                    if (dzis1 < 0) dzis1 = 0;

                    double dzis2 = licz2- (daystotoday+1) * estym2;
                    if (dzis2 < 0) dzis2 = 0;

                    db1.Text = (dzis1).ToString("0.00") + " kW";
                    db2.Text = (dzis2).ToString("0.00") + " kW";

                    dzis1 -= estym1;
                    dzis2 -= estym2;
                    if (dzis1 < 0) dzis1 = 0;
                    if (dzis2 < 0) dzis2 = 0;
                    jb1.Text = (dzis1).ToString("0.00") + " kW";
                    jb2.Text = (dzis2).ToString("0.00") + " kW";
                }
            }
            await myKole.ZapiszAsync();
            
        }
    }
}
