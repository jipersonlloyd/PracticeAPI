using Microsoft.EntityFrameworkCore;

namespace TestAPI.Model
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext appDbContext;

        public StudentRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Students> AddStudent(Students students) 
        {
            var result = await appDbContext.students.AddAsync(students);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteStudents(int studentID) 
        {
            var result = await appDbContext.students.FirstOrDefaultAsync(e => e.studentID == studentID);
            if (result != null) 
            {
                appDbContext.students.Remove(result);
                await appDbContext.SaveChangesAsync(); 
            }
        }

        public async Task<Students> GetStudents(int studentID) 
        {
            return await appDbContext.students.FirstOrDefaultAsync(e => e.studentID == studentID);
        }

        public async Task<Students> GetStudentByEmail(string email) 
        {
            return await appDbContext.students.FirstOrDefaultAsync(e => e.email == email);
        }

        public async Task<IEnumerable<Students>> GetAllStudents() 
        {
            return await appDbContext.students.ToListAsync();
        }

        public async Task<IEnumerable<Students>> Search(string name, Gender? gender) 
        {
            IQueryable<Students> query = appDbContext.students;

            if (!string.IsNullOrEmpty(name)) 
            {
                query = query.Where(e => e.firstName.Contains(name) || e.lastName.Contains(name));
            }

            if (gender != null) 
            {
                query = query.Where(e => e.gender == gender);
            }
            return await query.ToListAsync();
        }

        public async Task<Students> UpdateStudents(Students students) 
        {
            var result = await appDbContext.students.FirstOrDefaultAsync(e => e.studentID == students.studentID);

            if (result != null) 
            {
                result.firstName = students.firstName;
                result.lastName = students.lastName;
                result.email = students.email;
                result.gender = students.gender;
                if (students.departmentID != 0) 
                {
                    result.departmentID = students.departmentID;
                }
            }
            return result!;
        }
    }
}
