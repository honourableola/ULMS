﻿using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}