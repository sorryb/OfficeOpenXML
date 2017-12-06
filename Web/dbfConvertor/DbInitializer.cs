using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using ExcelToDbfConvertor.Data;
using ExcelToDbfConvertor.Models;

namespace ExcelToDbfConvertor
{
    /// <summary>
    /// Initialize
    /// </summary>
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        /// <summary>
        /// This example just creates an Administrator role and one Admin users
        /// </summary>
        public async void Initialize()
        {
            //create database schema if none exists
            _context.Database.EnsureCreated();

            ////If there is already an Administrator role, abort
            //if (_context.Roles.Any(r => r.Name == "Administrator")) return;

            //////Create the Administartor Role
            ////await _roleManager.CreateAsync(new IdentityRole("Administrator")); 

            
            //Create the default Admin account and apply the Administrator role
            string user = "sorin@gmail.com";
            string password = "bT@dm!n1";
            //await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true}, password);
            //await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Administrator");

            string role = "SuperAdmins";

            if (await _userManager.FindByNameAsync(user) == null)
            {
                // Create SuperAdmins role if it doesn't exist
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                // Create user account if it doesn't exist
                ApplicationUser userSeed = new ApplicationUser
                {
                    UserName = user,
                    Email = user
                    ,
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(userSeed, password);

                // Assign role to the user
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userSeed, role);
                }
            }
        }
    }
}
