using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectBank.Server;
using ProjectBank.Server.Entities;

namespace Server.Entities
{
    public static class SeedExtension
    {
        public static async Task<IHost> SeedAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

                await SeedProjectAsync(context);
            }
            return host;
        }
        public static async Task SeedProjectAsync(ProjectBankContext context)
        {
            await context.Database.MigrateAsync();

            var facade = new DBFacade(context);
            await facade.CreateProject("mAchine", "something about AI", 2);
            await facade.ApplyToProject(1, 2);
            await facade.AddView(1, 2);
            await facade.AddView(2, 2);
            context.SaveChanges();
        }
    }
}
