using System;
using System.Linq;

namespace XFTest.Helper
{
    public class UtilityMethods
    {
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
            switch (state)
            {
                case "ToDo":
                    return "#4E77D6";
                case "InProgress":
                    return "#F5C709";
                case "Done":
                    return "#25A87B";
                default:
                    return "#EF6565";
            }
        }
    }
}
