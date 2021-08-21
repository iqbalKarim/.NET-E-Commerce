using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public AccountController(ITokenService tokenService, IConfiguration config)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDto.UserName,
            };

            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand comm = new SqlCommand("insert into AppUsers (UserName, passwordHash) values ('" + registerDto.UserName + "', HASHBYTES('SHA2_512','" + registerDto.Password + "'))", conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }



        [HttpPost("deleteAllUsers")]
        public void DeleteAll()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-GMOA06LL\SQLEXPRESS;Initial Catalog=ONE_ProjectDatabase;Trusted_Connection=True"))
            {
                string c = "Delete from AppUsers";
                SqlCommand comm = new SqlCommand(c, connection);
                connection.Open();
                comm.ExecuteNonQuery();
                connection.Close();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-GMOA06LL\SQLEXPRESS;Initial Catalog=ONE_ProjectDatabase;Trusted_Connection=True"))
            {
                // string c = "select * from AppUsers where username = '"+loginDto.UserName+"' and PasswordHash = HashBytes('SHA2_512', '"+loginDto.Password+"')";
                AppUser user = new AppUser(0, loginDto.UserName);
                await connection.OpenAsync();
                SqlCommand comm = new SqlCommand("select count(*) from AppUsers where username = '" + loginDto.UserName + "' and PasswordHash = HashBytes('SHA2_512', '" + loginDto.Password + "')", connection);
                int count = 0;
                count = (int)comm.ExecuteScalar();

                if (count == 0) return Unauthorized("Invalid User");
                await connection.CloseAsync();

                return new UserDto
                {
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
        }

        private async Task<bool> UserExists(string user)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GMOA06LL\SQLEXPRESS;Initial Catalog=ONE_ProjectDatabase;Trusted_Connection=True"))
            {
                string c = "select count(*) from AppUsers where Lower(username) = lower('" + user + "') ";
                SqlCommand comm = new SqlCommand(c, conn);
                await conn.OpenAsync();
                int count = 0;
                count = (int)comm.ExecuteScalar();
                await conn.CloseAsync();
                return (count > 0 ? true : false);
            }
        }
    }
}