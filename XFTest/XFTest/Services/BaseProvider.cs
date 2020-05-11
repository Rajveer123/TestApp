using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XFTest.Models;
using XFTest.ViewModels;

namespace XFTest.Services
{
    public class BaseProvider
    {
        //Below Method will Read Local JSON Data and Return it Back
        protected List<Data> Get<T>()
        {
            string jsonFileName = "CarFitData.json";
            var assembly = typeof(BaseProvider).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<CarFitRecords>(jsonString).data;
            }
        }
    }
}
