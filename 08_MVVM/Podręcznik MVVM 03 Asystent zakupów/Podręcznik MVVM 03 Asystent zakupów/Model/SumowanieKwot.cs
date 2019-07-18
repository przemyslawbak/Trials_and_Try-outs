using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podręcznik_MVVM_03_Asystent_zakupów.Model
{
    public class SumowanieKwot
    {
        public decimal Limit { get; private set; } //publiczna własność
        public decimal Suma { get; private set; } //publiczna własność
        public SumowanieKwot(decimal limit, decimal suma = 0) //konstruktor klasy
        {
            this.Limit = limit;
            this.Suma = suma;
        }
        public void Dodaj(decimal kwota) //publiczna metoda
        {
            if (!CzyKwotaJestPoprawna(kwota))
                throw new ArgumentOutOfRangeException("Kwota zbyt duża lub ujemna");
            Suma += kwota;
        }
        public bool CzyKwotaJestPoprawna(decimal kwota) //publiczna metoda
        {
            bool czyDodatnia = kwota > 0;
            bool czyPrzekroczyLimit = Suma + kwota > Limit;
            return czyDodatnia && !czyPrzekroczyLimit;
        }
    }
}
