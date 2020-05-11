using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XFTest.Interface;

namespace XFTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor
        public MainPageViewModel(INavigationService navigationService, ICarFitApiService apiService)
            : base(navigationService, apiService)
        {

        }
        #endregion

        #region Commands
        /// <summary>
        /// Handle Open CleanList Command
        /// </summary>
        private Command _openCleanListCommand;
        public Command OpenCleanListCommand
        {
            get => _openCleanListCommand ?? (_openCleanListCommand = new Command(ExecuteOpenCleanListCommand));
            set
            {
                _openCleanListCommand = value;
            }
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// ExecuteOpenCleanListCommand Method
        /// This will used to open CleanList page after click on button
        /// </summary>
        private void ExecuteOpenCleanListCommand()
        {
            try
            {
                NavigationService.NavigateAsync("CleaningList");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At ExecuteOpenCleanListCommand Methood : {0}", ex.Message);
            }
        }
        #endregion
    }

}
