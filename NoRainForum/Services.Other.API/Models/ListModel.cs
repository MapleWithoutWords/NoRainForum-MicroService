using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Other.API.Models
{
    public class ListModel<T>
    {
        public List<T> Datas { get; set; }
        public long TotalCount { get; set; }
    }
}
