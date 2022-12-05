using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Controller_01_ksiazka.Models
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<int, Reservation> items;
        public MemoryRepository()
        {
            items = new Dictionary<int, Reservation>();
            new List<Reservation> {
            new Reservation { ClientName = "Alicja", Location = "Sala posiedzeń" },
            new Reservation { ClientName = "Bartek", Location = "Sala wykładowa" },
            new Reservation { ClientName = "Janek", Location = "Sala konferencyjna nr 1" }
        }.ForEach(r => AddReservation(r));
        }
        public Reservation this[int id] => items.ContainsKey(id) ? items[id] : null;
        public IEnumerable<Reservation> Reservations => items.Values;
        public Reservation AddReservation(Reservation reservation)
        {
            if (reservation.ReservationId == 0)
            {
                int key = items.Count;
                while (items.ContainsKey(key)) { key++; };
                reservation.ReservationId = key;
            }
            items[reservation.ReservationId] = reservation;
            return reservation;
        }
        public void DeleteReservation(int id) => items.Remove(id);
        public Reservation UpdateReservation(Reservation reservation)
        => AddReservation(reservation);
    }
}
