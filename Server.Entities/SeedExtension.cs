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
            //await context.Database.MigrateAsync();

            var facade = new DBFacade(context);
            await facade.CreateUser("Gustav Holmgren", true, "guho@itu.dk");
            await facade.CreateUser("Lukas", true, "luks@itu.dk");
            await facade.CreateUser("Sam Al-Sapti", true, "sals@itu.dk");
            await facade.CreateUser("Alexander Skou-Larsen", true, "alsk@itu.dk");
            await facade.CreateUser("Alexander Rode", true, "arod@itu.dk");
            await facade.CreateUser("Frederik Svendsen", true, "frsv@itu.dk");
            await facade.CreateUser("Kevin Bacon", false, "pbstudent1@hotmail.com");
            await facade.CreateUser("Hellen Mirren", false, "pbstudent2@hotmail.com");
            await facade.CreateUser("Supervisor Sam", true, "salsitu@outlook.com");
            await facade.CreateUser("Paolo Tell", true, "pate@itu.dk");
            await facade.CreateUser("Rasmus Lystrøm", true, "rnie@itu.dk");
            //await facade.CreateProject("mAchine", "something about AI", 2);
            //await facade.ApplyToProject(1, 2);
            //await facade.AddView(1, 2);
            //await facade.AddView(2, 2);
            context.SaveChanges();
        }
    }
}
