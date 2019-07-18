using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Controller_01_ksiazka.Models
{
    public interface IRepository
    {
        IList<Reservation> Reservations { get; }
        Reservation GetReservation(int id);
        Reservation AddReservation(Reservation reservation);
        Reservation UpdateReservation(Reservation reservation);
        Reservation DeleteReservation(int id);
    }
}
