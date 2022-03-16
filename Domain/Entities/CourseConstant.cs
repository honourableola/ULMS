using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class CourseConstant : BaseEntity, ITenant
    {
        public int MaximumNoOfMajorCourses { get; set; }
        public int MaximumNoOfAdditionalCourses { get; set; }
        public int NoOfAssessmentQuestions { get; set; }
        public TimeSpan DurationOfAssessment { get; set; }
        public string TenantId { get; set; }
    }
}
