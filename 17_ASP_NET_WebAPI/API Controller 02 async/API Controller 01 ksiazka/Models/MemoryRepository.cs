using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Controller_01_ksiazka.Models
{
    public class MemoryRepository : IRepository
    {
        private List<Reservation> items;

        public IList<Reservation> Reservations => items;

        public MemoryRepository()
        {
            items = new List<Reservation> {
            new Reservation { ReservationId = 1, ClientName = "Alicja", Location = "Sala posiedzeń" },
            new Reservation { ReservationId = 2, ClientName = "Bartek", Location = "Sala wykładowa" },
            new Reservation { ReservationId = 3, ClientName = "Janek", Location = "Sala konferencyjna nr 1" }
        };
        }
        public Reservation GetReservation(int id)
        {
            var dupa = items.AsQueryable();
            Reservation returnMe = dupa.SingleOrDefault(i => i.ReservationId == id);
            return returnMe;
        }
        public Reservation DeleteReservation(int id)
        {
            Reservation returnMe = items.SingleOrDefault(i => i.ReservationId == id);
            items.Remove(returnMe);
            return null;
        }

        public Reservation AddReservation(Reservation reservation)
        {
            if (reservation.ReservationId == 0)
            {
                Reservation lastOne = items.LastOrDefault();
                Reservation newReservation = new Reservation
                {
                    ReservationId = lastOne.ReservationId+1,
                    ClientName = reservation.ClientName,
                    Location = reservation.Location
                };
                items.Add(newReservation);
                return newReservation;
            }
            else
            {
                Reservation returnMe = items.SingleOrDefault(i => i.ReservationId == reservation.ReservationId);
                returnMe.Location = reservation.Location;
                returnMe.ClientName = reservation.ClientName;
                return returnMe;
            }
        }

        public Reservation UpdateReservation(Reservation reservation)
        {
            Reservation returnMe = items.SingleOrDefault(i => i.ReservationId == reservation.ReservationId);
            AddReservation(returnMe);
            return returnMe;
        }
    }
}
