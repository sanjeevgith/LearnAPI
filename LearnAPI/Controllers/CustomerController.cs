using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearnAPI.Controllers
{
    [Authorize]
    [EnableRateLimiting("fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService service;
        public CustomerController(ICustomerService service)
        {
            this.service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var data = await this.service.GetAll();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }


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


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Customermodal _data)
        {
            var data = await this.service.Create(_data);
         
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Customermodal _data, string code)
        {
            var data = await this.service.Update(_data,code);

            return Ok(data);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove( string code)
        {
            var data = await this.service.Remove( code);

            return Ok(data);
        }
    }
}
