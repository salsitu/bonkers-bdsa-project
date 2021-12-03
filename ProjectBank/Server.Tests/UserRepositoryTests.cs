using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Server;
using ProjectBank.Server.Entities;
using static ProjectBank.Server.Entities.Response;
using Xunit;

namespace Server.Tests;

public class UserRepositoryTests
{
    private readonly ProjectBankContext _context;
    private readonly UserRepository _repo;
    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.Projects.Add(new Project
        {
            Id = 1,
            Name = "huhu",
            Description = "jaja",
            AuthorId = 1
        });
        context.Users.Add(new User
        {
            Id = 1,
            Name = "paolo",
            IsSupervisor = true
        });
        context.Users.Add(new User
        {
            Id = 2,
            Name = "jakob",
            IsSupervisor = false
        });
        context.SaveChanges();
        _context = context;
        _repo = new UserRepository(_context);
    }
    //[Fact]
    //public async Task CreateUserAsync_returns_created_and_new_user()
    //{
    //    var project = new UserCreateDTO("Karl", false);
    //
    //    var created = await _repo.CreateUserAsync(project);
    //
    //    Assert.Equal((Created, new UserDTO(3, "Karl", false)), created);
    //}
    [Fact]
    public async Task GetUserAsync_returns_requested_user()
    {
        var user = await _repo.GetUserAsync(2);

        Assert.Equal(new UserDTO(2, "jakob", false), user);
    }
    [Fact]
    public async Task GetUserAsync_returns_null_if_user_doesnt_exists()
    {
        var user = await _repo.GetUserAsync(4);

        Assert.Null(user);
    }
}