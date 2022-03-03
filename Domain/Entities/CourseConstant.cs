using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CourseConstant : BaseEntity, ITenant
    {
        public int MaximumNoOfMajorCourses { get; set; }
        public int MaximumNoOfAdditionalCourses { get; set; }
        public int NoOfAssessmentQuestions { get; set; }
        public string TenantId { get; set; }
    }
}
