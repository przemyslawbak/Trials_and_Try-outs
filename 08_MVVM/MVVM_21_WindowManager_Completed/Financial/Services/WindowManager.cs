using Financial.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Financial.Services
{
    public class WindowManager : IWindowManager
    {
        /// <summary>
        /// Manages opened views.
        /// </summary>
        public WindowManager()
        {
            if (OpenedViews == null)
            {
                OpenedViews = new List<WindowModel>();
            }
        }

        /// <summary>
        /// List of currently opened WindowModels.
        /// </summary>
        public List<WindowModel> OpenedViews { get; set; }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <param name="messageBoxText">Passed text to be displayed</param>
        /// <param name="messageBoxTitle">Passed title of the window to be displayed</param>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle)
        {
            bool? result = null;

            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, messageBoxTitle, button);

            switch (messageResult)
            {
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
                case MessageBoxResult.Cancel:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <param name="messageBoxTitle">Passed title of the window to be displayed</param>
        /// <returns>String path</returns>
        public string OpenFileDialogWindow(string messageBoxTitle)
        {
            string result = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                openFileDialog.Title = messageBoxTitle;
                result = openFileDialog.FileName;
            }

            return result;
        }

        /// <summary>
        /// Creates new ModelWindow for modal dialog Window of type T that returns object
        /// </summary>
        /// <typeparam name="T">Window type</typeparam>
        /// <returns>Object type</returns>
        public async Task<object> OpenResultWindow<T>() where T : Window
        {
            if (!CheckIfAlreadyOpened(typeof(T)))
            {
                WindowModel model = CreateWindoModelAndShow<T>();

                model.OpenedWindow.Closed += new EventHandler((s, e) => ResultWindowClosed(s, e, model));
                model.OpenedWindow.ShowDialog();

                while (!model.IsValueReturned)
                    await Task.Delay(100);

                return model.ReturnedObjectResult;
            }

            return null;
        }

        /// <summary>
        /// Creates new ModelWindow for modal dialog Window of type T that returns nullable bool
        /// </summary>
        /// <typeparam name="T">Window type</typeparam>
        /// <returns>Nullable bool type</returns>
        public async Task<bool?> OpenModalDialogWindow<T>() where T : Window
        {
            if (!CheckIfAlreadyOpened(typeof(T)))
            {
                WindowModel model = CreateWindoModelAndShow<T>();

                model.OpenedWindow.Closed += new EventHandler((s, e) => DialogWindowClosed(s, e, model));
                model.OpenedWindow.ShowDialog();

                while (!model.IsValueReturned)
                    await Task.Delay(100);

                return model.ReturnedDialogResult;
            }

            return null;
        }

        /// <summary>
        /// Creates new ModelWindow for Window of type T
        /// </summary>
        /// <typeparam name="T">Window type</typeparam>
        public void OpenWindow<T>() where T : Window
        {
            if (!CheckIfAlreadyOpened(typeof(T)))
            {
                WindowModel model = CreateWindoModelAndShow<T>();

                model.OpenedWindow.Closed += new EventHandler((s, e) => WindowClosed(s, e, model));
                model.OpenedWindow.Show();
            }
        }

        /// <summary>
        /// Finds Window in OpenedViews List basing on VM object type, closes this window, removes from dictionary.
        /// </summary>
        /// <param name="viewModel">View model object</param>
        public void CloseWindow(object viewModel)
        {
            if (viewModel != null)
            {
                Type type = viewModel.GetType();
                Window window = OpenedViews.FirstOrDefault(model => model.AssignedViewModel.GetType() == type).OpenedWindow as Window;

                if (window != null)
                {
                    window.Close();
                }
            }
        }

        /// <summary>
        /// Verify if window can be found in OpenedViews.
        /// </summary>
        /// <param name="windowType">Window type</param>
        /// <returns>Bool type, if the window is opened already or not</returns>
        private bool CheckIfAlreadyOpened(Type windowType)
        {
            return OpenedViews.Select(model => model.OpenedWindow.GetType() == windowType).FirstOrDefault();
        }

        /// <summary>
        /// Handler for Closed event, triggered when window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="model">Window model object related to the window</param>
        private void WindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => WindowClosed(s, e, model));

            OpenedViews.Remove(model);
        }

        /// <summary>
        /// Handler for Closed event, triggered when dialog window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="model">Window model object related to the window</param>
        private void DialogWindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => DialogWindowClosed(s, e, model));

            OpenedViews.Remove(model);

            var vm = window.DataContext as IDialogViewModel;
            model.ReturnedDialogResult = vm.DialogResult;
            model.IsValueReturned = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="model"></param>
        private void ResultWindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => ResultWindowClosed(s, e, model));

            OpenedViews.Remove(model);

            var vm = window.DataContext as IResultViewModel;
            model.ReturnedObjectResult = vm.ObjectResult;
            model.IsValueReturned = true;
        }

        /// <summary>
        /// Creates new WindowModel based on Window type.
        /// </summary>
        /// <typeparam name="T">Window type</typeparam>
        /// <returns>WindowModel</returns>
        private WindowModel CreateWindoModelAndShow<T>() where T : Window
        {
            Window window = (T)Activator.CreateInstance(typeof(T));
            WindowModel model = new WindowModel()
            {
                OpenedWindow = window,
                AssignedViewModel = window.DataContext,
                IsValueReturned = false,
                ReturnedDialogResult = null,
                ReturnedObjectResult = null,
                ReturnedFilePathResult = null
            };

            OpenedViews.Add(model);

            return model;
        }
    }
}
