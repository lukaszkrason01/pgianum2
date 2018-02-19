using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace App2
{
    class WczytajZapisz
    {
        static public async Task<String> Read(String nameF)
        {
            String toRead = "";
            try
            {
                StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF);
                toRead = await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            { }
            return toRead;
        }

        static public async void WriteSettings(String toWrite, String nameF)
        {
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                CreateFileAsync(nameF, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, toWrite);
        }

    }
    public class Settings
    {
        private const String nameF = "settings.txt";
        public uint mocLicznika;
        public bool oplataHandlowa;

        public double [] ceny;

        public Settings()
        {
            ceny = new double[9];
        }

        public async void ZapiszUstawienia()
        {
            String toWrite = "";
            toWrite += mocLicznika;
            toWrite += Environment.NewLine;
            toWrite += oplataHandlowa;
            toWrite += Environment.NewLine;

            foreach(double c in ceny)
            {
                toWrite += c;
                toWrite += Environment.NewLine;
            }

            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.
            CreateFileAsync(nameF, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, toWrite);
        }
        public async Task WczytajUstawienia()
        {
            String toRead = "";
            try
            {
                StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF);
                toRead = await FileIO.ReadTextAsync(file);
                String[] array = toRead.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                mocLicznika = Convert.ToUInt32(array[0]);
                oplataHandlowa = Convert.ToBoolean(array[1]);

                if (mocLicznika == 0) ZaladujPodstawoweCeny();
                else
                    for (int i = 2; i < array.Length; ++i)
                    {
                        ceny[i - 2] = Convert.ToDouble(array[i]);
                    }
                }
            catch (Exception)
            {
                ZaladujPodstawoweCeny();
            }
        }

        public void ZaladujPodstawoweCeny()
        {
            mocLicznika = 0;
            oplataHandlowa = false;
            ceny[0] =10.5 ;
            ceny[1] =3.36 ;
            ceny[2] =1.65 ;
            ceny[3] =0.43 ;
            ceny[4] =0.0037 ;
            ceny[5] =0.52 ;
            ceny[6] =0.22 ;
            ceny[7] =0.34 ;
            ceny[8] =0.12 ;
        }
    }

    class Wpis
    {
        private const String nameF = "wpis.txt";
        private const String nameF2 = "doladowanie.txt";
        public int coto;
        public DateTime dzien;
        public double taryfapierwsza;
        public double taryfadruga;

        public Wpis(double a, double b, DateTime date,int c)
        {
            dzien = date;
            taryfapierwsza = a;
            taryfadruga = b;
            coto = c;
        }

        public async void ZapiszWpis()
        {
            String toRead = "";
            StorageFile file;
            try
            {
                if(coto == 0)
                    file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF);
                else
                    file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF2);
                toRead = await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            { }

            toRead += dzien.ToString("yyyy.MM.dd") + '\t' + 
                Convert.ToString(taryfapierwsza) +'\t'+ 
                Convert.ToString(taryfadruga)+Environment.NewLine;

            if (coto == 0)
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                    CreateFileAsync(nameF, CreationCollisionOption.OpenIfExists);
            else 
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                    CreateFileAsync(nameF2, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, toRead);
        }
    }
    class KolekcjaWpis
    {
        private const String nameF = "wpis.txt";
        private const String nameF2 = "doladowanie.txt";
        
        public List<Wpis> licznik;
        public List<Wpis> doladowania;

        public List<double> zuuzycie1;
        public List<double> zuuzycie2;

        public KolekcjaWpis()
        {
            licznik = new List<Wpis>();
            doladowania = new List<Wpis>();
            zuuzycie1 = new List<double>();
            zuuzycie2 = new List<double>();
        }

        public async Task Wczytaj()
        {
            String toRead = "";
            StorageFile file;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF);
                toRead = await FileIO.ReadTextAsync(file);

                String[] array = toRead.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (String day in array)
                {
                    String[] array2 = day.Split('\t');
                    licznik.Add(new Wpis(Convert.ToDouble(array2[1]), Convert.ToDouble(array2[2]),
                        Convert.ToDateTime(array2[0]), 0));
                }

                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(nameF2);
                toRead = await FileIO.ReadTextAsync(file);

                array = toRead.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (String day in array)
                {
                    String[] array2 = day.Split('\t');
                    doladowania.Add(new Wpis(Convert.ToDouble(array2[1]), Convert.ToDouble(array2[2]),
                        Convert.ToDateTime(array2[0]), 1));
                }
            }
            catch { }
        }
        public async Task ZapiszAsync()
        {
            String toWrite = "";
            StorageFile file;
            int max = licznik.Count;
            int i = max - 7;
            if (i < 0) i = 0;
            for ( ; i < max; ++i)
            {
                toWrite += licznik[i].dzien.ToString("yyyy.MM.dd") + '\t' +
                    Convert.ToString(licznik[i].taryfapierwsza) + '\t' +
                    Convert.ToString(licznik[i].taryfadruga) + Environment.NewLine;
            }

            file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                CreateFileAsync(nameF, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, toWrite);

            toWrite = "";
            max = doladowania.Count;
            i = max - 7;
            if (i < 0) i = 0;
            for (; i < doladowania.Count; ++i)
            {
                toWrite += doladowania[i].dzien.ToString("yyyy.MM.dd") + '\t' +
                    Convert.ToString(doladowania[i].taryfapierwsza) + '\t' +
                    Convert.ToString(doladowania[i].taryfadruga) + Environment.NewLine;
            }

            file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                CreateFileAsync(nameF2, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, toWrite);
        }

        public void MakeElse()
        {
            List<Wpis> SortedList = licznik.OrderBy(o => o.dzien).ToList();
            licznik = SortedList;

            SortedList = doladowania.OrderBy(o => o.dzien).ToList();
            doladowania = SortedList;

            for (int i = 0; i < licznik.Count - 1; ++i)
            {
                if ((licznik[i+1].dzien - licznik[i].dzien).TotalDays > 1) continue;

                zuuzycie1.Add(Porownaj(licznik[i].taryfapierwsza , licznik[i + 1].taryfapierwsza, licznik[i+1].dzien,1));
                zuuzycie2.Add(Porownaj(licznik[i].taryfadruga , licznik[i + 1].taryfadruga, licznik[i+1].dzien,2));
            }
            if(zuuzycie1.Count == 0)
            {
                try
                {
                    double temp;
                    temp = licznik[0].taryfapierwsza - licznik[licznik.Count - 1].taryfapierwsza;

                    for (int i = 0; i < doladowania.Count; ++i)
                    {
                        temp += doladowania[i].taryfapierwsza;
                    }
                    temp /= (licznik[licznik.Count - 1].dzien - licznik[0].dzien).TotalDays;
                    zuuzycie1.Add(temp);

                    temp = licznik[0].taryfadruga - licznik[licznik.Count - 1].taryfadruga;

                    for (int i = 0; i < doladowania.Count; ++i)
                    {
                        temp += doladowania[i].taryfadruga;
                    }
                    temp /= (licznik[licznik.Count - 1].dzien - licznik[0].dzien).TotalDays;
                    zuuzycie2.Add(temp);
                }
                catch { }
                
            }
        }

        public double Porownaj (double a, double b, DateTime d,int l)
        {
            double result=a-b;
            if (result < 0)
            {
             result = zuuzycie1.Sum() / zuuzycie1.Count;
                for (int i = 0; i < doladowania.Count; ++i) ;

            }
            if (result < 0) result = 0;

            return result;
        }
        
        public bool check(DateTime da)
        {
            for(int i=0;i<licznik.Count;++i)
            {
                if (DateTime.Compare(licznik[i].dzien,da)  == 0)
                    return true;
            }
            return false;
        }
        public bool check2(DateTime da)
        {
            for (int i = 0; i < doladowania.Count; ++i)
            {
                if (DateTime.Compare(doladowania[i].dzien, da) == 0)
                    return true;
            }
            return false;
        }
    }
}
