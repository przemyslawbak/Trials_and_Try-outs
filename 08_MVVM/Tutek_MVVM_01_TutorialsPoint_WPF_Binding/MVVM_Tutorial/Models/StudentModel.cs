using System.ComponentModel;

namespace MVVM_Tutorial.Models
{
    public class StudentModel
    {
    }
    public class Student : INotifyPropertyChanged
    {
        private string firstName; private string lastName;
        public string FirstName
        {
            get => firstName;
            set {
                if (firstName != value)
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                    RaisePropertyChanged("FullName");
                }
            }
        }
        public string LastName { get { return lastName; } set { if (lastName != value) { lastName = value; RaisePropertyChanged("LastName"); RaisePropertyChanged("FullName"); } } }
        public string FullName { get { return firstName + " " + lastName; } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}


