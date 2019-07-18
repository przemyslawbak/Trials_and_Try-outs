using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podręcznik_MVVM_04_ZadaniaWPF.Model
{
    public enum PriorytetZadania : byte { MniejWażne, Ważne, Krytyczne }; //enumerable, definicja stałych
    public class Zadanie //klasa
    {   //private set dla właściwości zmienianych tylko z modelu
        //właściwości rekordów
        public string Opis { get; private set; } //właściwość
        public DateTime DataUtworzenia { get; private set; } //właściwość
        public DateTime PlanowanyTerminRealizacji { get; private set; } //właściwość
        public PriorytetZadania Priorytet { get; private set; } //właściwość
        //czy zrealizowane - zmieniane spoza modelu
        public bool CzyZrealizowane { get; set; } //właściwość

        //przy teście ciągnie z tego:
        public Zadanie(string opis, DateTime dataUtworzenia, DateTime planowanyTerminRealizacji, PriorytetZadania priorytetZadania,
        bool czyZrealizowane) //konstruktor, przypisanie zmiennych właściwościom
        {
            this.Opis = opis;
            this.DataUtworzenia = dataUtworzenia;
            this.PlanowanyTerminRealizacji = planowanyTerminRealizacji;
            this.Priorytet = priorytetZadania;
            this.CzyZrealizowane = czyZrealizowane;
        }

        //nadpisana metoda ToString tak, by zwracała łańcuch z informacją o zadaniu, wykorzystana w pliku PlikTXT.cs:
        public override string ToString() //metoda
        {
            return Opis + ", priorytet: " + OpisPriorytetu(Priorytet) +
            ", data utworzenia: " + DataUtworzenia +
            ", planowany termin realizacji: " + PlanowanyTerminRealizacji.ToString() +
            ", " + (CzyZrealizowane ? "zrealizowane" : "niezrealizowane");
        }

        //przygotowanie łańcucha podstawie jednej z trzech wartości priorytetu
        public static string OpisPriorytetu(PriorytetZadania priorytet) //metoda, korzysta konwerter
        {
            switch (priorytet)
            {
                case PriorytetZadania.MniejWażne:
                    return "mniej ważne";
                case PriorytetZadania.Ważne:
                    return "ważne";
                case PriorytetZadania.Krytyczne:
                    return "krytyczne";
                default:
                    throw new Exception("Nierozpoznany priorytet zadania");
            }
        }

        //przygotowanie wartości priorytetu na podstawie łańcucha
        public static PriorytetZadania ParsujOpisPriorytetu(string opisPriorytetu) //metoda
        {
            switch (opisPriorytetu)
            {
                case "mniej ważne":
                    return PriorytetZadania.MniejWażne;
                case "ważne":
                    return PriorytetZadania.Ważne;
                case "krytyczne":
                    return PriorytetZadania.Krytyczne;
                default:
                    throw new Exception("Nierozpoznany opis priorytetu zadania");
            }
        }
    }
}
