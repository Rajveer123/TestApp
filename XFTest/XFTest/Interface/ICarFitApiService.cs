using System.Collections.Generic;
using XFTest.Models;

namespace XFTest.Interface
{
    public interface ICarFitApiService
    {
        List<Data> GetCarFitOrders();
    }
}
