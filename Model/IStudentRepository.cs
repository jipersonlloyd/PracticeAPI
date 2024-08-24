namespace TestAPI.Model
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Students>> Search(string name, Gender? gender);
        Task<Students> GetStudents(int id);
        Task<IEnumerable<Students>> GetAllStudents();
        Task<Students> GetStudentByEmail(string email);
        Task<Students> AddStudent(Students students);
        Task<Students> UpdateStudents(Students students);
        Task DeleteStudents(int studentID);

    }
}
