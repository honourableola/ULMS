using Domain.Entities;
using System;

namespace Domain.DTOs
{
    public class TopicDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Module Module { get; set; }
        public bool IsTaken { get; set; }
    }
}
