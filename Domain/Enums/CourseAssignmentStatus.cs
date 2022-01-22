using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CourseAssignmentStatus
    {
        Requested = 1,
        IsAssignedInitiatedByAdmin,
        IsAssignedInitiatedByLearner
    }
}
