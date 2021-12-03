using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Server;
using static ProjectBank.Server.Response;
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
            IsSupervisor = true
        });
        context.Users.Add(new User
        {
            Id = 2,
            Name = "jakob",
            IsSupervisor = false
        });
        context.Users.Add(new User
        {
            Id = 3,
            Name = "troels",
            IsSupervisor = false
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
        var application = await _repo.AddViewAsync(2, 3);

        Assert.Equal(Created, application);
    }
    [Fact]
    public async Task AddViewAsync_returns_conflict_if_already_exists()
    {
        var application = await _repo.AddViewAsync(1, 2);

        Assert.Equal(Conflict, application);
    }
    [Fact]
    public async Task GetViewAsync_returns_nr_of_views()
    {
        var application = await _repo.GetViewsOfProjectAsync(1);

        Assert.Equal(2, application);
    }
    [Fact]
    public async Task DeleteViewAsync_returns_deleted()
    {
        var application = await _repo.DeleteViewAsync(1);

        Assert.Equal(Deleted, application);
    }
    [Fact]
    public async Task DeleteViewAsync_returns_notFound()
    {
        var application = await _repo.DeleteViewAsync(120);

        Assert.Equal(NotFound, application);
    }
}
