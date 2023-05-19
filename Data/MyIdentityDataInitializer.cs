using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using static NETCORE3.Data.MyDbContext;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Data
{

  public class MyIdentityDataInitializer
  {
    public static IUnitofWork uow;
    public MyIdentityDataInitializer(IUnitofWork _uow)
    {
      uow = _uow;
    }
    public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
      if (!await roleManager.RoleExistsAsync("Administrator"))
      {
        await roleManager.CreateAsync(new ApplicationRole { Name = "Administrator", Description = "Quản trị", CreatedDate = DateTime.Now });
      }
      if (await userManager.FindByNameAsync("levinhdu") == null)
      {
        ApplicationUser user = new ApplicationUser();
        user.Id = Guid.Parse("c662783d-03c0-4404-9473-1034f1ac1caa");
        user.UserName = "levinhdu";
        user.Email = "levinhdu@thaco.com.vn";
        user.FullName = "Lê Vinh Dự";
        user.MaNhanVien = "2303032";
        user.IsActive = true;
        user.CreatedDate = DateTime.Now;
        IdentityResult result = await userManager.CreateAsync(user, "Abc@2017");
        if (result.Succeeded)
        {
          userManager.AddToRoleAsync(user, "Administrator").Wait();
        }
      } 
    }
  }
}