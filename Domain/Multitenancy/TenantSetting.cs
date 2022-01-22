using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Multitenancy
{
    public class TenantSetting
    {
        public TenantConfiguration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }

}

