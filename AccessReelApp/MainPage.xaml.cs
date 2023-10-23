using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;

namespace AccessReelApp;

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

	void TestLoadFromDataBase()
	{
		databaseControl.LoadData();
	}

	void TestDatabaseDefaultData()
	{	
		List<TestStructure> testStructures = new List<TestStructure>();

		for (int i = 0; i < 10; i++)
		{
			TestStructure newTestStructure = new TestStructure();
			newTestStructure.name = $"test structure {i}";
			testStructures.Add(newTestStructure);
		}

		databaseControl.SaveData(testStructures);
	}

    private void Saved_Clicked(object sender, EventArgs e)
    {
		TestDatabaseDefaultData();
    }

    private void Load_Clicked(object sender, EventArgs e)
    {
		TestLoadFromDataBase();
    }
}

