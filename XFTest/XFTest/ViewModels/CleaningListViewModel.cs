using System.Collections.Generic;
using System.Collections.ObjectModel;
using XFTest.Models;
using Prism.Navigation;
using System.Linq;
using System;
using XFTest.Helper;
using Xamarin.Forms;
using System.Windows.Input;
using XFTest.Interface;


namespace XFTest.ViewModels
{
    public class CleaningListViewModel : ViewModelBase, INavigationAware
    {
        #region private / public members
        private readonly ICarFitApiService _apiService;
        //Time formate we are going to use for displaying startTime
        const string TIME_FORMATE = "HH:MM";
        //Used to Assigned Filtered Dates after date selection and on pull to refresh
        ObservableCollection<Data> filteredCarfitData = new ObservableCollection<Data>();
        private DateTime currentDate = DateTime.Now;
        //It will contains the total number of records so next time if we wants to fetch data after perticular date selected by user from calender
        public readonly IList<Data> source;
        //Below class will contains all required utitiley methods
        private UtilityMethods utilityMethods;
        //Used to get count for row in for each
        int carFitCounter;
        #endregion

        #region Properties
        /// <summary>
        /// CarFitDataCollection Property
        /// This wil used as bindable property for item source for collectionview 
        /// </summary>
        private ObservableCollection<Data> _carFitDataCollection;
        public ObservableCollection<Data> CarFitDataCollection
        {
            get { return _carFitDataCollection ?? (_carFitDataCollection = new ObservableCollection<Data>()); }
            set { _carFitDataCollection = value; RaisePropertyChanged("CarFitDataCollection"); }
        }
        /// <summary>
        /// CalenderView Property
        /// Which dynamically loads the calender dates
        /// </summary>
        private StackLayout _calenderView;
        public StackLayout CalenderView
        {
            get { return _calenderView ?? (_calenderView = new StackLayout()); }
            set { _calenderView = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// State Property
        /// </summary>

        private string _state = "Large";
        public string State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Selected CalenderDate - Initially set today's Date By Default
        /// </summary>
        private DateTime _selectedCalenderDate = DateTime.Now;
        public DateTime SelectedCalenderDate
        {
            get { return _selectedCalenderDate; }
            set { _selectedCalenderDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Second Selected Calender Date - Initially set today's Date By Default
        /// </summary>
        private DateTime _selectedSecondCalenderDate = DateTime.Now;
        public DateTime SelectedSecondCalenderDate
        {
            get { return _selectedSecondCalenderDate; }
            set { _selectedSecondCalenderDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Calender Title
        /// </summary>
        private string _calenderTitle;
        public string CalenderTitle
        {
            get { return _calenderTitle; }
            set { _calenderTitle = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Page Title
        /// </summary>
        private string _pageTitle = "Today";
        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Empty View Visibility Property
        /// </summary>
        private bool _emptyViewVisibility;
        public bool EmptyViewVisibility
        {
            get { return _emptyViewVisibility; }
            set { _emptyViewVisibility = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Empty View Header Label Title
        /// </summary>
        private string _emptyViewHeaderLabel;
        public string EmptyViewHeaderLabel
        {
            get { return _emptyViewHeaderLabel; }
            set { _emptyViewHeaderLabel = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Empty View SubTitle Label Title
        /// </summary>
        private string _emptyViewSubTitleLabel = "Try To Select Different Dates From Calender.";
        public string EmptyViewSubTitleLabel
        {
            get { return _emptyViewSubTitleLabel; }
            set { _emptyViewSubTitleLabel = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Below Property is used to show / hide overlay activity indicator view while list is loading data
        /// </summary>
        bool isExecuting;
        public bool IsExecuting
        {
            get => isExecuting;
            set
            {
                isExecuting = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Below Property is used to control Pull To Refresh Feature
        /// </summary>
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
        /// <summary>
        /// Below Property is used to control visibility of Header View
        /// </summary>
        bool isHeaderVisibile = true;
        public bool HeaderVisibility
        {
            get => isHeaderVisibile;
            set
            {
                isHeaderVisibile = value;
                RaisePropertyChanged(nameof(HeaderVisibility));
            }
        }
        /// <summary>
        /// Below Property is used to control visibility of Calender View
        /// </summary>
        bool isCalenderVisibile = false;
        public bool CalenderVisibility
        {
            get => isCalenderVisibile;
            set
            {
                isCalenderVisibile = value;
                RaisePropertyChanged(nameof(CalenderVisibility));
            }
        }
        #endregion

        #region Constructor
        public CleaningListViewModel(INavigationService navigationService, ICarFitApiService apiService)
            : base(navigationService, apiService)
        {
            try
            {
                //Show Activity Indicator Overlay view while list is loading data
                IsExecuting = true;
                //Object Initialization
                _apiService = apiService;
                source = new List<Data>();
                utilityMethods = new UtilityMethods();

            }
            catch (Exception ex)
            {
                HandleException("Constructor", ex.Message, true);
            }

        }
        #endregion

        #region Commands
        /// <summary>
        /// Handle Arrow Command of calender view to load next / previous month calender
        /// </summary>
        private Command<string> _handleArrowCommand;
        public Command<string> HandleArrowCommand
        {
            get => _handleArrowCommand ?? (_handleArrowCommand = new Command<string>(ExecuteHandleArrowCommand));
            set
            {
                _handleArrowCommand = value;
            }
        }
        /// <summary>
        /// Handle Calendar Date Selection Command of calender view once user selects any date
        /// </summary>
        private Command<string> _handleCalendarDateSelectionCommand;
        public Command<string> HandleCalendarDateSelectionCommand
        {
            get => _handleCalendarDateSelectionCommand ?? (_handleCalendarDateSelectionCommand = new Command<string>(ExecuteCalendarDateSelectionCommand));
            set
            {
                _handleCalendarDateSelectionCommand = value;
            }
        }
        /// <summary>
        /// Show Dialog Command used for for displaying calender view
        /// </summary>
        private ICommand _showDialogCommand;
        public ICommand ShowDialogCommand
        {
            get => _showDialogCommand ?? (_showDialogCommand = new Command(ExecuteShowDialogCommand));
            set
            {
                _showDialogCommand = value;
            }
        }
        /// <summary>
        /// Refresh Command used for Pull to Refresh Feature
        /// </summary>
        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get => _refreshCommand ?? (_refreshCommand = new Command(ExecuteRefreshCommand));
            set
            {
                _refreshCommand = value;
            }
        }
        /// <summary>
        /// Refresh Command used for Pull to Refresh Feature
        /// </summary>
        private ICommand _handleOutSideClickCommand;
        public ICommand HandleOutSideClickCommand
        {
            get => _handleOutSideClickCommand ?? (_handleOutSideClickCommand = new Command(ExecuteHandleOutSideClickCommand));
            set
            {
                _handleOutSideClickCommand = value;
            }
        }

        #endregion

        #region private methods
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        /// <summary>
        /// Method used to call the injected method to get JSON data
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                //Get Car Fit Records From JSON File
                List<Data> carFitRecords = _apiService.GetCarFitOrders();
                if (carFitRecords != null && carFitRecords.Any())
                    carFitRecords.ForEach(
                                    //Change the formate for some Data calss properties based on requirement
                                    car => UpdateCarFitData(car, carFitRecords)
                                 );
                //Setting CollectionView ItemsSource value as Bindable property to CarFitData object from read only sorce object
                //After Calcualting Distancee between each places
                //Note : - Logic for Calculating Distance in KM DYNAMICALLY no matter how much places are there in json string
                CarFitDataCollection = utilityMethods.CalculateDistance(new ObservableCollection<Data>(source));
                //Hide overlay indicator view as list data has been set
                IsExecuting = false;
                //Show Empty View Visibility
                EmptyViewVisibility = true;
            }
            catch (Exception ex)
            {
                HandleException("OnNavigatedTo", ex.Message, true);
            }
        }
        /// <summary>
        /// ExecuteHandleArrowCommand
        /// This will used to load next or preevious month calender dates based on arrowType clicked by user
        /// </summary>
        /// <param name="arrowType"></param>
        private void ExecuteHandleArrowCommand(string arrowType)
        {
            try
            {
                DateTime newCalenderMonth;
                if (arrowType == "previous")
                {
                    newCalenderMonth = currentDate.AddMonths(-1);

                }
                else
                {
                    newCalenderMonth = currentDate.AddMonths(1);
                }
                //Setting modified date in currentDate again
                currentDate = newCalenderMonth;
                //And by selecting today's date by default selected
                List<DateTime> dates = utilityMethods.GetDates(currentDate.Year, currentDate.Month);
                //Load the Calender View Based on above dates
                LoadCalenderViewDates(dates);

            }
            catch (Exception ex)
            {
                HandleException("ExecuteHandleArrowCommand", ex.Message);
            }
        }
        /// <summary>
        /// Execute Handle OutSide Click of Calendar Command
        /// It will first hide the calender view and display Header view
        /// </summary>
        private void ExecuteHandleOutSideClickCommand()
        {
            try
            {
                HeaderVisibility = true;
                CalenderVisibility = false;
            }
            catch (Exception ex)
            {
                HandleException("ExecuteHandleOutSideClickCommand", ex.Message);
            }

        }
        /// <summary>
        /// Execute Calendar Date Selection Command
        /// It will first hide the calender view and display Header view
        /// Secondly This will load the carfit list data based on date selected by user
        /// </summary>
        private void ExecuteCalendarDateSelectionCommand(string selectedCalenderDate)
        {
            try
            {
                IsExecuting = true;
                HeaderVisibility = true;
                CalenderVisibility = false;
                //Update the page title once date selected from calender i.e Today or Date along with Month and Year
                PageTitle = (((DateTime.Now.Year == currentDate.Year) && (DateTime.Now.Month == currentDate.Month) && ((DateTime.Now.Day == Convert.ToInt32(selectedCalenderDate))))) ? "Today" : (selectedCalenderDate + " " + currentDate.ToString("MMMM") + " " + currentDate.Year);
                //Update the selected calender dates value based on user date selection
                //So next time when calender gets open it shows the last selected dates
                //As per logic it only displays latest selected two dates only
                DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, Convert.ToInt32(selectedCalenderDate));
                //If same date selected from calender no need to do further operation
                if ((SelectedCalenderDate.Year == selectedDate.Year && SelectedCalenderDate.Month == selectedDate.Month && SelectedCalenderDate.Day == selectedDate.Day) || (SelectedSecondCalenderDate.Year == selectedDate.Year && SelectedSecondCalenderDate.Month == selectedDate.Month && SelectedSecondCalenderDate.Day == selectedDate.Day))
                {
                    //Hide overlay indicator view
                    IsExecuting = false;
                    return;
                }
                else
                {
                    //Perform All Calender Date Selected Dates Logic
                    SettingCalendarDates(selectedDate);
                }
                //Update message text for Empty view based on two dates selected range
                EmptyViewHeaderLabel = "Sorry!! No CarFit Order Records Found In Between " + (SelectedCalenderDate.Day + " " + SelectedCalenderDate.ToString("MMMM") + " " + SelectedCalenderDate.Year) + " to " + (SelectedSecondCalenderDate.Day + " " + SelectedSecondCalenderDate.ToString("MMMM") + " " + SelectedSecondCalenderDate.Year) + ".";
                //Clear Collection and filteredCarfitData List Data
                ClearCollectionsData();
                //Get Filter data based on date selected by user
                foreach (var item in source)
                {
                    DateTime date = (DateTime)Convert.ChangeType(item.startTimeUtc, typeof(DateTime));
                    //If list having all items then we have to show filterd items based on two dates selected from calender
                    //i.e The data which are in between two selected dates with in calender view
                    if (utilityMethods.IsDateInBetweenIncludingEndPoints(SelectedCalenderDate, SelectedSecondCalenderDate, date))
                    {
                        filteredCarfitData.Add(item);
                    }

                }

                //Adding Final Filtered Data in CarFitDataCollection after Calculating Distance between them
                ObservableCollection<Data> distanceCalculatedCarFitData = utilityMethods.CalculateDistance(filteredCarfitData);
                foreach (var item in distanceCalculatedCarFitData)
                {
                    CarFitDataCollection.Add(item);
                }
                IsExecuting = false;
            }
            catch (Exception ex)
            {
                HandleException("ExecuteCalendarDateSelectionCommand", ex.Message, true);
            }
        }
        /// <summary>
        /// Method clears CarFit and filterCarFit Data
        /// </summary>
        private void ClearCollectionsData()
        {
            try
            {
                //Clear Collection and filteredCarfitData List Data
                CarFitDataCollection.Clear();
                filteredCarfitData.Clear();
            }
            catch (Exception ex)
            {
                HandleException("ClearCollectionsData", ex.Message);
            }

        }
        /// <summary>
        /// Below funcation contains Both Calendar Dates Selecction Logic
        /// </summary>
        /// <param name="selectedDate"></param>
        private void SettingCalendarDates(DateTime selectedDate)
        {
            try
            {
                int curretDateDayDifference = Math.Abs(Convert.ToInt32((selectedDate - SelectedCalenderDate).TotalDays));
                int secondDateDayDifference = Math.Abs(Convert.ToInt32((selectedDate - SelectedSecondCalenderDate).TotalDays));
                if (curretDateDayDifference == secondDateDayDifference)
                {
                    if (SelectedCalenderDate < selectedDate)
                    {
                        SelectedSecondCalenderDate = selectedDate;
                    }
                    else
                    {
                        SelectedCalenderDate = selectedDate;
                    }

                }
                else
                {
                    if (utilityMethods.IsDateInBetweenIncludingEndPoints(SelectedCalenderDate, SelectedSecondCalenderDate, selectedDate))
                    {
                        if (curretDateDayDifference < secondDateDayDifference)
                        {
                            //Assign new values
                            SelectedCalenderDate = selectedDate;
                        }
                        else
                        {
                            //Assign new values
                            SelectedSecondCalenderDate = selectedDate;
                        }
                    }
                    else
                    {
                        //Checking Selected date is greater then already selected two dates
                        if (selectedDate > SelectedCalenderDate)
                        {
                            //Assign new values
                            SelectedSecondCalenderDate = selectedDate;


                        }
                        else
                        {
                            //Assign new values
                            SelectedCalenderDate = selectedDate;
                        }
                    }

                }
                //Final Checking SelectedCalenderDate should be less then SelectedSecondCalenderDate
                //Otherwise replace the values
                if (SelectedCalenderDate > SelectedSecondCalenderDate)
                {
                    var intialSecondSelectedCalenderDate = SelectedSecondCalenderDate;
                    SelectedSecondCalenderDate = SelectedCalenderDate;
                    SelectedCalenderDate = intialSecondSelectedCalenderDate;
                }
            }
            catch (Exception ex)
            {
                HandleException("SettingCalendarDates", ex.Message);

            }

        }
        /// <summary>
        /// Toggle PULL to Refresh Feature: - In which we will display only Data having visitState 'Done', 'In-Progress'
        /// So you have idea that pull to refresh is working fine 
        /// And when again pull to refresh we will display total items of list from source object
        /// Hense acheving Toggle Refresh feature on each pull to refresh
        /// </summary>
        private void ExecuteRefreshCommand()
        {
            try
            {
                if (IsRefreshing)
                    return;
                IsExecuting = true;
                int itemCount = CarFitDataCollection.Count;
                //Clear Collection filteredCarfitData List Data
                ClearCollectionsData();
                //Get Filter data based on PUlL ReFresh Feature Data having "Done" Status only 
                foreach (var item in source)
                {
                    //If list having all itesms then we have to show filterd item on pull refresh Data having "Done" Status
                    if ((itemCount == source.Count))
                    {
                        if ((item.visitState == "Done"))
                            filteredCarfitData.Add(item);
                    }
                    else
                    {
                        //If list is already filterd then on refresh we will show all iteams again in list
                        filteredCarfitData.Add(item);
                    }
                }

                //Adding Final Filtered Data in CarFitDataCollection after Calculating Distance between them if we have data with only visitStatus = Done
                ObservableCollection<Data> distanceCalculatedCarFitData = (itemCount == source.Count) ? utilityMethods.CalculateDistance(filteredCarfitData) : filteredCarfitData;
                foreach (var item in distanceCalculatedCarFitData)
                {
                    CarFitDataCollection.Add(item);
                }
                // Stop refreshing
                IsRefreshing = false;
                // Stop Indicator
                IsExecuting = false;
            }
            catch (Exception ex)
            {
                HandleException("ExecuteRefreshCommand", ex.Message, true);

            }
        }
        /// <summary>
        /// Below method will hides the header bar and display Calender View at that place
        /// Also we will display current month calender dates once calender is opened
        /// </summary>
        private void ExecuteShowDialogCommand()
        {
            try
            {
                //Load the calender first time with current month and year
                //And by selecting today's date by default selected
                List<DateTime> dates = utilityMethods.GetDates(currentDate.Year, currentDate.Month);
                //Load the Calender View Based on above dates
                LoadCalenderViewDates(dates);
                HeaderVisibility = false;
                CalenderVisibility = true;

            }
            catch (Exception ex)
            {
                HandleException("ExecuteShowDialogCommand", ex.Message);
            }
        }
        /// <summary>
        //Below Method will load the calender dates
        /// </summary>
        private void LoadCalenderViewDates(List<DateTime> calenderDates)
        {
            try
            {
                //Update Calender Title with Month and Year
                CalenderTitle = currentDate.ToString("MMMM").Substring(0, 3).ToUpper() + " " + currentDate.Year;

                //Check if already data is there then clear it first before loading new dates 
                if (CalenderView.Children.Any())
                    CalenderView.Children.Clear();
                CalenderView.Orientation = StackOrientation.Horizontal;
                foreach (var calenderDate in calenderDates)
                {
                    StackLayout stackLayout = new StackLayout() { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                    stackLayout.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = HandleCalendarDateSelectionCommand,
                        CommandParameter = calenderDate.Day.ToString(),
                        NumberOfTapsRequired = 1
                    });
                    Grid grid = new Grid();
                    grid.VerticalOptions = LayoutOptions.FillAndExpand;
                    grid.HorizontalOptions = LayoutOptions.FillAndExpand;
                    grid.Style = (Style)Application.Current.Resources["CalDatePad"];
                    VisualStateManager.GoToState(grid, State);
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });

                    Frame view = new Frame();
                    view.HorizontalOptions = LayoutOptions.FillAndExpand;
                    view.VerticalOptions = LayoutOptions.FillAndExpand;
                    view.HeightRequest = 30;
                    view.WidthRequest = 30;
                    view.Margin = 0;
                    view.Padding = 0;
                    view.HasShadow = false;
                    view.BackgroundColor = Color.Transparent;
                    //If it matches with current date then set background color
                    if (((calenderDate.Year == SelectedCalenderDate.Year) && (calenderDate.Month == SelectedCalenderDate.Month) && (calenderDate.Day == SelectedCalenderDate.Day)) || ((calenderDate.Year == SelectedSecondCalenderDate.Year) && (calenderDate.Month == SelectedSecondCalenderDate.Month) && (calenderDate.Day == SelectedSecondCalenderDate.Day)))
                    {
                        view.CornerRadius = 15;
                        view.BackgroundColor = Color.FromHex("#368268");
                    }
                    Label dateLabel = new Label
                    {
                        FontFamily = Application.Current.Resources["BoldFont"] as string,
                        Style = (Style)Application.Current.Resources["CalDateText"],
                        Text = calenderDate.Day.ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        Padding = 0,
                        TextColor = (Color)Application.Current.Resources["ColorBluishGrey"],
                        Margin = 0
                    };
                    VisualStateManager.GoToState(dateLabel, State);
                    view.Content = dateLabel;

                    Grid.SetColumn(view, 0);
                    Grid.SetRow(view, 0);
                    grid.Children.Add(view);

                    Label dayLabel = new Label
                    {
                        FontFamily = Application.Current.Resources["RegularFont"] as string,
                        Style = (Style)Application.Current.Resources["NormalText11Cal"],
                        Text = calenderDate.DayOfWeek.ToString().Substring(0, 3),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        TextColor = (Color)Application.Current.Resources["ColorBluishGrey"]
                    };
                    VisualStateManager.GoToState(dayLabel, State);
                    Grid.SetColumn(dayLabel, 0);
                    Grid.SetRow(dayLabel, 1);
                    grid.Children.Add(dayLabel);

                    //Adding Grid to Stack Layout
                    stackLayout.Children.Add(grid);

                    //Adding StackLayout to Main StackLayout
                    CalenderView.Children.Add(stackLayout);
                }
            }
            catch (Exception ex)
            {
                HandleException("LoadCalenderViewDates", ex.Message);
            }
        }
        /// <summary>
        /// Main method which used to update some of Data class properties based on our requirement before display it on UI list
        /// Handle null cheeck and if value is there then only assign that property value otherwise set as blank value so in UI it will not displyed
        /// </summary>
        private Data UpdateCarFitData(Data car, List<Data> carFitData)
        {
            try
            {
                //Setting Button Background Color Theme
                car.backgroundTheme = utilityMethods.GetBackgroundThemColor((car.visitState));
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

                //Adding Final Data Object in to source
                source.Add(car);
                carFitCounter++;

            }
            catch (Exception ex)
            {
                HandleException("updateCarFitData", ex.Message);
            }
            return car;
        }
        /// <summary>
        /// Common Handle Exception Method
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="hideIndicator"></param>
        private void HandleException(string methodName, string exceptionMessage, bool hideIndicator = false)
        {
            if (hideIndicator)
                IsExecuting = false;

            Console.WriteLine("Exception At " + methodName + " : {0}", exceptionMessage);
        }
        #endregion
    }
}
