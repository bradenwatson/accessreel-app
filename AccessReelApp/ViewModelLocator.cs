using AccessReelApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public static class ViewModelLocator
    {
        public static NewsViewModel NewsViewModelInstance { get; set; }
        public static InterviewsViewModel InterviewsViewModelInstance { get; set; }
    }
}
