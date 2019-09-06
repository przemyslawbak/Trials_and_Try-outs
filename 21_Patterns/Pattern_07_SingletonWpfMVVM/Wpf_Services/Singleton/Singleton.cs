using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Services.Singleton
{
    public sealed class Singleton : ISingleton
    {
        public static Singleton Instance
        {
            get { return _lazyInstance.Value; }
        }
        //Use Lazy<T> to lazily initialize the class and provide thread-safe access
        private static readonly Lazy<Singleton> _lazyInstance = new Lazy<Singleton>(() => new Singleton());

        public double ValueOne { get; set; }
    }
}
