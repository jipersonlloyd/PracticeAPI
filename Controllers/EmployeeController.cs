using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly IConfiguration configuration;

        public EmployeeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("employeeData")]
        public ActionResult GetEmployeeData()
        {
            Dictionary<string, dynamic> result;
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
            String query = "SELECT * FROM tblEmployee where id = (SELECT MAX(id) FROM tblEmployee)";
            SqlDataAdapter da = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0) 
            {
                Employee employee = new Employee
                {
                    name = Convert.ToString(dt.Rows[0]["name"]),
                    age = Convert.ToString(dt.Rows[0]["age"]),
                    position = Convert.ToString(dt.Rows[0]["position"]),
                    phoneNumber = Convert.ToString(dt.Rows[0]["phonenumber"]),

                };
                result = new Dictionary<string, dynamic> 
                {
                    {
                        "result", true
                    },
                    {
                        "Message", new Dictionary<string, dynamic> 
                        {
                            {
                        "name", employee.name
                    },
                    {
                        "age", employee.age
                    },
                    {
                        "position", employee.position
                    },
                    {
                        "phonenumber", employee.phoneNumber
                    }
                        }
                    }
                };
                return Ok(result);
            }
            else 
            {
                return BadRequest("No Data Found");
            }
        }
    }
}
