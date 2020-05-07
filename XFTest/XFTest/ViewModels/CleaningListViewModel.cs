using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using XFTest.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace XFTest.ViewModels
{
    public class CleaningListViewModel : BindableBase, INotifyPropertyChanged
    {
        #region private / public members
        const string TIME_FORMATE = "HH:MM";
        readonly IList<Data> source;
        public ObservableCollection<Data> CarFitDataCollection { get; private set; }
        #endregion



        #region Constructor
        public CleaningListViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            //source object initialization
            source = new List<Data>();
            //Method for getting CarFit Records from json
            FetchCarFitRecords();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Below method will fetch the vlaues from JSON file
        /// Update some of properties for Data class so we can used in Binding in UI
        /// Store the final list in to readonly source object so while click on calender we can fect records based on date selection from Total Records
        //  Get CarFitDataCollection values from source itself 
        /// Set CarFitDataCollection as CollectionView ItemsSource
        /// </summary>
        private void FetchCarFitRecords()
        {
            string jsonFileName = "CarFitData.json";
            var assembly = typeof(CleaningListViewModel).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                //Converting JSON Array Objects into generic list and update some of newly added properties values
                if (JsonConvert.DeserializeObject<CarFitRecords>(jsonString) != null && JsonConvert.DeserializeObject<CarFitRecords>(jsonString).data.Any())
                    //iterate the each record and made necessary update in values
                    JsonConvert.DeserializeObject<CarFitRecords>(jsonString).data.ForEach(
                        //Change the formate for some Data calss properties based on requirement
                        car => UpdateCarFitData(car)
                     );

            }
            //Setting CollectionView ItemsSource value as Bindable property to CarFitData object from read only sorce object      
            CarFitDataCollection = new ObservableCollection<Data>(source);

        }
        /// <summary>
        /// Main method which used to update some of Data class properties based on our requirement
        /// Handle null cheeck and if value is there then only assign that property value otherwise set as blank value so in UI it will not displyed
        /// </summary>
        Data UpdateCarFitData(Data car)
        {
            //Setting Requried time formate based on task requirement
            car.startTime = (true ? car.startTimeUtc.ToString(TIME_FORMATE) : string.Empty);
            //Replacing '/' into '-' for expectedTimeDisplay based on UI task requirement
            car.expectedTimeDisplay = (!string.IsNullOrEmpty(car.expectedTime) ? car.expectedTime.Replace("/", "-") : string.Empty);
            //Appending '/' before expectedTimeDisplay based on UI task requirement
            car.expectedTimeDisplay = (!string.IsNullOrEmpty(car.expectedTimeDisplay) ? " / " + car.expectedTimeDisplay : string.Empty);
            //Calculate Tasks total time given in minutes
            car.tasksTotalTime = car.tasks.Sum(x => x.timesInMinutes).ToString();
            //If taskTotalTime is there then append min string otherwise keep string as Empty
            car.tasksTotalTime = !string.IsNullOrEmpty(car.tasksTotalTime) ? car.tasksTotalTime + " min" : string.Empty;
            //Combine all three address in one property in Data class and used that in Binding for displaying full address
            car.houseAddress = GetFullAddress(car.houseOwnerAddress, car.houseOwnerZip, car.houseOwnerCity);
            //Adding Final Data Object in to source
            source.Add(car);
            return car;
        }
        /// <summary>
        //Handle null cheeck and if value is there then only display it in UI otherwise display as blank
        /// </summary>
        private string GetFullAddress(string s1, string s2, string s3)
        {
            string[] addressArray = { s1, s2, s3 };
            return string.Join(" ", addressArray.Where(s => !string.IsNullOrEmpty(s)));
        }
        #endregion
    }
}
