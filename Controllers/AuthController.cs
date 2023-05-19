using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCORE3.Helpers;
using NETCORE3.Infrastructure;
using NETCORE3.Models;
using static NETCORE3.Data.MyDbContext;
using Microsoft.Extensions.Configuration;

namespace NETCORE3.Controllers
{
  [EnableCors("CorsApi")]
  [Route("token")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUnitofWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly AppSettings appSettings;
    private readonly IConfiguration config;
    public AuthController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, IOptions<AppSettings> _appSettings, IConfiguration _config)
    {
      uow = _uow;
      userManager = _userManager;
      signInManager = _signInManager;
      appSettings = _appSettings.Value;
      config = _config;
    }
    [HttpPost]
    public async Task<IActionResult> Authencation(LoginModel model)
    {
      var user = new ApplicationUser();
      var Email = "";
      if (model.Domain != "")
      {
        Email = model.Username + "@" + model.Domain;
        user = await userManager.FindByEmailAsync(Email);
        if (user == null)
        {
          var Password = model.Password;
          var userNew = new ApplicationUser();
          userNew.Id = Guid.NewGuid();
          userNew.UserName = model.Username;
          userNew.Email = Email;
          userNew.FullName = "";
          userNew.IsActive = true;
          userNew.CreatedDate = DateTime.Now;
          IdentityResult result = await userManager.CreateAsync(userNew, "Abc@2017");
          user = userNew;
          if (result.Succeeded)
          {
            userManager.AddToRoleAsync(userNew, "NHANVIEN").Wait();
          }
        }
      }
      else
      {
        Email = model.Username;
        user = await userManager.FindByNameAsync(model.Username);
      }
      if (user == null)
      {
        return BadRequest("Tài khoản không tồn tại");
      }
      else
      {
        if (!user.IsActive)
        {
          return BadRequest("Tài khoản đã bị khóa");
        }
        else
        {
          if (IsValidEmail(Email))
          {
            var isLoginedByEmail = CheckLogin(Email, model.Password, model.Domain);
            if (isLoginedByEmail)
            {
              // Update password same password login mail Thaco
              user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
              user.MustChangePass = false;
              var update_pass = await userManager.UpdateAsync(user);
              if (update_pass.Succeeded)
              {
                return Ok(GenToken(new UserToken { Id = user.Id.ToString(), Email = user.Email, FullName = user.FullName, MustChangePass = user.MustChangePass }));
              }
              else
              {
                return BadRequest("Đồng bộ mật khẩu tài khoản thất bại");
              }
            }
            else
            {
              return BadRequest("Đăng nhập Email thất bại");
            }
          }
          else
          {
            var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
              return Ok(GenToken(new UserToken { Id = user.Id.ToString(), Email = user.Email, FullName = user.FullName, MustChangePass = user.MustChangePass }));
            }
            else
            {
              return BadRequest("Thông tin đăng nhập không đúng");
            }
          }
        }
      }
    }
    private InfoLogin GenToken(UserToken userToken)
    {
      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Name, userToken.Id)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return new InfoLogin() { Token = tokenHandler.WriteToken(token), Id = userToken.Id, Email = userToken.Email, FullName = userToken.FullName, Expires = token.ValidTo, MustChangePass = userToken.MustChangePass };
    }
    bool IsValidEmail(string email)
    {
      try
      {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
      }
      catch
      {
        return false;
      }
    }
    [Authorize]
    [HttpPost("Refresh")]
    public IActionResult RefreshToken(UserToken model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return Ok(GenToken(new UserToken { Id = model.Id, Email = model.Email, FullName = model.FullName, MustChangePass = model.MustChangePass }));
    }
    private static bool CheckLogin(string email, string password, string domain)
    {
      ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
      service.Credentials = new WebCredentials(email, password);
      if (domain == "thaco.com.vn")
      {
        service.Url = new Uri("https://mail.thaco.com.vn/ews/exchange.asmx");
      }
      else if (domain == "vinamazda.vn")
      {
        service.Url = new Uri("https://mail.vinamazda.vn/ews/exchange.asmx");
      }
      else if (domain == "dqmcorp.vn")
      {
        service.Url = new Uri("https://mail.dqmcorp.vn/ews/exchange.asmx");
      }
      try
      {
        var findFolderResults = service.FindFolders(WellKnownFolderName.Root, new FolderView(1));
        if (findFolderResults != null)
          return true;
        else
          return false;
      }
      catch
      {
        return false;
      }

    }
  }
}