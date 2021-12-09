using IMP.Application.Enums;
using IMP.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models
{
    public class PageRequest
    {
        [FromQuery(Name = "page_index")]
        [DefaultValue(0)]
        public int PageIndex { get; set; }
        [FromQuery(Name = "page_size")]
        [DefaultValue(20)]
        public int PageSize { get; set; }
        [FromQuery(Name = "order_field")]
        public string OrderField { get; set; }
        [FromQuery(Name = "order_by")]
        public OrderBy OrderBy { get; set; }
        public PageRequest()
        {
        }

    }
}
