using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalutyLAB2.Klasy
{
    public class PozycjaTabA
    {
        public string NazwaWaluty { get; set; }

        public string Przelicznik { get; set; }

        public string KodWaluty { get; set; }

        public string KursSredni { get; set; }

        public decimal Kurs { get
            {
                decimal kurs;
                string s = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                decimal.TryParse(KursSredni.Replace(",", s), out kurs);

                return kurs;
            }
                }
    }
}
