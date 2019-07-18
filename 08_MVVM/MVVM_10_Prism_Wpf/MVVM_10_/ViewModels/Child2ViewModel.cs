﻿using MVVM_10_.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.ViewModels
{
    public class Child2ViewModel
    {
        public string Name { get; set; }

        public Child2ViewModel()
        {
            Utility.EventAggregator.GetEvent<UpdateNameEvent>().Subscribe(UpdateName);
        }

        private void UpdateName(string name)
        {
            this.Name = name;
        }
    }
}
