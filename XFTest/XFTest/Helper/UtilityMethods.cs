using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XFTest.Enums;
using XFTest.Models;

namespace XFTest.Helper
{
    public class UtilityMethods
    {
        const char DISTANCE_CALCULATION_UNIT = 'K';
        const string TODO_COLOR = "#4E77D6";
        const string INPROGRESS_COLOR = "#F5C709";
        const string DONE_COLOR = "#25A87B";
        const string REJECTED_COLOR = "#EF6565";
        //Time formate we are going to use for displaying startTime
        const string TIME_FORMATE = "HH:MM";
        /// <summary>
        //Method to get Distance in km based on lat long values
        //lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  
        //    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  
        //    unit = the unit you desire for results                               
        //           where: 'M' is statute miles (default)                         
        //                  'K' is kilometers                                      
        //                  'N' is nautical miles 
        /// </summary>
        public double GetDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }
        //<summary>
        ///This function converts decimal degrees to radians             
        //</summary>
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        //<summary>
        ///This function converts radians to decimal degrees              
        //</summary>
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        /// <summary>
        //Handle null cheeck and if value is there then only display it in UI otherwise display as blank
        /// </summary>
        public string GetFullAddress(string s1, string s2, string s3)
        {
            string[] addressArray = { s1, s2, s3 };
            return string.Join(" ", addressArray.Where(s => !string.IsNullOrEmpty(s)));
        }
        ///<summary>
        ///Method which return button theme color based visitState
        ///</summary>
        public string GetBackgroundThemColor(string state)
        {
            switch (EnumHelper.Parse<VisitStates>(state))
            {
                case VisitStates.ToDo:
                    return TODO_COLOR;
                case VisitStates.InProgress:
                    return INPROGRESS_COLOR;
                case VisitStates.Done:
                    return DONE_COLOR;
                default:
                    return REJECTED_COLOR;
            }
        }
        ///<summary>
        ///Method which return dates based on year and month we passed
        ///</summary>
        public List<DateTime> GetDates(int year, int month)
        {
            var days = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                days.Add(new DateTime(year, month, i));
            }
            return days;
        }
        ///<summary>
        ///Method which calculates the distance between places
        ///For first place we are seetting 0 km as it is starting point
        ///For rest of other points it will calculate distance adjustance to each other i.e 0-1, 1-2,2-3...
        ///</summary>
        public ObservableCollection<Data> CalculateDistance(ObservableCollection<Data> carFitListData)
        {
            int carFitCounter = 0;
            foreach (var car in carFitListData)
            {
                if (carFitCounter == 0)
                {
                    car.distance = "0.0 km";
                    carFitCounter++;
                    continue;
                }
                else
                {
                    //Comparing distance for two subsequest places i.e 1-2, 2-3,4-5...9-10
                    //Allow only one digit after decimal for displaying value of distance
                    car.distance = string.Format("{0:0.0}", (GetDistance(carFitListData[carFitCounter - 1].houseOwnerLatitude, carFitListData[carFitCounter - 1].houseOwnerLongitude, carFitListData[carFitCounter].houseOwnerLatitude, carFitListData[carFitCounter].houseOwnerLongitude, DISTANCE_CALCULATION_UNIT))) + " km";
                }

                carFitCounter++;
            }
            return carFitListData;
        }
        /// <summary>
        /// Methods knows that given date is in between startDate and endDate or not
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsDateInBetweenIncludingEndPoints(DateTime startDate, DateTime endDate, DateTime date)
        {
            if (date >= startDate && date <= endDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Method return format
        /// </summary>
        /// <returns></returns>
        public string GetDatFormat()
        {
            return TIME_FORMATE;
        }
    }
}
