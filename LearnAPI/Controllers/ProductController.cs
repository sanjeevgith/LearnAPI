using LearnAPI.Helper;
using LearnAPI.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly LearndataContext context;

        public ProductController(IWebHostEnvironment environment,LearndataContext context)
        {
            this.environment = environment;
            this.context = context;
        }


        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile, string productcode)
        {
            APIResponse response = new APIResponse();
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                string imagepath = Filepath+"\\" + productcode +".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO .File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return Ok(response);
        }





        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImage(IFormFileCollection filecollection, string productcode)
        {
            APIResponse response = new APIResponse();
            int passcount = 0;int errorcount = 0;
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach(var file in filecollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.ErrorMessage = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files Uploaded " + errorcount + " Files Failed";
            return Ok(response);
        }



        [HttpPut("DbMultiUploadImage")]
        public async Task<IActionResult> DbMultiUploadImage(IFormFileCollection filecollection, string productcode)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                foreach (var file in filecollection)
                {

                    using(MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        this.context.TblProductimages.Add(new Repos.Models.TblProductimage() { 
                            Productcode = productcode,
                            Productimage = stream.ToArray()
                        });
                        await this.context.SaveChangesAsync();
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.ErrorMessage = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files Uploaded " + errorcount + " Files Failed";
            return Ok(response);
        }



        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string productcode)
        {
            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = hosturl +"/Upload/Product/"+productcode+"/"+productcode+".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {

            }
            var response = new { ImageUrl =  Imageurl };
            return Ok(response);
        }


        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string productcode)
        {
            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo DirectoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = DirectoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/Upload/Product/" + productcode + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            var response = new { ImageUrl = Imageurl };
            return Ok(response);
        }

        [HttpGet("DbGetMultiImage")]
        public async Task<IActionResult> DbGetMultiImage(string productcode)
        { 
            List<string> Imageurl = new List<string>();
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                var _productimage = this.context.TblProductimages.Where(item=>item.Productcode== productcode).ToList();
                if(_productimage.Count>0 && _productimage != null)
                {
                    _productimage.ForEach(item =>
                    {
                        Imageurl.Add(Convert.ToBase64String(item.Productimage));
                    });
                }
                else 
                {
                    return NotFound();
                }
              /*  string Filepath = GetFilePath(productcode);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo DirectoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = DirectoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/Upload/Product/" + productcode + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {

            }
            return Ok(Imageurl);
        }

        [HttpGet("DownloadImg")]
        public async Task<IActionResult> Download(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using(FileStream fileStream = new FileStream(imagepath,FileMode.Open))
                    {
                        await fileStream.CopyToAsync(memoryStream);
                    }
                    memoryStream.Position = 0;
                    return File(memoryStream, "image/png", productcode + ".png");
                    //Imageurl = hosturl + "/Upload/Product/" + productcode + "/" + productcode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }



        [HttpGet("DbDownloadImg")]
        public async Task<IActionResult> DbDownload(string productcode)
        {
            try
            {
                var _productimage =await this.context.TblProductimages.FirstOrDefaultAsync(item => item.Productcode == productcode);
                if (_productimage != null)
                {
                    return File(_productimage.Productimage, "image/png", productcode + ".pmg");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }


        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                   System.IO.File.Delete(imagepath);
                    return Ok("File Deleted");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {   
                return NotFound(ex);
            }
        }


        [HttpDelete("MultiRemove")]
        public async Task<IActionResult> MultiRemove(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo DirectoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = DirectoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("File Deleted");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }


        [NonAction]
        private string GetFilePath(string  productcode)
        {
            return this.environment.WebRootPath + "\\Upload\\Product\\" + productcode;
        }


    }
}
