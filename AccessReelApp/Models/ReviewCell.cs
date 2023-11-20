using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.Models
{
    public class ReviewCell
    {
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public string PosterUrl { get; set; }
        public string CriticUrl { get; set; }
        public float? MovieRating { get; set; }
        public DateTime PostDate { get; set; }
    }
}
