using System;

namespace TemplateMvvm.ViewModels
{
    public class PersonViewModel
    {
        public string Name { get; } = "Bobby Tables"; //hard code model
        public DateTime DateOfBirth { get; } = new DateTime(1980, 03, 24); //hard code model
    }
}