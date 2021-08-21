using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class AppUserController : BaseApiController
    {
        private readonly IConfiguration _config;
        public AppUserController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public List<AppUser> GetUsers()
        {
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlDataAdapter sqlAda = new SqlDataAdapter("Select * from AppUsers", connection);
                DataTable data = new DataTable();
                sqlAda.Fill(data);

                List<AppUser> list = new List<AppUser>();
                list = (from DataRow dr in data.Rows
                        select new AppUser(Convert.ToInt32(dr["Id"]), dr["UserName"].ToString())
                        {
                            PasswordHash = Encoding.UTF8.GetBytes(dr["PasswordHash"].ToString())
                        }
                        ).ToList();
                return list;
            }
        }
    }
}