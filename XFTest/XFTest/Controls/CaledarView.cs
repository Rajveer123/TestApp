using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using XFTest.Helper;

namespace XFTest.Controls
{
    public class CaledarView : ContentView
    {
        #region Private Properties and Variable
        Label calendarTitelLabl = new Label();
        StackLayout stackLayout = new StackLayout();
        private DateTime currentDate = DateTime.Now;
        UtilityMethods utilityMethods = new UtilityMethods();
        StackLayout calenderView = new StackLayout();
        #endregion


        #region Calendar Bindable Properties
        /// <summary>
        /// First calender selected date bindable property
        /// </summary>
        public static readonly BindableProperty SelectedCalenderDateProperty = BindableProperty.Create("SelectedCalenderDate", typeof(DateTime), typeof(CaledarView), DateTime.Now, BindingMode.TwoWay);
        public DateTime SelectedCalenderDate
        {
            get { return (DateTime)GetValue(SelectedCalenderDateProperty); }
            set { SetValue(SelectedCalenderDateProperty, value); }
        }
        /// <summary>
        /// Second calender selected date bindable property
        /// </summary>
        public static readonly BindableProperty SelectedSecondCalenderDateProperty = BindableProperty.Create("SelectedSecondCalenderDate", typeof(DateTime), typeof(CaledarView), DateTime.Now, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var calendarControl = bindable as CaledarView;
        });
        public DateTime SelectedSecondCalenderDate
        {
            get { return (DateTime)GetValue(SelectedSecondCalenderDateProperty); }
            set { SetValue(SelectedSecondCalenderDateProperty, value); }
        }
        /// <summary>
        /// Header visibility  bindable property
        /// </summary>
        public static readonly BindableProperty HeaderVisibilityProperty = BindableProperty.Create("HeaderVisibility", typeof(bool), typeof(CaledarView), default(bool), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var calendarControl = bindable as CaledarView;
            calendarControl.IsVisible = !calendarControl.HeaderVisibility;
        });
        public bool HeaderVisibility
        {
            get { return (bool)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }
        /// <summary>
        /// Calender visibility  bindable property
        /// </summary>
        public static readonly BindableProperty CalenderVisibilityProperty = BindableProperty.Create("CalenderVisibility", typeof(bool), typeof(CaledarView), default(bool), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var calendarControl = bindable as CaledarView;
            calendarControl.IsVisible = calendarControl.CalenderVisibility;
        });
        public bool CalenderVisibility
        {
            get { return (bool)GetValue(CalenderVisibilityProperty); }
            set { SetValue(CalenderVisibilityProperty, value); }
        }
        /// <summary>
        /// Select Calendar Date Command  bindable property
        /// </summary>
        public static readonly BindableProperty SelectCalendarDateCommandProperty =
        BindableProperty.Create("SelectCalendarDateCommand", typeof(ICommand), typeof(CaledarView), default(ICommand));
        public ICommand SelectCalendarDateCommand
        {
            get { return (ICommand)GetValue(SelectCalendarDateCommandProperty); }
            set { SetValue(SelectCalendarDateCommandProperty, value); }
        }
        public static readonly BindableProperty SelectCalendarDateCommandParameterProperty = BindableProperty.Create("SelectCalendarDateCommandParameter", typeof(object), typeof(CaledarView));
        public object SelectCalendarDateCommandParameter
        {
            get { return GetValue(SelectCalendarDateCommandParameterProperty); }
            set { SetValue(SelectCalendarDateCommandParameterProperty, value); }
        }
        public string State { get; set; } = "Large";
        #endregion


        #region Constructor
        public CaledarView()
        {
            try
            {
                LoadControl();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At CaledarView Constructor  : {0}", ex.Message);
            }

        }
        #endregion

        #region Private Methods
        private ScrollView LoadCalendarView()
        {
            ScrollView scrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("#25A87B"),
                Orientation = ScrollOrientation.Horizontal,
                Padding = new Thickness(0)
            };
            calenderView.HorizontalOptions = LayoutOptions.FillAndExpand;
            calenderView.VerticalOptions = LayoutOptions.StartAndExpand;
            //Load the calenderview dates first time with current month and year
            LoadCalenderDates();
            //Once calenderView loads all dates add it in to scrollview
            scrollView.Content = calenderView;
            return scrollView;
        }
        /// <summary>
        /// Below mthod will load calendar title and next / previous buttons UI
        /// </summary>
        /// <returns></returns>
        private Grid LoadCalendarTopView()
        {
            Grid calendarTopViewGrid = new Grid { Padding = new Thickness(0, 0, 0, 10) };
            calendarTopViewGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            calendarTopViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            calendarTopViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(.5, GridUnitType.Star) });
            calendarTopViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(.5, GridUnitType.Star) });

            calendarTitelLabl.Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 55, 0, 0) : new Thickness(0, 30, 0, 0);
            calendarTitelLabl.Style = (Style)Application.Current.Resources["PageHeading"];
            calendarTitelLabl.HorizontalTextAlignment = TextAlignment.Start;


            //Apply FontFamily from StaticRsource
            var OnPlatformDic = (OnPlatform<string>)App.Current.Resources["BoldFont"];
            var fontFamily = OnPlatformDic.Platforms.FirstOrDefault((arg) => arg.Platform.FirstOrDefault() == Device.RuntimePlatform).Value;
            calendarTitelLabl.FontFamily = fontFamily.ToString();

            calendarTitelLabl.FontSize = Device.RuntimePlatform == Device.iOS ? 20 : 23;


            calendarTitelLabl.TextColor = Color.FromHex("#FFFFFF");
            Grid.SetColumn(calendarTitelLabl, 0);
            Grid.SetRow(calendarTitelLabl, 0);
            calendarTopViewGrid.Children.Add(calendarTitelLabl);

            SvgCachedImage leftArrowImage = new SvgCachedImage
            {
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 60, 0, 0) : new Thickness(0, 30, 0, 0),
                Source = "Arrow_Left.svg",
                WidthRequest = 12,
                HorizontalOptions = LayoutOptions.End
            };
            // Handle Arrow Command of calender view to load  previous month calender
            var imgTapped = new TapGestureRecognizer();
            imgTapped.Tapped += (ea, sa) =>
            {
                DateTime newCalenderMonth = currentDate.AddMonths(-1);
                //Setting modified date in currentDate again
                currentDate = newCalenderMonth;
                //Load the calenderview dates.
                LoadCalenderDates();
            };
            leftArrowImage.GestureRecognizers.Add(imgTapped);
            Grid.SetColumn(leftArrowImage, 1);
            Grid.SetRow(leftArrowImage, 0);
            calendarTopViewGrid.Children.Add(leftArrowImage);

            SvgCachedImage rightArrowImage = new SvgCachedImage
            {
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 60, 0, 0) : new Thickness(0, 30, 0, 0),
                Source = "Arrow_Right.svg",
                WidthRequest = 12,
                HorizontalOptions = LayoutOptions.End
            };
            // Handle Arrow Command of calender view to load  next month calender
            var rightImgTapped = new TapGestureRecognizer();
            rightImgTapped.Tapped += (ea, sa) =>
            {
                DateTime newCalenderMonth = currentDate.AddMonths(+1);
                //Setting modified date in currentDate again
                currentDate = newCalenderMonth;
                //Load the calenderview dates.
                LoadCalenderDates();
            };
            rightArrowImage.GestureRecognizers.Add(rightImgTapped);
            Grid.SetColumn(rightArrowImage, 2);
            Grid.SetRow(rightArrowImage, 0);
            calendarTopViewGrid.Children.Add(rightArrowImage);
            return calendarTopViewGrid;
        }
        /// <summary>
        //Below Method will load the calender control UI
        /// </summary>
        private void LoadControl()
        {
            if (stackLayout.Children.Any())
            {
                //Clear All UI Elemnts from StackLayout
                stackLayout.Children.Clear();
            }
            Grid parntGrid = new Grid();
            parntGrid.Padding = new Thickness(0);
            parntGrid.BackgroundColor = Color.FromHex("#25A87B");
            parntGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            parntGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            StackLayout innerStackLayout = new StackLayout { Padding = new Thickness(20) };
            //Load Calender Top View UI
            innerStackLayout.Children.Add(LoadCalendarTopView());
            //Load Calender Dates View UI
            innerStackLayout.Children.Add(LoadCalendarView());
            Grid.SetColumn(innerStackLayout, 0);
            Grid.SetRow(innerStackLayout, 0);
            parntGrid.Children.Add(innerStackLayout);
            stackLayout.Children.Add(parntGrid);
            this.Content = stackLayout;
        }
        /// <summary>
        //Below Method will load the calender dates with in stacklayout
        /// </summary>
        private void LoadCalenderViewDates(List<DateTime> calenderDates)
        {
            try
            {
                //Update Calender Title with Month and Year
                calendarTitelLabl.Text = currentDate.ToString("MMMM").Substring(0, 3).ToUpper() + " " + currentDate.Year;

                //Check if already data is there then clear it first before loading new dates 
                if (calenderView.Children.Any())
                    calenderView.Children.Clear();
                calenderView.Orientation = StackOrientation.Horizontal;
                foreach (var calenderDate in calenderDates)
                {
                    StackLayout stackLayout = new StackLayout() { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                    var stackLayoutTapped = new TapGestureRecognizer();
                    //Below code executes when user user selects any date from calendar
                    stackLayoutTapped.Tapped += (ea, sa) =>
                    {
                        //Call Mthod to Handle Calender Date Selection 
                        HandleCalenderDateSelection(calenderDate);
                    };
                    stackLayout.GestureRecognizers.Add(stackLayoutTapped);

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
                    calenderView.Children.Add(stackLayout);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At LoadCalenderViewDates  : {0}", ex.Message);
            }
        }
        /// <summary>
        /// Below funcation contains Both Calendar Dates Selection Logic
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
                Console.WriteLine("Exception At SettingCalendarDates  : {0}", ex.Message);

            }

        }
        /// <summary>
        /// Gets Month Dates and Load Calender Dates with in UI
        /// </summary>
        private void LoadCalenderDates()
        {
            try
            {
                //Get the calender dates from currentDate object
                List<DateTime> dates = utilityMethods.GetDates(currentDate.Year, currentDate.Month);
                //Load the Calender View Based on above dates
                LoadCalenderViewDates(dates);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At LoadCalenderDates  : {0}", ex.Message);
            }
        }
        /// <summary>
        /// Handle Calender Date Selection Logic
        /// </summary>
        private void HandleCalenderDateSelection(DateTime calenderDate)
        {
            try
            {
                //Show HaderView and Hide Calnderview after date selection
                HeaderVisibility = true;
                CalenderVisibility = false;
                SelectCalendarDateCommandParameter = calenderDate.Day.ToString();
                DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, Convert.ToInt32(SelectCalendarDateCommandParameter));
                //If same date selected from calender no need to do further operation
                if ((SelectedCalenderDate.Year == selectedDate.Year && SelectedCalenderDate.Month == selectedDate.Month && SelectedCalenderDate.Day == selectedDate.Day) || (SelectedSecondCalenderDate.Year == selectedDate.Year && SelectedSecondCalenderDate.Month == selectedDate.Month && SelectedSecondCalenderDate.Day == selectedDate.Day))
                {
                    return;
                }
                else
                {
                    //Update the selected calender dates value based on user date selection
                    //So next time when calender gets open it shows the last selected dates
                    //As per logic it only displays latest selected two dates
                    //Perform Calender Multipl Dates Selction Logic
                    SettingCalendarDates(selectedDate);
                    //Load the calenderview dates.
                    LoadCalenderDates();
                }
                //Execute Command to load data based on selected two dates
                if (SelectCalendarDateCommand != null && SelectCalendarDateCommand.CanExecute(null))
                    SelectCalendarDateCommand.Execute(SelectCalendarDateCommandParameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At HandleCalenderDateSelection  : {0}", ex.Message);
            }
        }
        #endregion
    }
}
