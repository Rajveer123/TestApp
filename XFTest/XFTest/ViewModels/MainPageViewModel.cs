using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFTest.Interface;

namespace XFTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, ICarFitApiService apiService)
            : base(navigationService, apiService)
        {

        }
    }

}
