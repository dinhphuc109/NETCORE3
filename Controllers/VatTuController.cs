using System;
using System.Collections.Generic;
using System.Drawing;
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
using OfficeOpenXml.Drawing;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Controllers
{
    [EnableCors("CorsApi")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VatTuController : ControllerBase
    {
        private readonly IUnitofWork uow;
        private readonly UserManager<ApplicationUser> userManager;
        public static IWebHostEnvironment environment;
        public VatTuController(IUnitofWork _uow, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _environment)
        {
            uow = _uow;
            userManager = _userManager;
            environment = _environment;
        }

        [HttpGet]
        public ActionResult Get()
        {
            string[] include = { "DonViTinh", "Nhom", "Hang", "LoaiVatTus", "LoaiVatTus.Loai", "User" };
            var data = uow.VatTus.GetAll(t => !t.IsDeleted, null, include)
            .Select(x => new
            {
                x.TenVatTu,
                x.Id,
                x.MaVatTu,
                x.DonViTinh.TenDonViTinh,
                x.Nhom.TenNhom,
                x.Hang.TenHang,
                x.ThongSoKyThuat,
                x.ViTri,
                x.User,
                LstLoai = x.LoaiVatTus.Select(y => new
                {
                    y.Loai.TenLoai,
                })
            }).OrderBy(x => x.TenVatTu)
            .ToList();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetDataPagnigation")]
        public ActionResult GetDataPagnigation(int page = 1, int pageSize = 20, string keyword = null)
        {
            if (keyword == null) keyword = "";
            string[] include = { "User", "DonViTinh" };
            var query = uow.VatTus.GetAll(t => !t.IsDeleted && (t.TenVatTu.ToLower().Contains(keyword.ToLower()) || t.MaVatTu.ToLower().Contains(keyword.ToLower()) || t.TenVatTuKhongDau.ToLower().Contains(keyword.ToLower())), null, include)
            .Select(x => new
            {
                x.TenVatTu,
                x.Id,
                x.MaVatTu,
                x.DonViTinh.TenDonViTinh,
                x.User.BoPhan_Id,
                x.User.DonVi_Id,
                x.User.FullName,
                x.ViTri,
            })
            .OrderBy(x => x.TenVatTu);
            List<ClassListVatTu> list = new List<ClassListVatTu>();

            foreach (var item in query)
            {
                var donvi = uow.DonVis.GetAll(x => !x.IsDeleted && x.Id == item.DonVi_Id, null, null).Select(x => new { x.TenDonVi }).ToList();
                var bophan = uow.BoPhans.GetAll(x => !x.IsDeleted && x.Id == item.BoPhan_Id, null, null).Select(x => new { x.TenBoPhan }).ToList();

                var infor = new ClassListVatTu();
                infor.Id = item.Id;
                infor.FullName = item.FullName;
                infor.TenVatTu = item.TenVatTu;
                infor.MaVatTu = item.MaVatTu;
                infor.ViTri = item.ViTri;
                infor.TenBoPhan = bophan[0].TenBoPhan;
                infor.TenDonVi = donvi[0].TenDonVi;
                list.Add(infor);
            }
            int totalRow = list.Count();
            int totalPage = (int)Math.Ceiling(totalRow / (double)pageSize);
            var data = list.OrderByDescending(a => a.Id).Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(new { data, totalPage, totalRow });
        }
        [HttpGet("GetVatTuDetail/{id}")]
        public ActionResult GetVatTuDetail(Guid id)
        {
            string[] include = { "User", "DonViTinh", "Nhom", "Hang", "LoaiVatTus.Loai" };
            var query = uow.VatTus.GetAll(t => !t.IsDeleted && t.Id == id, null, include)
            .Select(x => new
            {
                x.TenVatTu,
                x.Id,
                x.MaVatTu,
                x.DonViTinh.TenDonViTinh,
                x.User.BoPhan_Id,
                x.User.DonVi_Id,
                x.User.FullName,
                x.Nhom.TenNhom,
                x.Hang.TenHang,
                x.ViTri,
                x.ThongSoKyThuat,
                LstLoai = x.LoaiVatTus.Select(y => new
                {
                    y.Loai.TenLoai,
                }).ToList()
            })
            .OrderBy(x => x.TenVatTu);
            List<ClassVatTuDetail> list = new List<ClassVatTuDetail>();

            foreach (var item in query)
            {
                var donvi = uow.DonVis.GetAll(x => !x.IsDeleted && x.Id == item.DonVi_Id, null, null).Select(x => new { x.TenDonVi }).ToList();
                var bophan = uow.BoPhans.GetAll(x => !x.IsDeleted && x.Id == item.BoPhan_Id, null, null).Select(x => new { x.TenBoPhan }).ToList();

                var infor = new ClassVatTuDetail();
                infor.Id = item.Id;
                infor.FullName = item.FullName;
                infor.TenVatTu = item.TenVatTu;
                infor.MaVatTu = item.MaVatTu;
                infor.ViTri = item.ViTri;
                infor.TenBoPhan = bophan[0].TenBoPhan;
                infor.TenDonVi = donvi[0].TenDonVi;
                infor.TenNhom = item.TenNhom;
                infor.TenHang = item.TenHang;
                infor.ThongSoKyThuat = item.ThongSoKyThuat;
                infor.TenLoai = item.LstLoai[0].TenLoai;
                list.Add(infor);
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            string[] includes = { "LoaiVatTus" };
            var duLieu = uow.VatTus.GetAll(x => !x.IsDeleted && x.Id == id, null, includes);
            if (duLieu == null)
            {
                return NotFound();
            }
            return Ok(duLieu);
        }
        [HttpPost("SearchVatTu")]
        public ActionResult SearchVatTu(string keyword, Guid? Nhom_Id, Guid? Hang_Id, List<Guid> LstLoai)
        {
            if (keyword == null) keyword = "";
            string[] include = { "DonViTinh", "Nhom", "Hang", "LoaiVatTus", "LoaiVatTus.Loai" };
            var data = uow.VatTus.GetAll(x => !x.IsDeleted && (x.TenVatTu.ToLower().Contains(keyword.ToLower()) || x.MaVatTu.ToLower().Contains(keyword.ToLower()) || x.TenVatTuKhongDau.ToLower().Contains(keyword.ToLower())) && x.Nhom_Id == (Nhom_Id == null ? x.Nhom_Id : Nhom_Id) && x.Hang_Id == (Hang_Id == null ? x.Hang_Id : Hang_Id), null, include).Select(x => new
            {
                x.TenVatTu,
                x.Id,
                x.MaVatTu,
                x.DonViTinh.TenDonViTinh,
                x.Nhom.TenNhom,
                x.Hang.TenHang,
                x.PathImage,
                x.ThongSoKyThuat,
                LstLoai = x.LoaiVatTus.Select(y => new
                {
                    y.Loai.TenLoai,
                })
            }).OrderBy(x => x.TenVatTu)
            .ToList();
            if (LstLoai.Count() > 0)
            {
                List<Object> lst = new List<Object>();
                foreach (var item in data)
                {
                    var dem = 0;
                    if (LstLoai.Count() > 0)
                    {
                        foreach (var it in LstLoai)
                        {
                            var checkTon = uow.LoaiVatTus.Exists(x => x.Loai_Id == it && x.VatTu_Id == item.Id);
                            if (checkTon == true) dem++; else dem = 0;
                        }
                        if (dem == LstLoai.Count())
                        {
                            lst.Add(item);
                        }
                    }
                }
                return Ok(lst);
            }
            return Ok(data);
        }

        [HttpPost("ExportFileExcel")]
        public ActionResult ExportFileExcel(List<ExportExcel> data)
        {
            string fullFilePath = Path.Combine(environment.ContentRootPath, "Uploads/Templates/FileMauExportVatTu.xlsx");
            string dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            string[] arrDate = dateNow.Split("/");
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fullFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells["B5"].Value = dateNow;
                worksheet.Cells["G6"].Value = ".../" + arrDate[1] + "/" + arrDate[2];
                worksheet.Cells["H6"].Value = ".../" + arrDate[1] + "/" + arrDate[2];
                worksheet.Cells["I6"].Value = ".../" + arrDate[1] + "/" + arrDate[2];
                int indexrow = 15;
                int stt = 1;
                foreach (var item in data)
                {
                    worksheet.InsertRow(indexrow, 1, 14);
                    worksheet.Row(indexrow).Height = 55;
                    worksheet.Cells["A" + indexrow].Value = stt;
                    worksheet.Cells["B" + indexrow].Value = "00";
                    worksheet.Cells["C" + indexrow].Value = item.MaVatTu;
                    worksheet.Cells["D" + indexrow].Value = item.TenVatTu;
                    worksheet.Cells["E" + indexrow].Value = item.ThongSoKyThuat;
                    worksheet.Cells["G" + indexrow].Value = item.TenDonViTinh;
                    worksheet.Cells["H" + indexrow].Value = item.SoLuong;
                    if (!string.IsNullOrEmpty(item.PathImage))
                    {
                        ExcelPicture pic = worksheet.Drawings.AddPicture("F" + indexrow, Commons.HinhAnhUrl(item.PathImage));
                        pic.SetPosition((indexrow - 2), 1, 5, 1);
                        pic.SetSize(60, 60);
                    }
                    indexrow++;
                    stt++;
                }
                worksheet.DeleteRow(14, 1);
                return Ok(new { dataexcel = package.GetAsByteArray() });
            }
        }


        [HttpPost("UploadFile")]
        public ActionResult UploadFile(IFormFile file)
        {
            lock (Commons.LockObjectState)
            {
                var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                DateTime dt = DateTime.Now;
                // Rename file
                string fileName = (long)timeSpan.TotalSeconds + "_" + Commons.TiengVietKhongDau(file.FileName);
                string fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                string path = "Uploads/Image";
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
            }
        }

        [HttpPost]
        public ActionResult Post(VatTu data)
        {
            lock (Commons.LockObjectState)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (uow.VatTus.Exists(x => x.MaVatTu == data.MaVatTu && !x.IsDeleted))
                    return StatusCode(StatusCodes.Status409Conflict, "Mã " + data.MaVatTu + " đã tồn tại trong hệ thống");
                else if (uow.VatTus.Exists(x => x.MaVatTu == data.MaVatTu && !x.IsDeleted))
                {
                    var vattu = uow.VatTus.GetAll(x => x.MaVatTu == data.MaVatTu).ToArray();
                    vattu[0].IsDeleted = false;
                    vattu[0].DeletedBy = null;
                    vattu[0].DeletedDate = null;
                    vattu[0].UpdatedBy = Guid.Parse(User.Identity.Name);
                    vattu[0].UpdatedDate = DateTime.Now;
                    vattu[0].MaVatTu = data.MaVatTu;
                    vattu[0].TenVatTu = data.TenVatTu;
                    vattu[0].DVT_Id = data.DVT_Id;
                    vattu[0].ViTri = data.ViTri;
                    vattu[0].Hang_Id = data.Hang_Id;
                    vattu[0].Nhom_Id = data.Nhom_Id;
                    vattu[0].ThongSoKyThuat = data.ThongSoKyThuat;
                    vattu[0].TenVatTuKhongDau = Commons.NonUnicode(data.TenVatTu);
                    uow.VatTus.Update(vattu[0]);
                    foreach (var item in data.LstLoai)
                    {
                        item.CreatedBy = Guid.Parse(User.Identity.Name);
                        item.CreatedDate = DateTime.Now;
                        item.VatTu_Id = vattu[0].Id;
                        uow.LoaiVatTus.Add(item);
                    }
                }
                else
                {
                    Guid id = Guid.NewGuid();
                    data.Id = id;
                    data.CreatedDate = DateTime.Now;
                    data.CreatedBy = Guid.Parse(User.Identity.Name);
                    data.TenVatTuKhongDau = Commons.NonUnicode(data.TenVatTu);
                    uow.VatTus.Add(data);
                    foreach (var item in data.LstLoai)
                    {
                        item.CreatedBy = Guid.Parse(User.Identity.Name);
                        item.CreatedDate = DateTime.Now;
                        item.VatTu_Id = id;
                        uow.LoaiVatTus.Add(item);
                    }
                }
                uow.Complete();
                return Ok();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, VatTu data)
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
                data.TenVatTuKhongDau = Commons.NonUnicode(data.TenVatTu);
                uow.VatTus.Update(data);
                var lstLoai = data.LstLoai;
                var dataCheck = uow.LoaiVatTus.GetAll(x => !x.IsDeleted && x.VatTu_Id == id).ToList();
                if (dataCheck.Count() > 0)
                {
                    foreach (var item in dataCheck)
                    {
                        if (!lstLoai.Exists(x => x.Loai_Id == item.Loai_Id))
                        {
                            uow.LoaiVatTus.Delete(item.Id);
                        }
                    }
                    foreach (var item in lstLoai)
                    {
                        if (!dataCheck.Exists(x => x.Loai_Id == item.Loai_Id))
                        {
                            item.VatTu_Id = id;
                            item.CreatedDate = DateTime.Now;
                            item.CreatedBy = Guid.Parse(User.Identity.Name);
                            uow.LoaiVatTus.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in lstLoai)
                    {
                        item.VatTu_Id = id;
                        item.CreatedDate = DateTime.Now;
                        item.CreatedBy = Guid.Parse(User.Identity.Name);
                        uow.LoaiVatTus.Add(item);
                    }
                }
                uow.Complete();
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            lock (Commons.LockObjectState)
            {
                VatTu duLieu = uow.VatTus.GetById(id);
                if (duLieu.CreatedBy == Guid.Parse(User.Identity.Name) || Guid.Parse(User.Identity.Name) == Guid.Parse("c662783d-03c0-4404-9473-1034f1ac1caa"))
                {
                    if (duLieu == null)
                    {
                        return NotFound();
                    }
                    var dataCheck = uow.LoaiVatTus.GetAll(x => !x.IsDeleted && x.VatTu_Id == id).ToList();
                    foreach (var item in dataCheck)
                    {
                        uow.LoaiVatTus.Delete(item.Id);
                    }
                    duLieu.DeletedDate = DateTime.Now;
                    duLieu.DeletedBy = Guid.Parse(User.Identity.Name);
                    duLieu.IsDeleted = true;
                    uow.VatTus.Update(duLieu);
                    uow.Complete();
                    return Ok(duLieu);
                }
                return StatusCode(StatusCodes.Status409Conflict, "Bạn chỉ có thể chỉnh sửa thông tin thiết bị này");
            }
        }
        [HttpDelete("Remove/{id}")]
        public ActionResult Delete_Remove(Guid id)
        {
            lock (Commons.LockObjectState)
            {
                uow.VatTus.Delete(id);
                uow.Complete();
                return Ok();
            }
        }

        [HttpPost("KiemTraDuLieuImport")]
        public ActionResult KiemTraDuLieuImport(List<ClassVatTuImport> data)
        {
            var lstNhom = uow.Nhoms.GetAll(x => !x.IsDeleted);
            var lstDVT = uow.DonViTinhs.GetAll(x => !x.IsDeleted);
            var lstHang = uow.Hangs.GetAll(x => !x.IsDeleted);
            var lstLoai = uow.Loais.GetAll(x => !x.IsDeleted);
            var lstVatTu = uow.VatTus.GetAll(x => !x.IsDeleted);
            string[] include = { "Loai" };
            var lstLoaiVatTu = uow.LoaiVatTus.GetAll(x => !x.IsDeleted, null, include);
            foreach (var item in data)
            {
                item.ClassName = "new";
                var nhom = lstNhom.FirstOrDefault(x => x.MaNhom.ToLower() == item.MaNhom.ToLower());
                if (nhom == null)
                {
                    item.ClassName = "error";
                    item.GhiChuImport += "Mã nhóm chưa có trong danh mục ";
                }
                else item.Nhom_Id = nhom.Id;
                var hang = lstHang.FirstOrDefault(x => x.MaHang.ToLower() == item.MaHang.ToLower());
                if (hang == null)
                {
                    item.ClassName = "error";
                    item.GhiChuImport += "Hãng chưa có trong danh mục";
                }
                else item.Hang_Id = hang.Id;
                var dvt = lstDVT.FirstOrDefault(x => x.MaDonViTinh.ToLower() == item.MaDVT.ToLower());
                if (dvt == null)
                {
                    item.ClassName = "error";
                    item.GhiChuImport += "Đơn vị tính chưa có trong danh mục";
                }
                else item.DVT_Id = dvt.Id;
                string[] lstLoaiNhap = new string[] { "" };
                lstLoaiNhap = item.MaLoai.Split(", ");
                List<LoaiVatTu> LstLoai = new List<LoaiVatTu>();
                foreach (var item1 in lstLoaiNhap)
                {
                    var loai = lstLoai.FirstOrDefault(x => x.MaLoai == item1);
                    if (loai != null)
                    {
                        LstLoai.Add(new LoaiVatTu()
                        {
                            Loai_Id = loai.Id
                        });
                    }
                    else
                    {
                        item.ClassName = "error";
                        item.GhiChuImport += "Mã " + item.MaLoai + "chưa có trong danh mục";
                    }
                }
                item.LstLoai = LstLoai;
                var vattu = lstVatTu.FirstOrDefault(x => x.MaVatTu.ToLower() == item.MaVatTu.ToLower());
                if (vattu != null)
                {
                    item.ClassName = "edit";
                    item.GhiChuImport = "Mã vật tư đã tồn tại";
                    item.Id = vattu.Id;
                    item.CreateBy = vattu.CreatedBy;
                    item.CreateDate = vattu.CreatedDate;
                }
            }
            return Ok(data);
        }

        [HttpPost("ImportExel")]
        public ActionResult ImportExel(List<ClassVatTuImport> data)
        {
            foreach (var item in data)
            {
                VatTu vattu = new VatTu();
                vattu.TenVatTu = item.TenVatTu;
                vattu.MaVatTu = item.MaVatTu;
                vattu.Hang_Id = item.Hang_Id;
                vattu.Nhom_Id = item.Nhom_Id;
                vattu.DVT_Id = item.DVT_Id;
                vattu.ThongSoKyThuat = item.ThongSoKyThuat;
                vattu.TenVatTuKhongDau = Commons.NonUnicode(item.TenVatTu);
                var id = Guid.NewGuid();
                if (item.ClassName == "new")
                {
                    vattu.Id = id;
                    vattu.CreatedBy = Guid.Parse(User.Identity.Name);
                    vattu.CreatedDate = DateTime.Now;
                    uow.VatTus.Add(vattu);
                    foreach (var item1 in item.LstLoai)
                    {
                        item1.VatTu_Id = id;
                        item1.CreatedBy = Guid.Parse(User.Identity.Name);
                        item1.CreatedDate = DateTime.Now;
                        uow.LoaiVatTus.Add(item1);
                    }
                }
                else
                {
                    vattu.UpdatedBy = Guid.Parse(User.Identity.Name);
                    vattu.UpdatedDate = DateTime.Now;
                    uow.VatTus.Update(vattu);
                    var lstLoai = item.LstLoai;
                    var dataCheck = uow.LoaiVatTus.GetAll(x => !x.IsDeleted && x.VatTu_Id == item.Id).ToList();
                    if (dataCheck.Count() > 0)
                    {
                        foreach (var item1 in dataCheck)
                        {
                            if (!lstLoai.Exists(x => x.Loai_Id == item1.Loai_Id))
                            {
                                uow.LoaiVatTus.Delete(item.Id);
                            }
                        }
                        foreach (var item1 in lstLoai)
                        {
                            if (!dataCheck.Exists(x => x.Loai_Id == item1.Loai_Id))
                            {
                                item1.VatTu_Id = id;
                                item1.CreatedDate = DateTime.Now;
                                item1.CreatedBy = Guid.Parse(User.Identity.Name);
                                uow.LoaiVatTus.Add(item1);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in lstLoai)
                        {
                            item1.VatTu_Id = id;
                            item1.CreatedDate = DateTime.Now;
                            item1.CreatedBy = Guid.Parse(User.Identity.Name);
                            uow.LoaiVatTus.Add(item1);
                        }
                    }
                }
            }
            uow.Complete();
            return Ok();
        }
    }
}