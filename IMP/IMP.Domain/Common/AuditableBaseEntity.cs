﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
    }
}
