using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using XFTest.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace XFTest.ViewModels
{
	public class CleaningListViewModel : BindableBase, INotifyPropertyChanged
    {
        readonly IList<CleaningList> source;

        public ObservableCollection<CleaningList> Monkeys { get; private set; }


        public CleaningListViewModel( IDialogService dialogService, INavigationService navigationService)
        {
            source = new List<CleaningList>();
            CreateMonkeyCollection();
        }

        void CreateMonkeyCollection()
        {
            source.Add(new CleaningList
            {
                Name = "Wash1",

            });

            source.Add(new CleaningList
            {
                Name = "Wash2",

            });

            source.Add(new CleaningList
            {
                Name = "Wash3",

            });

            source.Add(new CleaningList
            {
                Name = "Wash4",

            });

            source.Add(new CleaningList
            {
                Name = "Wash5",
            });

            Monkeys = new ObservableCollection<CleaningList>(source);
        }
    }
}
