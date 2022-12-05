using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Financial.Services
{
    public class WindowManager: IWindowManager
    {
        /// <summary>
        /// Manages window lifetime.
        /// </summary>
        public WindowManager()
        {
            if (OpenViews == null)
            {
                OpenViews = new Dictionary<object, object>();
            }
        }

        //TODO: dialog window

        //TODO: file path

        public Dictionary<object, object> OpenViews { get; set; }

        /// <summary>
        /// Creates instance of Window of type T and shows it.
        /// </summary>
        /// <typeparam name="T">Window type</typeparam>
        public void OpenWindow<T>() where T : Window
        {
            if (!CheckIfAlreadyOpened(typeof(T)))
            {
                Window window = (T)Activator.CreateInstance(typeof(T));

                window.Closed += WindowClosed;
                window.Show();

                OpenViews.Add(window, window.DataContext);
            }
        }

        /// <summary>
        /// Finds Window in ActiveViews dictionary basing on VM value, closes this window, removes from dictionary.
        /// </summary>
        /// <param name="viewModel">View model object</param>
        public void CloseWindow(object viewModel)
        {
            if (viewModel != null)
            {
                Type type = viewModel.GetType();
                Window window = OpenViews.FirstOrDefault(view => view.Value.GetType() == type).Key as Window;

                if (window != null)
                {
                    window.Close();
                }
            }
        }

        /// <summary>
        /// Verify if window can be found in ActiveViews.
        /// </summary>
        /// <param name="type">Window type</param>
        /// <returns>is opened or not</returns>
        private bool CheckIfAlreadyOpened(Type type)
        {
            return OpenViews.Select(view => view.Key.GetType() == type).FirstOrDefault();
        }

        /// <summary>
        /// Handler for Closed event, triggered when window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        private void WindowClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Closed -= WindowClosed;

            OpenViews.Remove(window);
        }
    }
}
