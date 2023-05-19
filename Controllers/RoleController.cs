using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCORE3.Models;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Controllers
{
  [EnableCors("CorsApi")]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class RoleController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;
    public RoleController(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
    {
      userManager = _userManager;
      roleManager = _roleManager;
    }
    [HttpPost]
    public async Task<IActionResult> Post(ApplicationRole duLieu)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var exit = await roleManager.FindByNameAsync(duLieu.Name);
      if (exit == null)
      {
        IdentityResult result = await roleManager.CreateAsync(duLieu);
        if (result.Succeeded)
        {
          return StatusCode(StatusCodes.Status201Created);
        }
        else
          return BadRequest(string.Join(",", result.Errors));
      }
      else
      {
        if (exit.IsDeleted)
        {
          exit.IsDeleted = false;
          exit.DeletedDate = null;
          exit.Description = duLieu.Description;
          var result = await roleManager.UpdateAsync(exit);
          if (result.Succeeded)
          {
            return StatusCode(StatusCodes.Status204NoContent);
          }
          else
            return BadRequest(string.Join(",", result.Errors));
        }
        return StatusCode(StatusCodes.Status409Conflict, "Thông tin vai trò đã tồn tại");
      }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, ApplicationRole duLieu)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      if (id != duLieu.Id.ToString())
      {
        return BadRequest();
      }
      if (await roleManager.RoleExistsAsync(duLieu.Name) && duLieu.Id.ToString() != id)
      {
        return StatusCode(StatusCodes.Status409Conflict, "Thông tin vai trò đã tồn tại");
      }
      else
      {
        var role = await roleManager.FindByIdAsync(id);
        role.Description = duLieu.Description;
        role.UpdatedDate = DateTime.Now;
        role.Name = duLieu.Name;
        var result = await roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
          return StatusCode(StatusCodes.Status204NoContent);
        }
        return BadRequest(string.Join(",", result.Errors));
      }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(string id)
    {
      var duLieu = await roleManager.FindByIdAsync(id);
      if (duLieu == null)
      {
        return NotFound();
      }
      return Ok(duLieu);
    }
    [HttpGet]
    public ActionResult Get(int page = 1, int pageSize = 20, string keyword = null)
    {
      var query = roleManager.Roles.Include(x => x.UserRoles).Include(x => x.Menu_Roles).Where(x => (string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword.ToLower()) || x.Description.ToLower().Contains(keyword.ToLower())) && !x.IsDeleted);
      int count = query.Count();
      int totalPages = (int)Math.Ceiling(count / (double)pageSize);
      var data = query.OrderByDescending(a => a.Id).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { Id = x.Id, Name = x.Name, Description = x.Description, IsUsed = (x.UserRoles.Count > 0 || x.Menu_Roles.Where(a => !a.IsDeleted).Count() > 0) });
      return Ok(new
      {
        totalRow = count,
        totalPages,
        data
      });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var role = await roleManager.FindByIdAsync(id);
      role.IsDeleted = true;
      role.DeletedDate = DateTime.Now;
      var result = await roleManager.UpdateAsync(role);
      if (result.Succeeded)
      {
        return StatusCode(StatusCodes.Status200OK, "Xóa vai trò thành công");
      }
      return BadRequest(string.Join(",", result.Errors));
    }
    [HttpGet("Form")]
    public ActionResult Form()
    {
      var query = roleManager.Roles.Where(x => !x.IsDeleted);
      return Ok(query);
    }
  }
}