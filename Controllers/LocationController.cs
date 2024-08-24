using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public readonly IConfiguration configuration;

        public LocationController(IConfiguration configuration)
        {

            this.configuration = configuration;


        }

        [HttpGet]
        [Route("getLocation")]
        public ActionResult getLocation() 
        {
            Dictionary<string, dynamic> result;
            try 
            {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                String query = "SELECT * FROM tblLocation where id = (SELECT MAX(id) from tblLocation)";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) 
                {
                    LocationModel locationModel = new LocationModel();
                    locationModel.lat = Convert.ToString(dt.Rows[0]["lat"]);
                    locationModel.lng = Convert.ToString(dt.Rows[0]["long"]);

                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", true},
                            {"Message", locationModel}
                        };
                    return Ok(result);
                }
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error fetching location" }
                        };

                return BadRequest(result);

            }
            catch (Exception ex) 
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", ex }
                        };
                return BadRequest(result);
            }
        }
    }
}
