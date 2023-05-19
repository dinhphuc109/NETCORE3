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
  public class DonViController : ControllerBase
  {
    private readonly IUnitofWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    public static IWebHostEnvironment environment;
    public DonViController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment)
    {
      uow = _uow;
      userManager = _userManager;
      environment = _environment;
    }

    [HttpGet]
    public ActionResult Get(string keyword)
    {
      if (keyword == null) keyword = "";
      var data = uow.DonVis.GetAll(t => !t.IsDeleted && (t.MaDonVi.ToLower().Contains(keyword.ToLower()) || t.TenDonVi.ToLower().Contains(keyword.ToLower()))).Select(x => new
      {
        x.Id,
        x.MaDonVi,
        x.TenDonVi,
        x.TapDoan.TenTapDoan,
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
      DonVi duLieu = uow.DonVis.GetById(id);
      if (duLieu == null)
      {
        return NotFound();
      }
      return Ok(duLieu);
    }

    [HttpPost]
    public ActionResult Post(DonVi data)
    {
      lock (Commons.LockObjectState)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (uow.DonVis.Exists(x => x.MaDonVi == data.MaDonVi && !x.IsDeleted))
          return StatusCode(StatusCodes.Status409Conflict, "Mã " + data.MaDonVi + " đã tồn tại trong hệ thống");
        data.CreatedDate = DateTime.Now;
        data.CreatedBy = Guid.Parse(User.Identity.Name);
        uow.DonVis.Add(data);
        uow.Complete();
        return Ok();
      }
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, DonVi data)
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
        uow.DonVis.Update(data);
        uow.Complete();
        return StatusCode(StatusCodes.Status204NoContent);
      }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        DonVi duLieu = uow.DonVis.GetById(id);
        if (duLieu == null)
        {
          return NotFound();
        }
        duLieu.DeletedDate = DateTime.Now;
        duLieu.DeletedBy = Guid.Parse(User.Identity.Name);
        duLieu.IsDeleted = true;
        uow.DonVis.Update(duLieu);
        uow.Complete();
        return Ok(duLieu);
      }

    }
    [HttpDelete("Remove/{id}")]
    public ActionResult Delete_Remove(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        uow.DonVis.Delete(id);
        uow.Complete();
        return Ok();
      }
    }
  }
}