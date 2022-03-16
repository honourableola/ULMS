using System.Collections.Generic;

namespace Domain.Multitenancy
{
    public class TenantSetting
    {
        public TenantConfiguration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }

}

