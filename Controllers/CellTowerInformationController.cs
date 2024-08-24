using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/cellTowerData")]
    [ApiController]
    public class CellTowerInformationController : ControllerBase
    {
        public readonly IConfiguration configuration;
        public CellTowerInformationController(IConfiguration configuration)
        {

            this.configuration = configuration;

        }


        [HttpPost]
        [Route("data")]
        public async Task<IActionResult> insertData([FromQuery]CellTowerInformationModel cell) 
        {
            Dictionary<string, dynamic> result;
            try 
            {
                int convertedCellID = Convert.ToInt32(cell.cellID, 16);
                int convertedLac = Convert.ToInt32(cell.lac, 16);
                bool isExist = await Test.checkIfTowerisExist(convertedCellID.ToString(), convertedLac.ToString(), cell.mcc, cell.mnc);
                if (isExist)
                {
                    SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                    String query = $"INSERT INTO tblCellTowerInformation(cellID, lac, convertedCellID, convertedLac, MCC, MNC) VALUES('{cell.cellID}','{cell.lac}','{convertedCellID.ToString()}', '{convertedLac.ToString()}', '{cell.mcc}', '{cell.mnc}')";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Successfully inserted data"},
                };
                    return Ok(result);
                }
                else 
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Cell not found" }
                        };
                    return BadRequest(result);
                }
                
            }
            catch 
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error inserting data" }
                        };
                return BadRequest(result);
            }
        }
    }

    public class Test 
    {
        public static async Task<bool> checkIfTowerisExist(string cellID, string lac, string? mcc, string? mnc)
        {
            string apiKey = "pk.df82dc435b61dbce0ff9b4b9acb390b7";

            string apiUrl = $"https://opencellid.org/cell/get?key={apiKey}&mcc={mcc}&mnc={mnc}&lac={lac}&cellid={cellID}&format=json";
            Console.WriteLine($"apiURL: {apiUrl}");
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                        if (responseBody.Contains("not found"))
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
