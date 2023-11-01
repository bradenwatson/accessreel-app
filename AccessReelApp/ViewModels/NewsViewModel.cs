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
        public ObservableCollection<ImageButton> ButtonCollection { get; set; }
        
        public NewsViewModel() 
        {
            ButtonCollection = new ObservableCollection<ImageButton>
            {
                new ImageButton
                {
                    Source = "barbie.jpg"
                }
            };
        }
    }
}
