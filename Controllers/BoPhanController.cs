using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCORE3.Infrastructure;
using NETCORE3.Models;
using OfficeOpenXml;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Controllers
{
  [EnableCors("CorsApi")]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class BoPhanController : ControllerBase
  {
    private readonly IUnitofWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    public static IWebHostEnvironment environment;
    public BoPhanController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment)
    {
      uow = _uow;
      userManager = _userManager;
      environment = _environment;
    }

    [HttpGet]
    public ActionResult Get(string keyword)
    {
      if (keyword == null) keyword = "";
      string[] include = { "DonVi" };
      var data = uow.BoPhans.GetAll(t => !t.IsDeleted && (t.MaBoPhan.ToLower().Contains(keyword.ToLower()) || t.TenBoPhan.ToLower().Contains(keyword.ToLower())),null, include).Select(x => new
      {
        x.Id,
        x.MaBoPhan,
        x.TenBoPhan,
        x.DonVi.TenDonVi
      });
      if (data == null)
      {
        return NotFound();
      }
      return Ok(data);
    }

    [HttpGet("{id}")]
    public ActionResult Get(Guid id)
    {
      string[] include = { "DonVi" };
      var duLieu = uow.BoPhans.GetAll(x => !x.IsDeleted && x.Id == id, null, include);;
      if (duLieu == null)
      {
        return NotFound();
      }
      return Ok(duLieu);
    }

    [HttpPost]
    public ActionResult Post(BoPhan data)
    {
      lock (Commons.LockObjectState)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (uow.BoPhans.Exists(x => x.MaBoPhan == data.MaBoPhan && !x.IsDeleted))
          return StatusCode(StatusCodes.Status409Conflict, "Mã " + data.MaBoPhan + " đã tồn tại trong hệ thống");
        data.CreatedDate = DateTime.Now;
        data.CreatedBy = Guid.Parse(User.Identity.Name);
        uow.BoPhans.Add(data);
        uow.Complete();
        return Ok();
      }
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, BoPhan data)
    {
      lock (Commons.LockObjectState)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (id != data.Id)
        {
          return BadRequest();
        }
        data.UpdatedBy = Guid.Parse(User.Identity.Name);
        data.UpdatedDate = DateTime.Now;
        uow.BoPhans.Update(data);
        uow.Complete();
        return StatusCode(StatusCodes.Status204NoContent);
      }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        BoPhan duLieu = uow.BoPhans.GetById(id);
        if (duLieu == null)
        {
          return NotFound();
        }
        duLieu.DeletedDate = DateTime.Now;
        duLieu.DeletedBy = Guid.Parse(User.Identity.Name);
        duLieu.IsDeleted = true;
        uow.BoPhans.Update(duLieu);
        uow.Complete();
        return Ok(duLieu);
      }

    }
    [HttpDelete("Remove/{id}")]
    public ActionResult Delete_Remove(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        uow.BoPhans.Delete(id);
        uow.Complete();
        return Ok();
      }
    }
  }
}