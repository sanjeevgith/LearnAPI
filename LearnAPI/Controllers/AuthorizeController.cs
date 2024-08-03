using AutoMapper;
using LearnAPI.Container;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly LearndataContext context;
        private readonly IRefreshHandler refresh;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerService> logger;

        public AuthorizeController(LearndataContext context , IOptions<JwtSettings> options, IRefreshHandler refresh, IMapper mapper, ILogger<CustomerService> logger)
        {
            this.context = context;
            this.refresh = refresh;
            this.jwtSettings = options.Value;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("GenerateToken")]

        public async Task<IActionResult> GenerateToken([FromBody] UserCred userCred)
        {
            var user = await this.context.TblUsers.FirstOrDefaultAsync(item => item.Code == userCred.username && item.Password == userCred.password);
            if (user != null)
            {
                //generate token
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
                var tokendesc = new SecurityTokenDescriptor
                {
                    Subject=new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.Code),
                        new Claim(ClaimTypes.Role,user.Role)
                    }),
                    //Expires = DateTime.UtcNow.AddMinutes(10),
                    Expires = DateTime.UtcNow.AddSeconds(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey),SecurityAlgorithms.HmacSha256)
                };
                var token = tokenhandler.CreateToken(tokendesc);
                var finaltoken = tokenhandler.WriteToken(token);
                return Ok(new TokenResponse() { Token = finaltoken, RefreshToken= await this.refresh.GenerateToken(userCred.username) });
            }
            else
            {
                return Unauthorized();
            }
             
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUser createUser )
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create begin");
                TblUser _customer = this.mapper.Map<CreateUser, TblUser>(createUser);
                await this.context.TblUsers.AddAsync(_customer);
                await this.context.SaveChangesAsync();
                response.ResponseCode = 201;
                response.Result = createUser.Code;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
                this.logger.LogError(ex.Message, ex);
            }
            return Ok(response);
        }







        [HttpPost("GenerateRefreshToken")]

        public async Task<IActionResult> GenerateRefreshToken([FromBody] TokenResponse token)
        {
            var _refreshtoken = await this.context.TblRefreshtokens.FirstOrDefaultAsync(item => item.Refreshtoken == token.RefreshToken);
            if (_refreshtoken != null)
            {
                //generate token
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
                SecurityToken securityToken;
                var pricipal = tokenhandler.ValidateToken(token.Token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out securityToken);

                var _token = securityToken as JwtSecurityToken;
                if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    string username = pricipal.Identity?.Name;
                    var _existdata =await this.context.TblRefreshtokens.FirstOrDefaultAsync(item => item.Userid == username
                   && item.Refreshtoken == token.RefreshToken);
                    if (_existdata != null)
                    {
                        var _newtoken = new JwtSecurityToken(
                            claims: pricipal.Claims.ToArray(),
                            expires: DateTime.Now.AddMinutes(5),
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.securitykey)),
                            SecurityAlgorithms.HmacSha256)
                            );

                        var _finaltoken = tokenhandler.WriteToken(_newtoken);
                        return Ok(new TokenResponse() { Token = _finaltoken, RefreshToken = await this.refresh.GenerateToken(username) });

                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }   
            }
            else
            {
                
                return Unauthorized();
            }

        }
    }
}
