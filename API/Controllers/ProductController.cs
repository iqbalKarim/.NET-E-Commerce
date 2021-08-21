using System;
using System.Collections.Generic;
using System.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IConfiguration _config;
        public ProductController(IConfiguration config)
        {
            _config = config;

        }

        [HttpGet("allProducts")]
        public List<Product> GetProducts()
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlDataAdapter sqlAda = new SqlDataAdapter("select * from Products", connection);
                DataTable data = new DataTable();
                sqlAda.Fill(data);

                List<Product> products = new List<Product>();
                products = (from DataRow row in data.Rows
                            select new Product(Convert.ToInt32(row["Id"]), row["ProductName"].ToString(), row["CategoryName"].ToString(), Convert.ToDouble(row["Price"]))
                            {
                                Image = row["Img"].ToString()
                            }).ToList<Product>();
                return products;
            }
        }

        [HttpGet("{category}")]
        public List<Product> GetProductsFromCategory(string Category)
        {
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlDataAdapter sqlAda = new SqlDataAdapter("select * from products where CategoryName = '" + Category + "'", connection);
                DataTable data = new DataTable();
                sqlAda.Fill(data);

                List<Product> products = new List<Product>();
                products = (from DataRow row in data.Rows
                            select new Product(Convert.ToInt32(row["Id"]), row["ProductName"].ToString(), row["CategoryName"].ToString(), Convert.ToDouble(row["Price"]))
                            {
                                Image = row["Img"].ToString()
                            }).ToList<Product>();
                return products;
            }
        }

    }
}

