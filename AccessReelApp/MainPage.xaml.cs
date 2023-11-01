using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;
using Plugin.LocalNotification;

namespace AccessReelApp
{
	public partial class MainPage : ContentPage
	{
		DatabaseControl databaseControl = new DatabaseControl();

		public MainPage(MainViewModel vm)
		{
			InitializeComponent();
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

		private void Button_Clicked(object sender, EventArgs e)
        {
			var request = new NotificationRequest
			{
				NotificationId = 1337,
				Title = "Hello World",
				Subtitle = "Test",
				Description = "Working",
				BadgeNumber = 42,
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = DateTime.Now.AddSeconds(5),
					NotifyRepeatInterval = TimeSpan.FromDays(1),
				}
			};
			LocalNotificationCenter.Current.Show(request);
        }
    }
}

