using System;
using System.Collections.Generic;

namespace XFTest.Models
{
    public class CarFitRecords
    {
        public List<Data> data { get; set; }
    }
    public class Tasks
    {
        public string taskId { get; set; }
        public string title { get; set; }
        public bool isTemplate { get; set; }
        public int timesInMinutes { get; set; }
        public double price { get; set; }
        public string paymentTypeId { get; set; }
        public DateTime createDateUtc { get; set; }
        public DateTime lastUpdateDateUtc { get; set; }

    }
    public class Data
    {
        public string visitId { get; set; }
        public string homeBobEmployeeId { get; set; }
        public string houseOwnerId { get; set; }
        public bool isBlocked { get; set; }
        public DateTime startTimeUtc { get; set; }
        public string startTime { get; set; }
        public DateTime endTimeUtc { get; set; }
        public string title { get; set; }
        public bool isReviewed { get; set; }
        public bool isFirstVisit { get; set; }
        public bool isManual { get; set; }
        public int visitTimeUsed { get; set; }
        public string houseOwnerFirstName { get; set; }
        public string houseOwnerLastName { get; set; }
        public string houseOwnerMobilePhone { get; set; }
        public string houseOwnerAddress { get; set; }
        public string houseOwnerZip { get; set; }
        public string houseOwnerCity { get; set; }
        public string houseAddress { get; set; }
        public double houseOwnerLatitude { get; set; }
        public double houseOwnerLongitude { get; set; }
        public bool isSubscriber { get; set; }
        public string professional { get; set; }
        public string visitState { get; set; }
        public int stateOrder { get; set; }
        public string expectedTime { get; set; }
        public string expectedTimeDisplay { get; set; }
        public string tasksTotalTime { get; set; }
        public List<Tasks> tasks { get; set; }

    }
}
