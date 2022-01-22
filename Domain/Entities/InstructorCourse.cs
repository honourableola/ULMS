﻿using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InstructorCourse : BaseEntity, ITenant
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string TenantId { get; set; }
    }
}
