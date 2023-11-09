using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class SignUpLogin : ContentPage
{
	public SignUpLogin(SignUpLoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}