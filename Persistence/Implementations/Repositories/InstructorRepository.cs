using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Paging;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class InstructorRepository : BaseRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IList<Instructor>> GetInstructorsByCourse(Guid courseId)
        {
            return await _context.Instructors
                .Include(s => s.InstructorCourses)
                .ThenInclude(f => f.Course)
                .Where(g => g.InstructorCourses.Any(h => h.CourseId == courseId)).ToListAsync();
        }

        public async Task<IEnumerable<Instructor>> GetSelectedInstructors(IList<Guid> ids)
        {
            return await _context.Instructors.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText)
        {
            return await _context.Instructors.Where(instructor => EF.Functions.Like(instructor.FirstName, $"%{searchText}%")).ToListAsync();
        }

        public async Task<PaginatedList<InstructorDTO>> LoadInstructorsAsync(string filter, int page, int limit)
        {
            var instructors = await _context.Instructors.Include(i => i.InstructorCourses).ThenInclude(ic => ic.Course).ToListAsync();
            return await instructors.AsQueryable()
                .Where(c => filter == null || c.FirstName.Contains(filter))
                .Select(n => new InstructorDTO
                {
                    Id = n.Id,
                    FirstName = n.FirstName,
                    LastName = n.LastName,
                    Email = n.Email,
                    InstructorPhoto = n.InstructorPhoto,
                    PhoneNumber = n.PhoneNumber,
                    InstructorLMSCode = n.InstructorLMSCode,
                    InstructorCourses = n.InstructorCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                    }).ToList()

                }).ToPaginatedListAsync(page,
                                          limit);
        
        }

       /* public async Task<List<Instructor>> NewLoadInstructorsAsync()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var instructorData = await _context.Instructors.ToListAsync();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                instructorData = instructorData.Where(m => m.FirstName.Contains(searchValue)
                                            || m.LastName.Contains(searchValue)
                                            || m.Email.Contains(searchValue));
            }
            recordsTotal = instructorData.Count();
            var data = instructorData.Skip(skip).Take(pageSize).ToList();
        }*/
    }
}
