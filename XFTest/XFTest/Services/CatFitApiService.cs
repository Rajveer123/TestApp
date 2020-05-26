using System;
using System.Collections.Generic;
using XFTest.Interface;
using XFTest.Models;

namespace XFTest.Services
{
    public class CatFitApiService : BaseProvider, ICarFitApiService
    {

        public List<Data> GetCarFitOrders()
        {
            try
            {
                return Get<List<Data>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception At GetCarFitOrders Method for Reading Carfit Data from Local Json: {0}", ex.Message);
                return new List<Data>();
            }

        }


    }
}
