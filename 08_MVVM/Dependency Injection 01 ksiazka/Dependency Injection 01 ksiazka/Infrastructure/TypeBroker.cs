using Dependency_Injection_01_ksiazka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dependency_Injection_01_ksiazka.Infrastructure
{
    public static class TypeBroker
    {
        private static Type repoType = typeof(MemoryRepository); //nowy typ
        private static IRepository testRepo; //instancja repo
        //zwracam testRepo, a jeśli null, to tworzę instancję (?)
        public static IRepository Repository =>
        testRepo ?? Activator.CreateInstance(repoType) as IRepository;
        //określenie typu (?)
        public static void SetRepositoryType<T>() where T : IRepository =>
        repoType = typeof(T);
        public static void SetTestObject(IRepository repo)
        {
            testRepo = repo;
        }
    }
}
