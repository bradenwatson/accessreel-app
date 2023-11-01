using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;

namespace AccessReelApp
{
	public partial class MainPage : ContentPage
	{
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}

