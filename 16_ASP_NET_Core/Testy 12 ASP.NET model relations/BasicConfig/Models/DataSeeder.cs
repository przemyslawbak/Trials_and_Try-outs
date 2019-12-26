using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public static class DataSeeder
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var clients = new Client[]
            {
                new Client { Name = "Pszemek Boniek" },
                new Client { Name = "Alicja Zet" },
                new Client { Name = "John Wayne" },
                new Client { Name = "Czlowiek Warga" },
                new Client { Name = "Marta z Pekaes" },
                new Client { Name = "Adolf Stalin" },
                new Client { Name = "Jezus Ghandi" }
            };

            foreach (Client s in clients)
            {
                context.Clients.Add(s);
            }
            context.SaveChanges();

            var products = new Product[]
            {
                new Product { Name = "Gompka", Description = "Do szorowania", Price = 1000},
                new Product { Name = "Szampon", Description = "Do wlasow", Price = 2000},
                new Product { Name = "Mydlo", Description = "Do schylania sie", Price = 666},
                new Product { Name = "Szczoteczka do zebow", Description = "Do czyszczenia wc", Price = 111},
                new Product { Name = "Pasta do zebow", Description = "Do niczego", Price = 777},
                new Product { Name = "Dezodorant", Description = "Zamiast mydla", Price = 123},
                new Product { Name = "Maszynka do golenia", Description = "Do golenia", Price = 4}
            };

            foreach (Product i in products)
            {
                context.Products.Add(i);
            }
            context.SaveChanges();

        }
    }
}
