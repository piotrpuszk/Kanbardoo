﻿using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly DBContext _dbContext;

    public UserRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.ID== id);

        if (user is null)
        {
            return new User();
        }

        return user;
    }

    public async Task<User> GetAsync(string name)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Name == name);

        if (user is null)
        {
            return new User();
        }

        return user;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }
}