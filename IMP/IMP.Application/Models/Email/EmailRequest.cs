﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Models.Email
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string AttactmentFile { get; set; }
    }
}
