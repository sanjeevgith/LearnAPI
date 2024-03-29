﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.Json;



namespace LearnAPI.Controllers
{
    [Authorize]
    //to limiet request EnableRateLimiting
    //[EnableRateLimiting("fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService service;
        private readonly IWebHostEnvironment environment;
        private readonly IMemoryCache memoryCache;

        public CustomerController(ICustomerService service,IWebHostEnvironment environment, IMemoryCache cache)
        {
            this.service = service;
            this.environment = environment;
            this.memoryCache = cache;
        }

        // [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var key = "getAll";
            string cachedData;
            bool isCached = this.memoryCache.TryGetValue(key, out cachedData);
            if (!isCached)
            {
                var data = await this.service.GetAll();
                Thread.Sleep(3000);
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddSeconds(20), //perticular time k bad expire ho jayega
                    SlidingExpiration = TimeSpan.FromSeconds(30), // if 10 second tak koi request nahi mila to data erase ho jayega
                };
                var serializedData = JsonSerializer.Serialize(data); //list data directly store nai hota isliye jsonSerializer use kiya he
                this.memoryCache.Set(key, serializedData, options);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            else
            {
                var deserializedData = JsonSerializer.Deserialize<List<Customermodal>>(cachedData);
                return Ok(deserializedData);
            }
        }



        // [AllowAnonymous]
        [HttpPost("GetAllWithPagination")]
        public async Task<IActionResult> GetPagi(PageinationReq pageinationReq)
        {
            var data = await this.service.GetAllWithPagination();
            if (data == null)
            {
                return NotFound();
            }
            var paginatedItems = data
                               .Skip((pageinationReq.pageNumber - 1) * pageinationReq.pageSize)
                               .Take(pageinationReq.pageSize)
                               .ToList();
            return Ok(paginatedItems);
        }


        // [AllowAnonymous]
        [HttpGet("GetByCode")]
        public async Task<IActionResult> Getbycode(string code) 
        {
            var data = await this.service.GetByCode(code);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        // [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Customermodal _data)
        {
            var data = await this.service.Create(_data);
         
            return Ok(data);
        }


        //[AllowAnonymous]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Customermodal _data, string code)
        {
            var data = await this.service.Update(_data,code);

            return Ok(data);
        }

        //[AllowAnonymous]
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove( string code)
        {
            var data = await this.service.Remove( code);

            return Ok(data);
        }


        [AllowAnonymous]
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {

                //save file in project directory
                string Filepath = GetFilePath();
                string excelpath = Filepath + "\\customerinfo.xlsx";
                //end
                DataTable dt=new DataTable();
                dt.Columns.Add("Code",typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("CreditLimit", typeof(int));
                var data = await this.service.GetAll();
                if (data != null && data.Count > 0)
                {
                    data.ForEach(item =>
                    {
                        dt.Rows.Add(item.Code,item.Name,item.Email,item.Phone,item.Creditlimit);
                    });
                }
                    using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.AddWorksheet(dt, "Customer Info");
                    using(MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        //save file in project directory
                        if (System.IO.File.Exists(excelpath))
                        {
                            System.IO.File.Delete(excelpath);
                        }
                        wb.SaveAs(excelpath);
                        //end

                        return File(stream.ToArray(),"application/vnb.openxmlformats-officedocument.spreadsheetml.sheet","Customer.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex);

            }
        }






        [NonAction]
        private string GetFilePath()
        {
            return this.environment.WebRootPath + "\\Export";
        }





        [AllowAnonymous]
        [HttpGet("generatepdf")]

        public async Task<IActionResult> generatepdf(string code)
        {
            var data = await this.service.GetByCode(code);
            if (data == null)
            {
                return NotFound();
            }


            return Ok("");
        }


    




    }
}
