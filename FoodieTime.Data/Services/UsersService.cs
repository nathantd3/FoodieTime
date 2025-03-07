﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodieTime.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodieTime.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _appDbContext;

        public UsersService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
        }
    }
}
