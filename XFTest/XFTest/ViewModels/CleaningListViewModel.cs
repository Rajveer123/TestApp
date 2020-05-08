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
using XFTest.Helper;
using Xamarin.Forms;
using System.Windows.Input;

namespace XFTest.ViewModels
{
    public class CleaningListViewModel : BindableBase, INotifyPropertyChanged
    {
        #region private / public members
        //used to get count for row in for each
        int carFitCounter = 0;
        //Time formate we are going to use for displaying startTime
        const string TIME_FORMATE = "HH:MM";
        //It will contains the total number of records so next time if we wants to fetch data after perticular date selected by user from calender
        readonly IList<Data> source;
        //This wil used as bindable property for item source for collectionview
        public ObservableCollection<Data> CarFitDataCollection { get; private set; }
        //Below class will contains all required utitiley methods
        private UtilityMethods utilityMethods;
        public ICommand RefreshCommand { get; }
        #endregion

        #region Properties
        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                RaisePropertyChanged(nameof(IsRefreshing));
            }
        }
        #endregion

        #region Constructor
        public CleaningListViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            try
            {
                //source object initialization
                source = new List<Data>();
                //Initializing UtilityMethods object so we can access its methods
                utilityMethods = new UtilityMethods();
                //Initializing RefreshCommand used for PULL to Refresh
                RefreshCommand = new Command(ExecuteRefreshCommand);
                //Method for getting CarFit Records from json
                fetchCarFitRecords();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At Constructor : {0}", ex.Message);
            }

        }
        #endregion

        #region private methods
        /// <summary>
        /// Toggle PULL to Refresh Feature: - In which we will display only Data having visitState 'Done', 'In-Progress'
        /// So you have idea that pull to refresh is working fine 
        /// And when again pull to refresh we will display total items of list from source object
        /// Hense acheving Toggle Refresh feature on each pull to refresh
        /// </summary>
        private void ExecuteRefreshCommand()
        {
            if (IsRefreshing)
                return;
            int itemCount = CarFitDataCollection.Count;
            CarFitDataCollection.Clear();
            foreach (var item in source)
            {
                //If list having all itesms then we have to show filterd item on refresh
                if (itemCount == source.Count)
                {

                    if ((item.visitState == "Done" || item.visitState == "InProgress"))
                        CarFitDataCollection.Add(item);
                }
                else
                {
                    //If list is already filterd then on refresh we will show all iteams again in list
                    CarFitDataCollection.Add(item);
                }

            }
            // Stop refreshing
            IsRefreshing = false;
        }
        /// <summary>
        /// Below method will fetch the vlaues from JSON file
        /// Update some of properties for Data class so we can used in Binding in UI
        /// Store the final list in to readonly source object so while click on calender we can fect records based on date selection from Total Records
        //  Get CarFitDataCollection values from source itself 
        /// Set CarFitDataCollection as CollectionView ItemsSource
        /// </summary>
        private void fetchCarFitRecords()
        {
            try
            {

                carFitCounter = 0;
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
                            car => updateCarFitData(car, JsonConvert.DeserializeObject<CarFitRecords>(jsonString).data)
                         );

                }
                //Setting CollectionView ItemsSource value as Bindable property to CarFitData object from read only sorce object      
                CarFitDataCollection = new ObservableCollection<Data>(source);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At fetchCarFitRecords Method : {0}", ex.Message);
            }
        }
        /// <summary>
        /// Main method which used to update some of Data class properties based on our requirement
        /// Handle null cheeck and if value is there then only assign that property value otherwise set as blank value so in UI it will not displyed
        /// </summary>
        private Data updateCarFitData(Data car, List<Data> carFitData)
        {
            try
            {
                //Setting Button Background Color Theme
                car.backgroundTheme = utilityMethods.GetBackgroundThemColor(car.visitState);
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
                car.houseAddress = utilityMethods.GetFullAddress(car.houseOwnerAddress, car.houseOwnerZip, car.houseOwnerCity);
                //Note : - Logic for Calculating Distance in KM DYNAMICALLY no matter how much places are there in json string
                //Until we does not reached to the last place we will take lat long for subsciquest places
                //And once reached at the last place then we will calculate his distance from first place so last entry of list will show distance between first element of list and last element
                if (carFitCounter != carFitData.Count - 1)
                {
                    //Comparing distance for two subsequest places i.e 1-2, 2-3,4-5...9-10
                    //Allow only one digit after decimal for displaying value of distance
                    car.distance = string.Format("{0:0.0}", (utilityMethods.GetDistance(carFitData[carFitCounter].houseOwnerLatitude, carFitData[carFitCounter].houseOwnerLongitude, carFitData[carFitCounter + 1].houseOwnerLatitude, carFitData[carFitCounter + 1].houseOwnerLongitude, 'K'))) + " km";

                }
                else
                {
                    //Once reached the last place Comparing that place distance from first place i.e 10-1
                    //Allow only one digit after decimal for displaying value of distance
                    car.distance = string.Format("{0:0.0}", (utilityMethods.GetDistance(carFitData[carFitCounter].houseOwnerLatitude, carFitData[carFitCounter].houseOwnerLongitude, carFitData[0].houseOwnerLatitude, carFitData[0].houseOwnerLongitude, 'K'))) + " km";
                }
                //Adding Final Data Object in to source
                source.Add(car);
                carFitCounter++;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At updateCarFitData Method : {0}", ex.Message);
            }
            return car;
        }
        #endregion
    }
}
