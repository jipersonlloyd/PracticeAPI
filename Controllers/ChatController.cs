using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using TestAPI.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public readonly IConfiguration configuration;

        public ChatController(IConfiguration configuration)
        {

            this.configuration = configuration;


        }


        [HttpGet]
        [Route("getChats")]
        public IActionResult getChatsByUsername(string username)
        {
            Dictionary<string, dynamic> result;
            try
            {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                string query = $"select * from tblChat WHERE SENDERID = '{username}' OR RECEIVERID = '{username}'";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ChatModel> chatLists = new List<ChatModel>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ChatModel chatModel = new ChatModel
                        {
                            message = Convert.ToString(dt.Rows[i]["MESSAGE"]),
                            datetime = Convert.ToString(dt.Rows[i]["DATETIME"]),
                            senderID = Convert.ToString(dt.Rows[i]["SENDERID"]),
                            receiverID = Convert.ToString(dt.Rows[i]["RECEIVERID"])
                        };
                        chatLists.Add(chatModel);
                    }
                }
                if (!chatLists.IsNullOrEmpty())
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", true},
                            {"Message", chatLists}
                        };
                    return Ok(result);
                }
                else
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error fetching usernames" }
                        };

                    return BadRequest(result);
                }
            }
            catch (Exception e) 
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error" }
                        };
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("insertChat")]
        public ActionResult insertChat([FromBody]ChatModel chatModel) 
        {
            Dictionary<string, dynamic> result;
            try
            {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                string query = $"INSERT INTO tblChat(MESSAGE, DATETIME, SENDERID, RECEIVERID) VALUES('{chatModel.message}', '{chatModel.datetime}', '{chatModel.senderID}', '{chatModel.receiverID}')";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Successfully Inserting Chat"},
                };


                return Ok(result);
            }
            catch (Exception e) 
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error inserting chat" }
                        };
                return BadRequest(result);
            }
        }
    }
}
