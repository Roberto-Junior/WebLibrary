using BiblioTechA.Data;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace BiblioTechA.Models
{
    public static class SeedRoleAndUser
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

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
                        Id = "1", 
                        Name = "Usuário",
                        NormalizedName = "USUÁRIO"
                    },

                    new IdentityRole
                    {
                        Id = "2", 
                        Name = "Administrador",
                        NormalizedName = "ADMINISTRADOR"
                    },

                     new IdentityRole
                     {
                         Id = "3", 
                         Name = "Gerente",
                         NormalizedName = "GERENTE"
                     }
                );
                context.SaveChanges();

                context.Users.Add(
                    new ApplicationUser
                    {
                        Id = "1",
                        FirstName = "gerente",
                        LastName = "one",
                        Email = "gerente@hotmail.com",
                        NormalizedEmail = "GERENTE@HOTMAIL.COM",
                        UserName = "gerente@hotmail.com",
                        NormalizedUserName = "GERENTE@HOTMAIL.COM",
                        EmailConfirmed = true,
                        AskedReservation = false,
                        PasswordHash = hasher.HashPassword(null, "123456")
                    }
                );
                    context.SaveChanges();

                context.UserRoles.Add(
                    new IdentityUserRole<string>
                    {
                        RoleId = "3",
                        UserId = "1"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
