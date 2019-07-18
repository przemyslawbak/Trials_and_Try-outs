using API_Controller_01_ksiazka.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Controller_01_ksiazka.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private IRepository repository;
        public ReservationController(IRepository repo) => repository = repo;
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var returnMe = await Task.Run(() => repository.Reservations);
            return Ok(returnMe);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var returnMe = await Task.Run(() => repository.GetReservation(id));
            return Ok(returnMe);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Reservation res)
        {
            var returnMe = await Task.Run(() => repository.AddReservation(new Reservation
            {
                ReservationId = 0,
                ClientName = res.ClientName,
                Location = res.Location
            }));
            return Ok(returnMe);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Reservation res)
        {
            var returnMe = await Task.Run(() => repository.UpdateReservation(res));
            return Ok(returnMe);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var returnMe = await Task.Run(() => repository.DeleteReservation(id));
            return Ok(returnMe);
        }
    }
}
