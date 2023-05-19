using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCORE3.Models;
using OfficeOpenXml;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Controllers
{
    [EnableCors("CorsApi")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        public static IWebHostEnvironment environment;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment, IConfiguration _config)
        {
            userManager = _userManager;
            environment = _environment;
            config = _config;
        }
        // GET api/account
        [HttpPost]
        public async Task<IActionResult> Post(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserName = model.Email.Split(new[] { '@' })[0];
            var exit = await userManager.FindByEmailAsync(model.Email);
            // Kiểm tra tài khoản, email có tồn tại không
            //Nếu tài khoản không tồn tại -- Thêm mới
            var Password = config.GetValue<string>("DefaultPass");
            if (exit == null)
            {
                var user = new ApplicationUser() { UserName = UserName, FullName = model.FullName, MaNhanVien = model.MaNhanVien, Email = model.Email, IsActive = model.IsActive, MustChangePass = true, CreatedDate = DateTime.Now, DonVi_Id = model.DonVi_Id, BoPhan_Id = model.BoPhan_Id };
                IdentityResult result = await userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    foreach (string RoleName in model.RoleNames)
                    {
                        await userManager.AddToRoleAsync(user, RoleName);
                    }
                    return StatusCode(StatusCodes.Status201Created);
                }
                return BadRequest(string.Join(",", result.Errors));
            }
            else
            {
                if (exit.IsDeleted)
                {
                    exit.UpdatedDate = DateTime.Now;
                    exit.DeletedDate = null;
                    exit.IsDeleted = false;
                    exit.IsActive = model.IsActive;
                    exit.MustChangePass = true;
                    exit.PasswordHash = userManager.PasswordHasher.HashPassword(exit, Password);
                    var result = await userManager.UpdateAsync(exit);
                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(exit);
                        foreach (string item_remove in roles)
                        {
                            await userManager.RemoveFromRoleAsync(exit, item_remove);
                        }
                        foreach (string RoleName in model.RoleNames)
                        {
                            await userManager.AddToRoleAsync(exit, RoleName);
                        }
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    return BadRequest(string.Join(",", result.Errors));
                }
                return StatusCode(StatusCodes.Status409Conflict, "Thông tin email tài khoản đã tồn tại");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UserInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var UserName = model.Email.Split(new[] { '@' })[0];
            var exit = await userManager.FindByEmailAsync(model.Email);
            // Kiểm tra tài khoản, email có tồn tại không
            if (exit != null && exit.Id.ToString() != id)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Thông tin tài khoản, email đã tồn tại");
            }
            var appUser = await userManager.FindByIdAsync(model.Id);
            appUser.UserName = UserName;
            appUser.NormalizedUserName = UserName.ToUpper();
            appUser.FullName = model.FullName;
            appUser.MaNhanVien = model.MaNhanVien;
            appUser.Email = model.Email;
            appUser.IsActive = model.IsActive;
            appUser.UpdatedDate = DateTime.Now;
            appUser.DonVi_Id = model.DonVi_Id;
            appUser.BoPhan_Id = model.BoPhan_Id;
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(appUser);
                foreach (string item_remove in roles)
                {
                    await userManager.RemoveFromRoleAsync(appUser, item_remove);
                }
                foreach (string RoleName in model.RoleNames)
                {
                    await userManager.AddToRoleAsync(appUser, RoleName);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            else
                return BadRequest(string.Join(",", result.Errors));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var appUser = await userManager.FindByIdAsync(id);
            if (appUser == null)
                return NotFound();
            else
            {
                var role = await userManager.GetRolesAsync(appUser);
                if (role.Count > 0)
                {
                    return Ok(new UserInfoModel
                    {
                        Id = id,
                        Email = appUser.Email,
                        UserName = appUser.UserName,
                        MaNhanVien = appUser.MaNhanVien,
                        FullName = appUser.FullName,
                        IsActive = appUser.IsActive,
                        RoleNames = role.ToList(),
                        BoPhan_Id = appUser.BoPhan_Id,
                        DonVi_Id = appUser.DonVi_Id,
                        // DonVi_Id = appUser.DonVi_Id
                    });
                }
                return BadRequest();
            }
        }
        [HttpGet("GetListUser")]
        public async Task<ActionResult> GetListUser()
        {
            var query = await userManager.Users.Include(u => u.DonVi)
            .Include(u => u.BoPhan).ToListAsync();
            List<ListUserModel> list = new List<ListUserModel>();

            foreach (var item in query)
            {
                var infor = new ListUserModel();
                infor.Id = item.Id.ToString();
                infor.FullName = item.FullName;
                infor.TenBoPhan = item.BoPhan.TenBoPhan;
                infor.TenDonVi = item.DonVi.TenDonVi;
                list.Add(infor);
            }
            return Ok(list);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int page = 1, int pageSize = 20, string keyword = null)
        {
            var query = userManager.Users.Where(x => (string.IsNullOrEmpty(keyword) || x.Email.ToLower().Contains(keyword.ToLower()) || x.UserName.ToLower().Contains(keyword.ToLower()) || x.FullName.ToLower().Contains(keyword.ToLower())) && !x.IsDeleted)
            .Include(u => u.DonVi)
            .Include(u => u.BoPhan);
            List<UserInfoModel> list = new List<UserInfoModel>();
            foreach (var item in query)
            {
                var role = await userManager.GetRolesAsync(item);
                if (role.Count > 0)
                {
                    var info = new UserInfoModel();
                    info.Id = item.Id.ToString();
                    info.Email = item.Email;
                    info.UserName = item.UserName;
                    info.MaNhanVien = item.MaNhanVien;
                    info.FullName = item.FullName;
                    info.IsActive = item.IsActive;
                    info.RoleNames = role.ToList();
                    info.TenBoPhan = item.BoPhan.TenBoPhan;
                    info.TenDonVi = item.DonVi.TenDonVi;
                    list.Add(info);
                }
            }
            int totalRow = list.Count();
            int totalPage = (int)Math.Ceiling(totalRow / (double)pageSize);
            var data = list.OrderByDescending(a => a.Id).Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(new
            {
                totalRow,
                totalPage,
                data
            });
        }
        [HttpPut("Active/{id}")]
        public async Task<ActionResult> Active(string id)
        {
            var appUser = await userManager.FindByIdAsync(id);
            appUser.IsActive = !appUser.IsActive;
            appUser.UpdatedDate = DateTime.Now;
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                if (appUser.IsActive)
                {
                    return StatusCode(StatusCodes.Status200OK, "Mở khóa tài khoản thành công");
                }
                return StatusCode(StatusCodes.Status200OK, "Khóa tài khoản thành công");
            }
            return BadRequest(string.Join(",", result.Errors));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var appUser = await userManager.FindByIdAsync(id);
            appUser.IsDeleted = true;
            appUser.DeletedDate = DateTime.Now;
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, "Xóa tài khoản thành công");
            }
            return BadRequest(string.Join(",", result.Errors));
        }
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var Password = config.GetValue<string>("DefaultPass");
            var appUser = await userManager.FindByIdAsync(id);
            appUser.UpdatedDate = DateTime.Now;
            appUser.MustChangePass = true;
            appUser.PasswordHash = userManager.PasswordHasher.HashPassword(appUser, Password);
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, "Khôi phục mật khẩu mặc định thành công");
            }
            return BadRequest(string.Join(",", result.Errors));
        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appUser = await userManager.FindByIdAsync(User.Identity.Name);
            appUser.MustChangePass = false;
            appUser.UpdatedDate = DateTime.Now;
            var result = await userManager.ChangePasswordAsync(appUser, model.Password, model.NewPassword);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, "Đổi mật khẩu thành công");
            }
            return BadRequest("Mật khẩu hiện tại không đúng");
        }
        [HttpPost("Import")]
        public async Task<ActionResult> Import(IFormFile file)
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            DateTime dt = DateTime.Now;
            // Rename file
            string fileName = (long)timeSpan.TotalSeconds + "_" + Commons.TiengVietKhongDau(file.FileName);
            string fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
            string[] supportedTypes = new[] { "xls", "xlsx" };
            if (supportedTypes.Contains(fileExt))
            {
                string webRootPath = environment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRootPath))
                {
                    webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                }
                string fullPath = Path.Combine(webRootPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                byte[] file_byte = System.IO.File.ReadAllBytes(fullPath);
                //Kiểm tra tồn tại file và xóa
                System.IO.File.Delete(fullPath);
                using (MemoryStream ms = new MemoryStream(file_byte))
                using (ExcelPackage package = new ExcelPackage(ms))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    for (int i = 2; i <= rowCount; i++)
                    {
                        object hoten = worksheet.Cells[i, 1].Value;
                        string HoTen = hoten.ToString().Trim().Replace("\t", "").Replace("\n", "");
                        object email = worksheet.Cells[i, 2].Value;
                        string Email = email.ToString().Trim().Replace("\t", "").Replace("\n", "");
                        object vaitro = worksheet.Cells[i, 3].Value;
                        string VaiTro = vaitro.ToString().Trim().Replace("\t", "").Replace("\n", "");
                        var UserName = Email.Split(new[] { '@' })[0];
                        var exit_username = await userManager.FindByNameAsync(UserName);
                        var exit_email = await userManager.FindByEmailAsync(Email);
                        // Kiểm tra tài khoản, email có tồn tại không
                        if (exit_username == null && exit_email == null)
                        {
                            var Password = config.GetValue<string>("DefaultPass");
                            var user = new ApplicationUser() { UserName = UserName, FullName = HoTen, Email = Email, IsActive = true, MustChangePass = true, CreatedDate = DateTime.Now };
                            IdentityResult result = await userManager.CreateAsync(user, Password);
                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(user, VaiTro);
                            }
                        }
                    }
                    return Ok();
                }
            }
            return BadRequest("Định dạng tệp tin không cho phép");
        }
    }
}