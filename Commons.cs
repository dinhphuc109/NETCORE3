using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NETCORE3
{
  public class Commons
  {
    public static string UploadBase64(string webRootPath, string File_Base64, string File_Name)
    {
      //Xử lý file base 64 lưu trữ
      var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
      DateTime dt = DateTime.Now;
      string fileName = (long)timeSpan.TotalSeconds + "_" + TiengVietKhongDau(File_Name);
      byte[] fileBytes = Convert.FromBase64String(File_Base64.Split(',')[1]);
      var buffer = Convert.FromBase64String(Convert.ToBase64String(fileBytes));
      // Convert byte[] to file type
      string path = "Uploads/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
      if (string.IsNullOrWhiteSpace(webRootPath))
      {
        webRootPath = Path.Combine(Directory.GetCurrentDirectory(), path);
      }
      if (!Directory.Exists(webRootPath))
      {
        Directory.CreateDirectory(webRootPath);
      }
      string fullPath = Path.Combine(webRootPath, fileName);
      System.IO.FileStream f = System.IO.File.Create(fullPath);
      f.Close();
      System.IO.File.WriteAllBytes(fullPath, buffer);
      return path + "/" + fileName;
    }
    public static string Upload(string webRootPath, IFormFile file)
    {
      var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
      DateTime dt = DateTime.Now;
      // Rename file
      string fileName = (long)timeSpan.TotalSeconds + "_" + Commons.TiengVietKhongDau(file.FileName);
      string fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
      string path = "Uploads/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
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
      return path + "/" + fileName;
    }

    public static string NonUnicode(string text)
    {
      string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
      string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
      for (int i = 0; i < arr1.Length; i++)
      {
        text = text.Replace(arr1[i], arr2[i]);
        text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
      }
      return text;
    }

    public static object LockObjectState = new object();
    public static string TiengVietKhongDau(string s)
    {
      Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
      string temp = s.Normalize(NormalizationForm.FormD);
      return regex.Replace(temp, string.Empty).Replace("đ", "d").Replace("Đ", "D").Replace(' ', '_').ToLower();
    }
    public static float ConvertFloat(string number)
    {
      float num = 0;
      float.TryParse(number, out num);
      return num;
    }
    public static float TinhTrungBinh(params float[] array)
    {
      return array.Average();
    }
    public static string ConvertObjectToJson(object ob)
    {
      return JsonConvert.SerializeObject(ob);
    }
    public static Image HinhAnhUrl(string url)
    {
      var base_url = "http://demo1api.thacoindustries.vn/" + url;
      WebClient wc = new WebClient();
      byte[] bytes = wc.DownloadData(base_url);
      MemoryStream ms = new MemoryStream(bytes);
      System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
      return img;
    }
  }
}
