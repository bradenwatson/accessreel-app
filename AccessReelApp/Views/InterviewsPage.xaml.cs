using AccessReelApp.database_structures;
using AccessReelApp.ViewModels;
using System.Diagnostics;
using System.Windows.Input;

namespace AccessReelApp.Views;

public partial class InterviewsPage : ContentPage
{
    //DatabaseControl databaseControl = new DatabaseControl();
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public InterviewsPage(InterviewsViewModel vm)
    {
        InitializeComponent();
        ViewModelLocator.InterviewsViewModelInstance = vm;
        BindingContext = vm;
    }

}