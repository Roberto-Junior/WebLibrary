using BiblioTechA.Data;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace BiblioTechA.Models
{
    public static class SeedRole
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
               
                if (context.Roles.Any())
                {
                    return;
                }

                context.Roles.AddRange(
                    new IdentityRole
                    {
                        //Id = Guid.NewGuid().ToString(),
                        Name = "Usuário",
                        NormalizedName = "USUÁRIO"
                    },

                    new IdentityRole
                    {
                        //Id = Guid.NewGuid().ToString(),
                        Name = "Administrador",
                        NormalizedName = "ADMINISTRADOR"
                    },

                     new IdentityRole
                     {
                         //Id = Guid.NewGuid().ToString(),
                         Name = "Gerente",
                         NormalizedName = "GERENTE"
                     }
                );
                context.SaveChanges();
            }
        }
    }
}
