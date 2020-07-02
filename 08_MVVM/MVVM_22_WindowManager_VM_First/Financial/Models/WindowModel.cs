using System.Windows;

namespace Financial.Models
{
    public class WindowModel
    {
        public Window OpenedWindow { get; set; }
        public object AssignedViewModel { get; set; }
        public bool IsValueReturned { get; set; }
        public bool? ReturnedDialogResult { get; set; }
        public object ReturnedObjectResult { get; set; }
        public string ReturnedFilePathResult { get; set; }
    }
}
