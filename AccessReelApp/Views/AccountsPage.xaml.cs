using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class AccountsPage : ContentPage
{
	public AccountsPage(AccountsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}