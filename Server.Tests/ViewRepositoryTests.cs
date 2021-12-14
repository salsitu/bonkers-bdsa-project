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

public class ViewRepositoryTests
{
    private readonly ProjectBankContext _context;
    private readonly ViewRepository _repo;
    public ViewRepositoryTests()
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
        context.Projects.Add(new Project
        {
            Id = 2,
            Name = "hihi",
            Description = "dada",
            AuthorId = 2
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
        context.Users.Add(new User
        {
            Id = 3,
            Name = "troels",
            IsSupervisor = false,
            Email = "troels@"
        });
        context.Views.Add(new View
        {
            ProjectId = 1,
            StudentId = 2
        });
        context.Views.Add(new View
        {
            ProjectId = 1,
            StudentId = 5
        });
        context.SaveChanges();
        _context = context;
        _repo = new ViewRepository(_context);
    }

    [Fact]
    public async Task AddViewAsync_returns_Created_when_new_view()
    {
        var response= await _repo.AddViewAsync(2, 3);

        Assert.Equal(Created, response);
    }
    [Fact]
    public async Task AddViewAsync_returns_conflict_if_already_exists()
    {
        var response = await _repo.AddViewAsync(1, 2);

        Assert.Equal(Conflict, response);
    }
    [Fact]
    public async Task GetViewAsync_returns_2_if_two_relations_with_projectid_exist()
    {
        var count = await _repo.GetViewsOfProjectAsync(1);

        Assert.Equal(2, count);
    }
    [Fact]
    public async Task DeleteViewAsync_returns_deleted()
    {
        var response = await _repo.DeleteViewAsync(1);

        Assert.Equal(Deleted, response);
    }
    [Fact]
    public async Task DeleteViewAsync_returns_notFound_if_relations_with_projectid_do_not_exist()
    {
        var response = await _repo.DeleteViewAsync(120);

        Assert.Equal(NotFound, response);
    }
}
