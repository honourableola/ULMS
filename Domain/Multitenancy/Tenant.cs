using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Multitenancy
{
    public class Tenant
    {
        public string Name { get; set; }
        public string TID { get; set; }
        public string ConnectionString { get; set; }
    }
}
