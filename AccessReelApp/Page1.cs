namespace AccessReelApp
{
    public class Page1 : ContentPage
    {
        public Page1()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Page 1" }
                }
            };
        }
    }
}