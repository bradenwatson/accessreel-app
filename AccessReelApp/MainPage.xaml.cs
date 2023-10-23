using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;

namespace AccessReelApp
{
	public partial class MainPage : ContentPage
	{
		DatabaseControl databaseControl = new DatabaseControl();

		public MainPage(MainViewModel vm)
		{		
			BindingContext = vm;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext is MainViewModel vm)
			{
				vm.Text = "Changed!";
			}
		}
    }
}

