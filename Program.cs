using System;
using System.Collections.Generic;

namespace KontaBankowe
{
    public abstract class KontoBankowe
    {
        public string NumerKonta { get; }
        public string Wlasciciel { get; }
        public decimal Saldo { get; protected set; }
        protected KontoBankowe(string numerKonta, string wlasciciel)
        {
            NumerKonta = numerKonta;
            Wlasciciel = wlasciciel;
            Saldo = 0m;
        }
        public void Wplac(decimal kwota)
        {
            if (kwota <= 0)
            {
                Console.WriteLine("Kwota musi być dodatnia.");
                return;
            }

            Saldo += kwota;
            Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
        }
        public virtual void Wyplac(decimal kwota)
        {
            if (kwota <= 0)
            {
                Console.WriteLine("Kwota musi być dodatnia.");
                return;
            }

            if (kwota > Saldo)
            {
                Console.WriteLine("Brak środków na koncie.");
                return;
            }

            Saldo -= kwota;
            Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
        }
        public abstract decimal ObliczOprocentowanie();

        public virtual void WyswietlInformacje()
        {
            Console.WriteLine($"Konto: {NumerKonta}, Właściciel: {Wlasciciel}, Saldo: {Saldo} zł");
        }
    }

    public class KontoOszczednosciowe : KontoBankowe
    {
        public KontoOszczednosciowe(string numer, string wlasciciel)
            : base(numer, wlasciciel) { }

        public override decimal ObliczOprocentowanie()
        {
            return Saldo * 0.05m;
        }

        public override void WyswietlInformacje()
        {
            Console.WriteLine($"Oszczędnościowe {NumerKonta}, {Wlasciciel}, Saldo: {Saldo} zł");
        }
    }
    public class KontoStudenckie : KontoBankowe
    {
        public KontoStudenckie(string numer, string wlasciciel)
            : base(numer, wlasciciel) { }

        public override decimal ObliczOprocentowanie()
        {
            return Saldo * 0.02m;
        }

        public override void WyswietlInformacje()
        {
            Console.WriteLine($"Studenckie {NumerKonta}, {Wlasciciel}, Saldo: {Saldo} zł");
        }
    }

    public class KontoFirmowe : KontoBankowe
    {
        public KontoFirmowe(string numer, string wlasciciel)
            : base(numer, wlasciciel) { }

        public override decimal ObliczOprocentowanie()
        {
            return 0m;
        }

        public override void Wyplac(decimal kwota)
        {
            decimal prowizja = 2m;
            decimal suma = kwota + prowizja;

            if (kwota <= 0)
            {
                Console.WriteLine("Kwota musi być dodatnia.");
                return;
            }

            if (suma > Saldo)
            {
                Console.WriteLine("Brak środków na koncie (kwota + prowizja).");
                return;
            }

            Saldo -= suma;
            Console.WriteLine(
                $"Wypłacono {kwota} zł + prowizja {prowizja} zł. " +
                $"Łącznie pobrano {suma} zł. Nowe saldo: {Saldo} zł."
            );
        }

        public override void WyswietlInformacje()
        {
            Console.WriteLine($"Firmowe {NumerKonta}, {Wlasciciel}, Saldo: {Saldo} zł");
        }
    }

    class Program
    {
        static void Main()
        {
            List<KontoBankowe> konta = new List<KontoBankowe>()
            {
                new KontoOszczednosciowe("1111", "Jan Kowalski"),
                new KontoStudenckie("2222", "Anna Nowak"),
                new KontoFirmowe("3333", "Firma wiertareczki")
            };
            foreach (var konto in konta)
            {
                konto.WyswietlInformacje();
                konto.Wplac(1000m);

                decimal odsetki = konto.ObliczOprocentowanie();
                Console.WriteLine($"Oprocentowanie: {odsetki} zł\n");
            }

            Console.WriteLine(" Wypłata z konta firmowego");
            KontoFirmowe firmowe = konta[2] as KontoFirmowe;

            firmowe?.Wyplac(200m);
        }
    }
}