using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Controllers
{
  [EnableCors("CorsApi")]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class DashboardController : ControllerBase
  {
    private MyDbContext db;
    private readonly IUnitofWork uow;
    public DashboardController(IUnitofWork _uow, MyDbContext _db)
    {
      uow = _uow;
      db = _db;
    }
    [HttpGet]
    public ActionResult Get(string user)
    {
      Expression<Func<Log, bool>> whereFunc = item => item.AccessdBy.ToString() == user;
      var result = uow.Logs.GetAll(whereFunc, null, null).Select(x => new { x.Id }).ToList();
      return Ok(result.Count);
    }
  }
}