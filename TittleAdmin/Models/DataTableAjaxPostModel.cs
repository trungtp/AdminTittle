using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TittleAdmin.Model.DTO;

namespace TittleAdmin.Models
{
    public class DataTableAjaxPostModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
        public string CustomData { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public List<string> sort_by { get; set; }
        public string promoCode { get; set; }
        public string plan { get; set; }
        public string action { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class DataTableResult<T>
    {
        public IList<T> result { get; set; }
        public int filteredResultsCount { get; set; }
        public int totalResultsCount { get; set; }
        public int iosCount { get; set; }
        public int androidCount { get; set; }
        public string countryGrouping { get; set; }
    }
}