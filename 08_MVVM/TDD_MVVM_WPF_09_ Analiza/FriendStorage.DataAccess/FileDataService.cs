using System;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.Model;
using System.IO;
using Newtonsoft.Json;

namespace FriendStorage.DataAccess
{
    public class FileDataService : IDataService
    {
        private const string StorageFile = "Friends.json"; //ścieżka pliku

        public Friend GetFriendById(int friendId) //pobierz fumfla
        {
            var friends = ReadFromFile(); //metoda odczytu wszystkich
            return friends.Single(f => f.Id == friendId); //zwróć przyjaciela o określonym ID
        }

        public void SaveFriend(Friend friend) //zapisz fumfla
        {
            if (friend.Id <= 0) //jeśli jest nowy
            {
                InsertFriend(friend); //dodaj
            }
            else //jeśli nie jest nowy
            {
                UpdateFriend(friend); //aktualizacja
            }
        }

        public void DeleteFriend(int friendId) //usuń fumfla
        {
            var friends = ReadFromFile(); //metoda odczytu wszystkich
            var existing = friends.Single(f => f.Id == friendId); //obiekt istniejącego fumfla
            friends.Remove(existing); //usuń go
            SaveToFile(friends); //zapisz do pliku wszystkich fumfli
        }

        private void UpdateFriend(Friend friend) //metoda aktualizacji przyjaciela
        {
            var friends = ReadFromFile(); //metoda odczytu wszystkich
            var existing = friends.Single(f => f.Id == friend.Id); //obiekt istniejącego fumfla
            var indexOfExisting = friends.IndexOf(existing); //index(?) przyjaciela
            friends.Insert(indexOfExisting, friend); //aktualizuje po indeksie i obiekcie
            friends.Remove(existing); //usuwa starego
            SaveToFile(friends); //zapisz do pliku wszystkich fumfli
        }

        private void InsertFriend(Friend friend) //metoda wstawienia nowego frienda
        {
            var friends = ReadFromFile(); //metoda odczytu wszystkich
            var maxFriendId = friends.Count == 0 ? 0 : friends.Max(f => f.Id); //pobiera ID ostatniego fumfla
            friend.Id = maxFriendId + 1; //ID nowego fumfla
            friends.Add(friend); // dodaj obiekt
            SaveToFile(friends); //zapisz do pliku wszystkich fumfli
        }

        public IEnumerable<LookupItem> GetAllFriends() //metoda pobrania wszystkich fumfli
        {
            return ReadFromFile()
              .Select(f => new LookupItem
              {
                  Id = f.Id,
                  DisplayMember = $"{f.FirstName} {f.LastName}"
              });
        }

        public void Dispose()
        {
            // Usually Service-Proxies are disposable. This method is added as demo-purpose
            // to show how to use an IDisposable in the client with a Func<T>. =>  Look for example at the FriendDataProvider-class
        }

        private void SaveToFile(List<Friend> friendList) //zapisuje do pliku wszystkich friendów
        {
            string json = JsonConvert.SerializeObject(friendList, Formatting.Indented); //serializacja kolekcji
            File.WriteAllText(StorageFile, json); //zapis kolecji
        }

        private List<Friend> ReadFromFile() //metoda odczytu z pliku bądź zapełnienie listy jeśli pliku nie ma
        {
            if (!File.Exists(StorageFile)) //jeśli pliku nie ma
            {
                return new List<Friend>
                {
                    new Friend{Id=1,FirstName = "Thomas",LastName="Huber",
                        Birthday = new DateTime(1980,10,28), IsDeveloper = true},
                    new Friend{Id=2,FirstName = "Julia",LastName="Huber",
                        Birthday = new DateTime(1982,10,10)},
                    new Friend{Id=3,FirstName="Anna",LastName="Huber",
                        Birthday = new DateTime(2011,05,13)},
                    new Friend{Id=4,FirstName="Sara",LastName="Huber",
                        Birthday = new DateTime(2013,02,25)},
                    new Friend{Id=5,FirstName = "Andreas",LastName="Böhler",
                        Birthday = new DateTime(1981,01,10), IsDeveloper = true},
                    new Friend{Id=6,FirstName="Urs",LastName="Meier",
                        Birthday = new DateTime(1970,03,5), IsDeveloper = true},
                     new Friend{Id=7,FirstName="Chrissi",LastName="Heuberger",
                        Birthday = new DateTime(1987,07,16)},
                     new Friend{Id=8,FirstName="Erkan",LastName="Egin",
                        Birthday = new DateTime(1983,05,23)},
                };
            }

            string json = File.ReadAllText(StorageFile); //jeśli plik jest przeczytaj wszystko
            return JsonConvert.DeserializeObject<List<Friend>>(json); //deserializacja do kolekcji friendów
        }
    }
}