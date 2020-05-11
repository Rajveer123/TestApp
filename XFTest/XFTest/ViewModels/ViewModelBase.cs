using Prism.Mvvm;
using Prism.Navigation;
using XFTest.Interface;

namespace XFTest.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, IDestructible, INavigationAware
    {

        protected INavigationService NavigationService { get; private set; }



        public ViewModelBase(INavigationService navigationService, ICarFitApiService apiService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
