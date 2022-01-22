using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Multitenancy
{
    public class TenantConfiguration
    {
        public string DBProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
