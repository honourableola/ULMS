
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULMS.Domain.Interfaces.Audit;

namespace Domain.Entities
{
    public abstract class BaseEntity : IAuditBase, ISoftDeletable
    {
        public Guid Id { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; } 
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
