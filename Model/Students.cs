using System.ComponentModel.DataAnnotations;

namespace TestAPI.Model
{
    public class Students
    {
        [Key]
        public int studentID { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public Gender gender { get; set; }

        public int departmentID { get; set; }

    }
}
