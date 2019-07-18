using System;

namespace Unit_Test_13___PresenterMock
{
    public class Presenter
    {
        private readonly IView _view;
        public Presenter(IView view)
        {
            _view = view;
            _view.Loaded += OnLoaded;
        }
        private void OnLoaded()
        {
            _view.Render("Witaj, świecie");
        }
    }
    public interface IView
    {
        event Action Loaded;
        void Render(string text);
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
