﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamFormsMVVM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactListPage : ContentPage
	{
        public const string ItemSelectedCommandPropertyName = "ItemSelectedCommand";
        public static BindableProperty ItemSelectedCommandProperty =
        BindableProperty.Create(
        propertyName: "ItemSelectedCommand",
        returnType: typeof(ICommand),
        declaringType: typeof(ContactListPage),
        defaultValue: null);
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            RemoveBinding(ItemSelectedCommandProperty);
            SetBinding(ItemSelectedCommandProperty, new
            Binding(ItemSelectedCommandPropertyName));
        }
        private void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            var command = ItemSelectedCommand;
            if (command != null && command.CanExecute(e.SelectedItem))
            {
                command.Execute(e.SelectedItem);
            }
        }
        public ContactListPage()
        {
            InitializeComponent();
        }
    }
}