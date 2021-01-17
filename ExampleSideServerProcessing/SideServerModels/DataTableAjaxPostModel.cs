using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleSideServerProcessing.SideServerModels
{
    public class DataTableAjaxPostModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        //public List<Column> columns { get; set; }
       //public Search search { get; set; }
       //public List<Order> order { get; set; }
    }
}