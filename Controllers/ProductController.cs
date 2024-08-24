using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using TestAPI.Model;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        public readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {

            this.configuration = configuration;

        }

        List<ProductsModel> getProductsList()
        {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                string query = "SELECT * FROM tblProducts";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ProductsModel> productList = new List<ProductsModel>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductsModel productModel = new ProductsModel();
                        productModel.Id = Convert.ToInt32(dt.Rows[i]["id"]);
                        productModel.productName = Convert.ToString(dt.Rows[i]["Product_Name"]);
                        productModel.price = Convert.ToDouble(dt.Rows[i]["Price"]);
                        productModel.sold = Convert.ToInt32(dt.Rows[i]["Sold"]);
                        productModel.stocks = Convert.ToInt32(dt.Rows[i]["Stocks"]);
                    productModel.imageString = Convert.ToString(dt.Rows[i]["ImageString"]);
                        productList.Add(productModel);
                    }
                }
                return productList;
        }

        [HttpGet]
        [Route("getProducts")]
        public ActionResult GetProducts()
        {
            try 
            {
                List<ProductsModel> productList = getProductsList();
                if (productList.Count > 0)
                {
                    return Ok(JsonConvert.SerializeObject(productList));
                }
                else
                {
                    return BadRequest("Error getting products from Database");

                }
            }
            catch (Exception e) 
            {
                return BadRequest($"Error: {e}");
            }
        }

        [HttpGet("GetData")]
        public ActionResult GetData([FromQuery(Name = "callback")] string callback = null)
        {
            try
            {
                List<ProductsModel> productList = getProductsList();
                if (productList.Count > 0)
                {
                    return Ok($"{callback}({JsonConvert.SerializeObject(productList)})");
                }
                else
                {
                    return BadRequest("Error getting products from Database");

                }
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e}");
            }
        }

        [HttpPost("uploadProduct")]
        public ActionResult uploadProduct([FromBody]InsertProductModel insertProduct) 
        {
            Dictionary<string, dynamic> result;
            try 
            {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                String query = $"INSERT INTO tblProducts(Product_Name, Price, Sold, Stocks, ImageString) VALUES('{insertProduct.productName}', '{insertProduct.price}', '{insertProduct.sold}', '{insertProduct.stocks}', '{insertProduct.imageString}')";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Successfully Uploading Product"},
                };

                return Ok(result);
            }
            catch(Exception e)
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", $"{e}" }
                        };
                return BadRequest(result);

            }
        }
    }
}
