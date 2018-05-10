using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomChartData<T>
    {
        public string xAxis { get; set; }
        public T yAxisAndroid { get; set; }
        public T yAxisIphone { get; set; }
    }
}
