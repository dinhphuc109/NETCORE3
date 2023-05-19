using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Controllers
{
  [EnableCors("CorsApi")]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UploadController : ControllerBase
  {
    private readonly IUnitofWork uow;
    public static IWebHostEnvironment environment;
    public UploadController(IUnitofWork _uow, IWebHostEnvironment _environment)
    {
      uow = _uow;
      environment = _environment;
    }
    [HttpPost]
    public ActionResult Upload(IFormFile file)
    {
      lock (Commons.LockObjectState)
      {
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime dt = DateTime.Now;
        // Rename file
        string fileName = (long)timeSpan.TotalSeconds + "_" + Commons.TiengVietKhongDau(file.FileName);
        string fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
        // string[] supportedTypes = new[] { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "pdf", "zip", "rar", "7z", "png", "jpg","jpeg","txt" };
        // if (supportedTypes.Contains(fileExt))
        // {
        string path = "Uploads/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
        string webRootPath = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
          webRootPath = Path.Combine(Directory.GetCurrentDirectory(), path);
        }
        if (!Directory.Exists(webRootPath))
        {
          Directory.CreateDirectory(webRootPath);
        }
        string fullPath = Path.Combine(webRootPath, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
          file.CopyTo(stream);
        }
        return Ok(new FileModel { Path = path + "/" + fileName, FileName = file.FileName });
        // }
        // else
        //   return BadRequest("Định dạng tệp tin không cho phép");
      }
    }
    [HttpPost("Multi")]
    public ActionResult Multi(List<IFormFile> lstFiles)
    {
      lock (Commons.LockObjectState)
      {
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime dt = DateTime.Now;
        List<FileModel> lst = new List<FileModel>();
        foreach (var file in lstFiles)
        {
          // Rename file
          string fileName = (long)timeSpan.TotalSeconds + "_" + Commons.TiengVietKhongDau(file.FileName);
          string fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
          string path = "Uploads/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
          string webRootPath = environment.WebRootPath;
          if (string.IsNullOrWhiteSpace(webRootPath))
          {
            webRootPath = Path.Combine(Directory.GetCurrentDirectory(), path);
          }
          if (!Directory.Exists(webRootPath))
          {
            Directory.CreateDirectory(webRootPath);
          }
          string fullPath = Path.Combine(webRootPath, fileName);
          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            file.CopyTo(stream);
          }
          lst.Add(new FileModel { Path = path + "/" + fileName, FileName = file.FileName });
        }
        return Ok(lst);
      }
    }
  }
}