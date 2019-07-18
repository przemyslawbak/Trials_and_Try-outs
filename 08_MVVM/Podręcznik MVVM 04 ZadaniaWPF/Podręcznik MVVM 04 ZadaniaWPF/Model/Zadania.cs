using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Podręcznik_MVVM_04_ZadaniaWPF.Model
{
    public class Zadania : IEnumerable<Zadanie> //klasa
    {
        private List<Zadanie> listaZadań = new List<Zadanie>(); //nowa instancja listy


        public void DodajZadanie(Zadanie zadanie)
        {
            listaZadań.Add(zadanie); //dodanie zadania do listy - metoda
        }
        public bool UsuńZadanie(Zadanie zadanie)
        {
            return listaZadań.Remove(zadanie); //usunięcie zadania z listy - metoda
        }
        public int LiczbaZadań
        {
            get
            {
                return listaZadań.Count;
            }
        }
        public Zadanie this[int indeks]
        {
            get
            {
                return listaZadań[indeks];
            }
        }
        public IEnumerator<Zadanie> GetEnumerator() //masło maślane, implementacja IEnumerable
        {
            return listaZadań.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() //masło maślane, implementacja IEnumerable
        {
            return GetEnumerator();
        }



        private Comparison<Zadanie> porównywaniePriorytetów = new Comparison<Zadanie>( //porównywacz
            (Zadanie zadanie1, Zadanie zadanie2) =>
            {
                int wynik = -zadanie1.Priorytet.CompareTo(zadanie2.Priorytet);
                if (wynik == 0) wynik = zadanie1.PlanowanyTerminRealizacji.CompareTo(zadanie2.PlanowanyTerminRealizacji);
                return wynik;
            });
        private Comparison<Zadanie> porównywaniePlanowanychTerminówRealizacji =
        new Comparison<Zadanie>(
        (Zadanie zadanie1, Zadanie zadanie2) =>
        {
            int wynik = zadanie1.PlanowanyTerminRealizacji.CompareTo(zadanie2.PlanowanyTerminRealizacji);
            if (wynik == 0) wynik = -zadanie1.Priorytet.CompareTo(zadanie2.Priorytet);
            return wynik;
        });
        public void SortujZadania(
        bool porównywaniePriorytetówCzyPlanowanychTerminówRealizacji)
        {
            if (porównywaniePriorytetówCzyPlanowanychTerminówRealizacji)
                listaZadań.Sort(porównywaniePriorytetów);
            else
                listaZadań.Sort(porównywaniePlanowanychTerminówRealizacji);
        }
    }
}
