﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Multitenancy
{
    public interface ITenant
    {
        public string TenantId { get; set; }
    }
}
