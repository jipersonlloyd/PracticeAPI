namespace TestAPI.Model
{
    public class CreateAccountModel
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string dateCreated { get; set; }
        public string token { get; set; }
    }
}
