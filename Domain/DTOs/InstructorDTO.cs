﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class InstructorDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string InstructorPhoto { get; set; }
        public string InstructorLMSCode { get; set; }
        public ICollection<CourseDTO> InstructorCourses { get; set; } = new List<CourseDTO>();
    }
}
