using IMP.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Filters
{
    public class RequestParameter
    {
        [FromQuery(Name = "page_number")]
        public int PageNumber { get; set; }
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }

        [FromQuery(Name = "includes")]
        public List<string> Includes { get; set; }
        [FromQuery(Name = "order_field")]
        public string OrderField { get; set; }
        [FromQuery(Name = "order_by")]
        public OrderBy OrderBy { get; set; }
        public RequestParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public RequestParameter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
