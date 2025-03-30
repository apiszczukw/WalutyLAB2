using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using WalutyLAB2.Klasy;
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

namespace WalutyLAB2
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<PozycjaTabA> kursyWalut = new List<PozycjaTabA>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        void Przelicz()
        {
            if (WalutaWejscieLbx.SelectedIndex == -1 || WalutaWyjscieLbx.SelectedIndex == -1 || KwotaTbx.Text == "")
                return;
            

            var ZWaluty = WalutaWejscieLbx.SelectedItem as PozycjaTabA;
            var NaWalute = WalutaWyjscieLbx.SelectedItem as PozycjaTabA;

            decimal kwota;

            if( decimal.TryParse(KwotaTbx.Text, out kwota ))
            {
                Blad.Visibility = Visibility.Collapsed;

                decimal kwotaNaPLN = kwota * ZWaluty.Kurs;
                decimal kwotaZPLN = kwotaNaPLN / NaWalute.Kurs;

                PrzeliczonaTb.Text = string.Format("{0:f2} {1}", kwotaZPLN, NaWalute.KodWaluty);
            }
            else
            {
                Blad.Visibility = Visibility.Visible;
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var klient = new HttpClient();
            var dane = await klient.GetStringAsync(new Uri(NBPAPI.daneNBP));

            var daneXml = XDocument.Parse(dane);

            foreach (var pozycja in daneXml.Descendants("pozycja"))
            {
                var pozycjaTabA = new PozycjaTabA()
                {
                    NazwaWaluty = pozycja.Element("nazwa_waluty").Value,
                    Przelicznik = pozycja.Element("przelicznik").Value,
                    KodWaluty = pozycja.Element("kod_waluty").Value,
                    KursSredni = pozycja.Element("kurs_sredni").Value
                };

                kursyWalut.Add(pozycjaTabA);
            }

            var pln = new PozycjaTabA() { NazwaWaluty = "Złoty polski", KodWaluty = "PLN", Przelicznik = "1", KursSredni = "1,0000" };

            kursyWalut.Insert(0, pln);

            // ustawiamy listę na naszych listboxach
            WalutaWejscieLbx.ItemsSource = kursyWalut;
            WalutaWyjscieLbx.ItemsSource = kursyWalut;
        }

        private void KwotaTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            Przelicz();
        }


        private void WalutaLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Przelicz();
        }
    }
}
