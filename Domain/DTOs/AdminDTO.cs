using System;

namespace Domain.DTOs
{
    public class AdminDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AdminPhoto { get; set; }
        public Guid UserId { get; set; }
    }
}
