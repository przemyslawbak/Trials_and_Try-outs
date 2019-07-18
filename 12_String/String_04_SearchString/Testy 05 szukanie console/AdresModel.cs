using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_05_szukanie_console
{
    public class AdresModel
    {
        public string AdresMailowy { get; set; }

        public bool AktywnyEmail { get; set; }
        public bool ZaznaczenieEmail { get; set; }

        public AdresModel(string adresEmail, bool aktywnyEmail, bool zaznaczenieEmail)
        {
            AdresMailowy = adresEmail;
            AktywnyEmail = aktywnyEmail;
            ZaznaczenieEmail = zaznaczenieEmail;
        }
    }

    public class AdresyEmail : IEnumerable<AdresModel>
    {
        public IEnumerator<AdresModel> GetEnumerator()
        {
            return listaAdresow.GetEnumerator();
        }


        private List<AdresModel> listaAdresow = new List<AdresModel>(); //inst kolekcji

        public void DodajAdres(AdresModel adres) //dodawanie konta
        {
            listaAdresow.Add(adres);
        }

        public void UsunAdres(AdresModel adres) //usuwanie konta
        {
            listaAdresow.Remove(adres);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
