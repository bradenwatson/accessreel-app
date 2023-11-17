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
        [ObservableProperty] ObservableCollection<ImageButton> trailers;

        void Initialise()
        {
            // if null -> use compound assign
            ButtonCollection ??= new ObservableCollection<ImageButton>();
            Trailers ??= new ObservableCollection<ImageButton>();
        }

        void AddImageButtons(string source)
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
