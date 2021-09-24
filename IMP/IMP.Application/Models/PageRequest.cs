using IMP.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models
{
    public class PageRequest
    {
        [FromQuery(Name = "page_index")]
        public int PageIndex { get; set; }
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }
        [FromQuery(Name = "order_field")]
        public string OrderField { get; set; }
        [FromQuery(Name = "order_by")]
        public OrderBy OrderBy { get; set; }
    }
}
