using AccessReelApp.Models;
using AccessReelApp.Prototypes;
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
        ObservableCollection<ReviewCell> _reviewCells;
        public ObservableCollection<ReviewCell> ReviewCells
        {
            get { return _reviewCells; }
            set
            {
                _reviewCells = value;
                OnPropertyChanged();
            }
        }

        public void ReviewFetchedHandler(string movieKey, string posterUrl, ReviewCell reviewCell)
        {
            CarouselModel.SetImageSource(movieKey, posterUrl);
            ReviewCells.Add(reviewCell);
        }

        static NewsViewModel _instance;
        public static NewsViewModel Instance
        {
            get
            {
                _instance ??= new NewsViewModel();
                return _instance;
            }
        }

        ObservableCollection<ReviewCell> GetOrCreateReviewCellsCollection()
        {
            _reviewCells ??= new ObservableCollection<ReviewCell>();
            return _reviewCells;
        }
    }
}