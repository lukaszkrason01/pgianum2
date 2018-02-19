using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    /// 
   
    public class Graph
    {
        public string Day { get; set; }
        public double Value { get; set; }
    }
    public sealed partial class PageStats : Page
    {
        KolekcjaWpis myKole;
        public PageStats()
        {
            myKole = new KolekcjaWpis();
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }

        private async void LoadChartContents()
        {
            await Task.Run(() => myKole.Wczytaj());

            myKole.MakeElse();

            List<Graph> myGraph = new List<Graph>();
            for(int i = 0; i < myKole.licznik.Count; ++i)
            {
                double wartosc = myKole.licznik[i].taryfapierwsza;
                DateTime data = myKole.licznik[i].dzien;
                String dataform = data.Day + "/" + data.Month + "/" + data.Year;
                myGraph.Add(new Graph() { Day = dataform, Value = wartosc });
            }
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = myGraph;

            List<Graph> myGraph2 = new List<Graph>();
            for (int i = 0; i < myKole.licznik.Count; ++i)
            {
                double wartosc = myKole.licznik[i].taryfadruga;
                DateTime data = myKole.licznik[i].dzien;
                String dataform = data.Day + "/" + data.Month + "/" + data.Year;
                myGraph2.Add(new Graph() { Day = dataform, Value = wartosc });
            }
            (BarChart.Series[0] as BarSeries).ItemsSource = myGraph2;

        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }


}
