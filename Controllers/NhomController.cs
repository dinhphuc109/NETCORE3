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
  public class NhomController : ControllerBase
  {
    private readonly IUnitofWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    public static IWebHostEnvironment environment;
    public NhomController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment)
    {
      uow = _uow;
      userManager = _userManager;
      environment = _environment;
    }

    [HttpGet]
    public ActionResult Get(string keyword)
    {
      if (keyword == null) keyword = "";
      var data = uow.Nhoms.GetAll(t => !t.IsDeleted && (t.MaNhom.ToLower().Contains(keyword.ToLower()) || t.TenNhom.ToLower().Contains(keyword.ToLower()))).Select(x => new
      {
        x.Id,
        x.MaNhom,
        x.TenNhom,
      });
      if (data == null)
      {
        return NotFound();
      }
      return Ok(data.OrderBy(x => x.TenNhom));
    }

    [HttpGet("{id}")]
    public ActionResult Get(Guid id)
    {
      var duLieu = uow.Nhoms.GetAll(x => !x.IsDeleted && x.Id == id);
      if (duLieu == null)
      {
        return NotFound();
      }
      return Ok(duLieu);
    }

    public class ChiTietNhomResult
    {
      public Guid Id { get; set; }
      public string TenHang { get; set; }
    }

    [HttpGet("GetChiTietNhom")]
    public ActionResult GetChiTietNhom(Guid id)
    {
      string[] include = { "Hang" };
      var data = uow.VatTus.GetAll(x => x.Nhom_Id == id && !x.IsDeleted).GroupBy(x => x.Hang_Id).Select(x => new { x.Key });
      var dataHang = uow.Hangs.GetAll(x => !x.IsDeleted).Select(x => new { x.Id, x.TenHang });
      List<ChiTietNhomResult> list = new List<ChiTietNhomResult>();
      foreach (var x in data)
      {
        var item = dataHang.FirstOrDefault(y => y.Id == x.Key);
        list.Add(new ChiTietNhomResult { Id = item.Id, TenHang = item.TenHang });
      }
      return Ok(list.OrderBy(x => x.TenHang));
    }
    [HttpPost]
    public ActionResult Post(Nhom data)
    {
      lock (Commons.LockObjectState)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (uow.Nhoms.Exists(x => x.MaNhom == data.MaNhom && !x.IsDeleted))
          return StatusCode(StatusCodes.Status409Conflict, "Mã " + data.MaNhom + " đã tồn tại trong hệ thống");
        Guid id = Guid.NewGuid();
        data.Id = id;
        data.CreatedDate = DateTime.Now;
        data.CreatedBy = Guid.Parse(User.Identity.Name);
        uow.Nhoms.Add(data);
        uow.Complete();
        return Ok();
      }
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, Nhom data)
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
        uow.Nhoms.Update(data);
        uow.Complete();
        return StatusCode(StatusCodes.Status204NoContent);
      }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        Nhom duLieu = uow.Nhoms.GetById(id);
        if (duLieu == null)
        {
          return NotFound();
        }
        duLieu.DeletedDate = DateTime.Now;
        duLieu.DeletedBy = Guid.Parse(User.Identity.Name);
        duLieu.IsDeleted = true;
        uow.Nhoms.Update(duLieu);
        uow.Complete();
        return Ok(duLieu);
      }

    }
    [HttpDelete("Remove/{id}")]
    public ActionResult Delete_Remove(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        uow.Nhoms.Delete(id);
        uow.Complete();
        return Ok();
      }
    }
  }
}