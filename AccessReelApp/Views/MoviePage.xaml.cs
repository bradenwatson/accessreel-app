using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessReelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePage : TabbedPage
    {
        public MoviePage ()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}