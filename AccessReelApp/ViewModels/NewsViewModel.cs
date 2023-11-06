using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessReelApp.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<ImageButton> buttonCollection;

        void Initialise()
        {
            if (ButtonCollection == null)
            {
                ButtonCollection = new ObservableCollection<ImageButton>();
            }
        }

        void AddImageButton(string source)
        {
            Initialise();
            CarouselModel.SetImageSource(source);
            ButtonCollection.Add(CarouselModel.ButtonCollection[0]); // wasn't public 
            // Careful not to get confused as you have a ButtonCollection here as well
        }

        public NewsViewModel()
        {
            AddImageButton("turtles.jpg");
        }
    }
}
