﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Filters
{
    public class RequestParameter
    {
        [FromForm(Name = "page_number")]
        public int PageNumber { get; set; }
        [FromForm(Name = "page_size")]
        public int PageSize { get; set; }
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
