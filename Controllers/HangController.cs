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
  public class HangController : ControllerBase
  {
    private readonly IUnitofWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    public static IWebHostEnvironment environment;
    public HangController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment)
    {
      uow = _uow;
      userManager = _userManager;
      environment = _environment;
    }

    [HttpGet]
    public ActionResult Get(string keyword)
    {
      if (keyword == null) keyword = "";
      var data = uow.Hangs.GetAll(t => !t.IsDeleted && (t.MaHang.ToLower().Contains(keyword.ToLower()) || t.TenHang.ToLower().Contains(keyword.ToLower()))).Select(x => new
      {
        x.Id,
        x.MaHang,
        x.TenHang,
      });
      if (data == null)
      {
        return NotFound();
      }
      return Ok(data.OrderBy(x => x.TenHang));
    }

    [HttpGet("{id}")]
    public ActionResult Get(Guid id)
    {
      string[] include = { "ChiTietNhoms" };
      var duLieu = uow.Hangs.GetAll(x => !x.IsDeleted && x.Id == id, null, include);
      if (duLieu == null)
      {
        return NotFound();
      }
      return Ok(duLieu);
    }

    public class ChiTietLoai{
      public Guid Loai_Id { get; set; }
      public Guid VatTu_Id { get; set;}
      public string TenLoai { get; set;}
    }

    [HttpGet("GetChiTietHang")]
    public ActionResult GetChiTietHang(Guid idNhom, Guid idHang)
    {
      string[] include = { "Loai" };
      // lấy được danh sách vật tư
      var data = uow.VatTus.GetAll(x => x.Nhom_Id == idNhom && x.Hang_Id == idHang);
      // lấy danh sách loại từ danh sách vật tư
      List<ChiTietLoai> list = new List<ChiTietLoai>();
      foreach (var item in data)
      {
        var dataLoai = uow.LoaiVatTus.GetAll(x => x.VatTu_Id == item.Id, null, include).Select(x => new {Loai_Id = x.Loai_Id, VatTu_Id = x.VatTu_Id, TenLoai = x.Loai.TenLoai });
        foreach (var itemList in dataLoai)
        {
          if(!list.Exists(x => x.Loai_Id == itemList.Loai_Id)){
            list.Add(new ChiTietLoai{Loai_Id = itemList.Loai_Id, VatTu_Id = itemList.VatTu_Id, TenLoai = itemList.TenLoai});
          }
        }
      }
      return Ok(list.OrderBy(x => x.TenLoai));
    }

    [HttpPost]
    public ActionResult Post(Hang data)
    {
      lock (Commons.LockObjectState)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        if (uow.Hangs.Exists(x => x.MaHang == data.MaHang && !x.IsDeleted))
          return StatusCode(StatusCodes.Status409Conflict, "Mã " + data.MaHang + " đã tồn tại trong hệ thống");
        Guid id = Guid.NewGuid();
        data.Id = id;
        data.CreatedDate = DateTime.Now;
        data.CreatedBy = Guid.Parse(User.Identity.Name);
        uow.Hangs.Add(data);
        uow.Complete();
        return Ok();
      }
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, Hang data)
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
        uow.Hangs.Update(data);
        uow.Complete();
        return StatusCode(StatusCodes.Status204NoContent);
      }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        Hang duLieu = uow.Hangs.GetById(id);
        if (duLieu == null)
        {
          return NotFound();
        }
        duLieu.DeletedDate = DateTime.Now;
        duLieu.DeletedBy = Guid.Parse(User.Identity.Name);
        duLieu.IsDeleted = true;
        uow.Hangs.Update(duLieu);
        uow.Complete();
        return Ok(duLieu);
      }

    }
    [HttpDelete("Remove/{id}")]
    public ActionResult Delete_Remove(Guid id)
    {
      lock (Commons.LockObjectState)
      {
        uow.Hangs.Delete(id);
        uow.Complete();
        return Ok();
      }
    }
  }
}