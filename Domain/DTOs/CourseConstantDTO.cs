using System;

namespace Domain.DTOs
{
    public class CourseConstantDTO
    {
        public Guid Id { get; set; }
        public int MaximumNoOfMajorCourses { get; set; }
        public int MaximumNoOfAdditionalCourses { get; set; }
        public int NoOfAssessmentQuestions { get; set; }
        public TimeSpan DurationOfAssessment { get; set; }
    }
}
