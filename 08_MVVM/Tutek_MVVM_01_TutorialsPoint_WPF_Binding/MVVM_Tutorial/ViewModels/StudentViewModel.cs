using MVVM_Tutorial.Models;
using System.Collections.ObjectModel;

namespace MVVM_Tutorial.ViewModels
{
    public class StudentViewModel
    {
        public MyICommand DeleteCommand { get; set; }
        public StudentViewModel() { LoadStudents(); DeleteCommand = new MyICommand(OnDelete, CanDelete); }

        private void OnDelete() { Students.Remove(SelectedStudent); }
        private bool CanDelete() { return SelectedStudent != null; }
        private Student _selectedStudent; public Student SelectedStudent { get { return _selectedStudent; } set { _selectedStudent = value; DeleteCommand.RaiseCanExecuteChanged(); } }
        public ObservableCollection<Student> Students { get; set; }

        public void LoadStudents()
        {
            ObservableCollection<Student> students = new ObservableCollection<Student>();
            students.Add(new Student { FirstName = "Mark", LastName = "Allain" });
            students.Add(new Student { FirstName = "Allen", LastName = "Brown" });
            students.Add(new Student { FirstName = "Linda", LastName = "Hamerski" });

            Students = students;
        }
    }
}
