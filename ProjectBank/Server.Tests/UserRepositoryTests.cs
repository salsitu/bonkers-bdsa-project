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
            IsSupervisor = true,
            Email = "paolo@"
        });
        context.Users.Add(new User
        {
            Id = 2,
            Name = "jakob",
            IsSupervisor = false,
            Email = "jakob@"
        });
        context.SaveChanges();
        _context = context;
        _repo = new UserRepository(_context);
    }
    [Fact]
    public async Task CreateUserAsync_returns_created_and_new_user()
    {
        var user = new UserCreateDTO("Karl", false, "Email@mei.dk");
    
        var created = await _repo.CreateUserAsync(user);
    
        Assert.Equal((Created, new UserDTO(3, "Karl", false, "Email@mei.dk")), created);
    }
    [Fact]
    public async Task CreateUserAsync_given_existing_User_returns_Conflict_and_user()
    {
        var user = new UserCreateDTO("jakob", false, "jakob@");

        var created = await _repo.CreateUserAsync(user);

        Assert.Equal((Conflict, new UserDTO(2, "jakob", false, "jakob@")), created);
    }
    [Fact]
    public async Task GetUserAsync_returns_requested_user()
    {
        var user = await _repo.GetUserAsync(2);

        Assert.Equal(new UserDTO(2, "jakob", false, "jakob@"), user);
    }
    [Fact]
    public async Task GetUserAsync_returns_null_if_user_doesnt_exists()
    {
        var user = await _repo.GetUserAsync(4);

        Assert.Null(user);
    }
    [Fact]
    public async Task GetUserWithEmailAsync_returns_requested_user()
    {
        var user = await _repo.GetUserWithEmailAsync("jakob@");

        Assert.Equal(new UserDTO(2, "jakob", false, "jakob@"), user);
    }
    [Fact]
    public async Task GetUserWithEmailAsync_returns_null_if_user_doesnt_exists()
    {
        var user = await _repo.GetUserWithEmailAsync("whateven@");

        Assert.Null(user);
    }
}