using Microsoft.AspNetCore.Identity;

namespace BiblioTechA.Models
{
    public class Role
    {
        RoleManager<IdentityRole> _roleManager;

        public Role(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
    }
}
