using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CourseRequestDTO
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid LearnerId { get; set; }
        public Learner Learner { get; set; }
        public string RequestMessage { get; set; }
    }
}
