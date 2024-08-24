using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Web.Http.Results;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {
        public readonly IConfiguration configuration;
        public CreateAccountController(IConfiguration configuration)
        {

            this.configuration = configuration;

        }

        List<CreateAccountModel> getAccounts()
        {
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
            String query = "SELECT * FROM tblAccount";
            SqlDataAdapter da = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<CreateAccountModel> accountList = new List<CreateAccountModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CreateAccountModel createAccountModel = new CreateAccountModel();
                    createAccountModel.userName = Convert.ToString(dt.Rows[i]["username"]);
                    createAccountModel.password = Convert.ToString(dt.Rows[i]["password"]);
                    createAccountModel.email = Convert.ToString(dt.Rows[i]["email"]);
                    createAccountModel.phoneNumber = Convert.ToString(dt.Rows[i]["phoneNumber"]);
                    createAccountModel.dateCreated = Convert.ToString(dt.Rows[i]["dateCreated"]);
                    createAccountModel.token = Convert.ToString(dt.Rows[i]["token"]);

                    accountList.Add(createAccountModel);
                }
            }
            return accountList;
        }

        [HttpGet]
        [Route("usernames")]
        public ActionResult getUsernames()
        {
            Dictionary<string, dynamic> result;
            try
            {

                List<CreateAccountModel> accountList = getAccounts();
                List<string> userNames = new List<string>();
                foreach (var item in accountList)
                {
                    userNames.Add(item.userName);
                }
                if (!userNames.IsNullOrEmpty())
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", true},
                            {"Message", userNames}
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
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("data")]
        public ActionResult getUserData(String username)
        {
            Dictionary<string, dynamic> result;
            try
            {
                List<CreateAccountModel> accountList = getAccounts();
                CreateAccountModel accountModel = accountList.FirstOrDefault((e) => e.userName == username);
                CreateAccountModel newModel = new CreateAccountModel
                {
                    userName = accountModel.userName,
                    email = accountModel.email,
                    phoneNumber = accountModel.phoneNumber,
                    dateCreated = accountModel.dateCreated,
                };
                if (accountModel != null)
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", true},
                            {"Message", newModel}
                        };
                    return Ok(result);
                }
                else
                {
                    result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error fetching account" }
                        };
                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error fetching data" }
                        };
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("createAccount")]
        public ActionResult createAccount([FromBody]AccountModel createAccountModel)
        {
            Dictionary<string, dynamic> result;
            try
            {

                List<CreateAccountModel> accountList = getAccounts();

                foreach (var item in accountList)
                {
                    if (item.userName == createAccountModel.userName)
                    {
                        result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Username already exist" }
                        };
                        return BadRequest(result);
                    }
                }

                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                String query = $"INSERT INTO tblAccount(username, password, email, phoneNumber, dateCreated) VALUES('{createAccountModel.userName}','{createAccountModel.password}','{createAccountModel.email}', '{createAccountModel.phoneNumber}', GETUTCDATE())";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Successfully Creating Account"},
                };


                return Ok(result);
            }
            catch (Exception e)
            {

                result = new Dictionary<string, dynamic>
                        {
                            {"Result", false},
                            {"Message", "Error in creating account" }
                        };
                return BadRequest("Error in creating Account");
            }
        }

        [HttpPost]
        [Route("loginAccount")]
        public ActionResult checkAccount([FromBody] LoginAccountModel loginAccount)
        {
            Dictionary<string, dynamic> result;
            List<CreateAccountModel> accountList = getAccounts();
            bool isFound = false;

            foreach (var item in accountList)
            {
                if (item.userName == loginAccount.userName && item.password == loginAccount.password)
                {
                    isFound = true;
                    break;
                }
                else
                {
                    isFound = false;
                }
            }
            if (isFound)
            {
                result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Success" }
                };
            }
            else
            {
                result = new Dictionary<string, dynamic>
                {
                    {"Result", false},
                    {"Message", "Incorrect username and password" }
                };
            }
            return isFound ? Ok(result) : BadRequest(result);
        }

        [HttpOptions]
        [Route("loginAccountOption")]
        public ActionResult checkAccountOptionRequest([FromBody] LoginAccountModel loginAccount)
        {
            Dictionary<string, dynamic> result;
            List<CreateAccountModel> accountList = getAccounts();
            bool isFound = false;

            foreach (var item in accountList)
            {
                if (item.userName == loginAccount.userName && item.password == loginAccount.password)
                {
                    isFound = true;
                    break;
                }
                else
                {
                    isFound = false;
                }
            }
            if (isFound)
            {
                result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Success" }
                };
            }
            else
            {
                result = new Dictionary<string, dynamic>
                {
                    {"Result", false},
                    {"Message", "Incorrect username and password" }
                };
            }
            return isFound ? Ok(result) : BadRequest(result);
        }

        [HttpPatch]
        [Route("updateToken")]
        public ActionResult updateToken([FromBody]UpdateTokenModel updateTokenModel) 
        {
            Dictionary<string, dynamic> result;
            try 
            {
                List<CreateAccountModel> accounts = getAccounts();
                foreach (var item in accounts) 
                {
                    if (item.userName == updateTokenModel.username) 
                    {
                        SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                        String query = $"UPDATE tblAccount SET token = '{updateTokenModel.token}' WHERE username = '{updateTokenModel.username}'";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", "Success" }
                };

                        return Ok(result);
                    }
                }

                result = new Dictionary<string, dynamic>
                {
                    {"Result", false},
                    {"Message", "Account doesn't exist" }
                };
                return BadRequest(result);


            }
            catch (Exception e) 
            {
                result = new Dictionary<string, dynamic>
                {
                    {"Result", false},
                    {"Message", "Error" }
                };
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("getToken")]
        public ActionResult getToken(String receiverID) 
        {
            Dictionary<string, dynamic> result;
            string? token;
            try
            {
                SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DbConnection").ToString());
                string query = $"SELECT token FROM tblAccount WHERE username = '{receiverID}'";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    token = reader.GetString(0);
                    if (token != null)
                    {
                        result = new Dictionary<string, dynamic>
                {
                    {"Result", true},
                    {"Message", token!}
                };
                        return Ok(result);
                    }
                }
                result = new Dictionary<string, dynamic>
                {
                    {"Result", false},
                    {"Message", "Token is null" }
                };
                return BadRequest(result);

            }
            catch (Exception e) 
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
