using Microsoft.AspNetCore.Mvc;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Students>>> Search(string name, Gender? gender) 
        {
            try 
            {
                var result = await studentRepository.Search(name, gender);
                if (result.Any()) 
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            try
            {
                var result = await studentRepository.GetStudents(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet]
        public async Task<ActionResult<Students>> GetAllStudent()
        {
            try
            {
                return Ok(await studentRepository.GetAllStudents());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Students>> CreateStudent(Students students) 
        {
            try 
            {
                if (students == null) 
                {
                    return BadRequest();
                }
                var stu = await studentRepository.GetStudentByEmail(students.email);

                if (stu != null) 
                {
                    ModelState.AddModelError("Email", "Student email is already in use");
                    return BadRequest(ModelState);
                }
                var createdStudent = await studentRepository.AddStudent(students);
                return CreatedAtAction(nameof(GetStudent),
                    new { id = createdStudent.studentID }, createdStudent
                    );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new student record");
            }
        }
    }
}
