using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XFTest.Interface;
using XFTest.Models;

namespace XFTest.Services
{
    public class CatFitApiService : BaseProvider, ICarFitApiService
    {

        public List<Data> GetCarFitOrders()
        {
            return Get<List<Data>>();
        }


    }
}
