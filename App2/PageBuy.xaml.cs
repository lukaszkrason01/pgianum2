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
    public sealed partial class PageBuy : Page
    {
        KolekcjaWpis myKole;
        Settings settings;

        bool past;
        public PageBuy()
        {
            this.InitializeComponent();
            myKole = new KolekcjaWpis();
            settings = new Settings();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            past = true;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (past)
            {
                past = false;
                await Task.Run(() => myKole.Wczytaj());
                myKole.MakeElse();

                if (myKole.licznik.Count > 1)
                {
                    double oblt1, oblt2;
                    oblt1 = oblt2 = 0;
                    double daystotoday = (DateTime.Today - myKole.licznik[myKole.licznik.Count - 1].dzien).TotalDays;

                    double max = myKole.zuuzycie1.Max();
                    double averige = myKole.zuuzycie1.Sum() / myKole.zuuzycie1.Count;

                    double estym = (max + 4 * averige) / 5;

                    oblt1 = myKole.licznik[myKole.licznik.Count - 1].taryfapierwsza - estym * daystotoday;

                    t1.Text = oblt1.ToString("0.00");

                    double max2 = myKole.zuuzycie1.Max();
                    double averige2 = myKole.zuuzycie2.Sum() / myKole.zuuzycie2.Count;

                    double estym2 = (max2 + 4 * averige2) / 5;

                    oblt2 = myKole.licznik[myKole.licznik.Count - 1].taryfadruga - estym2 * daystotoday;

                    t2.Text = oblt2.ToString("0.00");
                }
                else
                {
                    MessageDialog md = new MessageDialog("Zbyt mało danych. \nObliczenia się nie powiodły");
                    await md.ShowAsync();
                }
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => settings.WczytajUstawienia());
            
            if (settings.mocLicznika == 0)
            {
                MessageDialog me = new MessageDialog("Najpierw wpisz moc Licznika");
                await me.ShowAsync();
            }
            else
            {
                await Task.Run(() => myKole.Wczytaj());
                myKole.MakeElse();
                if (myKole.licznik.Count < 2)
                {
                    MessageDialog me = new MessageDialog("Zbyt mało danych. Wpisuj cześciej dane z licznika");
                    await me.ShowAsync();
                }
                else
                {
                    try
                    {
                        double ilepradu1 = Convert.ToDouble(t1.Text);
                        double ilepradu2 = Convert.ToDouble(t2.Text);
                        double iledni = Convert.ToDouble(dni.Text);

                        double max = myKole.zuuzycie1.Max();
                        double averige = myKole.zuuzycie1.Sum() / myKole.zuuzycie1.Count;

                        double estym = (max + 4 * averige) / 5;
                        double max2 = myKole.zuuzycie1.Max();
                        double averige2 = myKole.zuuzycie2.Sum() / myKole.zuuzycie2.Count;

                        double estym2 = (max2 + 4 * averige2) / 5;

                        double doZakupu1 = estym * iledni - ilepradu1;
                        double doZakupu2 = estym2 * iledni - ilepradu2;

                        double oplataOZE = doZakupu1 + doZakupu2;

                        oplataOZE *= settings.ceny[4];
                        oplataOZE *= 1.23;

                        doZakupu1 *= (settings.ceny[5] + settings.ceny[6]);
                        doZakupu1 *= 1.23;

                        doZakupu1 += oplataOZE;

                        doZakupu2 *= (settings.ceny[7] + settings.ceny[8]);
                        doZakupu2 *= 1.23;

                        if (Oplaty.IsChecked.HasValue && Oplaty.IsChecked.Value)
                        {
                            double kosztoplat = 0;
                            if (settings.oplataHandlowa) kosztoplat += settings.ceny[0];
                            kosztoplat += settings.ceny[1] * settings.mocLicznika;
                            kosztoplat += settings.ceny[2] * settings.mocLicznika;
                            kosztoplat += settings.ceny[3];
                            kosztoplat *= 1.23;
                            doZakupu1 += kosztoplat;
                        }

                        if (doZakupu1 < 0) doZakupu1 = 0;
                        if (doZakupu2 < 0) doZakupu2 = 0;

                        MessageDialog me = new MessageDialog("Doładuj za:\nTaryfa pierwsza: " + doZakupu1.ToString("0.00")
                            + "zł\nTaryfa Druga: " + doZakupu2.ToString("0.00") + "zł");
                        await me.ShowAsync();

                    }
                    catch
                    {
                        MessageDialog me = new MessageDialog("Wpisz dane");
                        await me.ShowAsync();
                    }
                }
            }
        }
    }
}
