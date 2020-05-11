using Prism.Mvvm;
using Prism.Navigation;
using XFTest.Interface;

namespace XFTest.ViewModels
{
    public class ViewModelBase : BindableBase
    {

        protected INavigationService NavigationService { get; private set; }

        public ViewModelBase(INavigationService navigationService, ICarFitApiService apiService)
        {
            NavigationService = navigationService;
        }
    }
}
