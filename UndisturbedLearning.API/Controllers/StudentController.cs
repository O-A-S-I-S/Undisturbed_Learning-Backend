using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using UndisturbedLearning.DataAccess;
using UndisturbedLearning.Entities;
using UndisturbedLearning.Dto.Request;
using UndisturbedLearning.Dto.Response;

namespace UndisturbedLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController: ControllerBase
{
    private readonly UndisturbedLearningDbContext _context;

    public StudentController(UndisturbedLearningDbContext context)
    {
        _context = context;
    }

    public class NamePart
    {
        public string Surname { get; set; }
        public string LastName { get; set; }
    }
    
    private static DtoStudentResponse StudentToResponse(Student student) => new DtoStudentResponse
        {
            Id = student.Id,
            Code = student.Code,
            Dni = student.Dni,
            Surname = student.Surname,
            LastName = student.LastName,
            BirthDate = student.BirthDate,
            Email = student.Email,
            Cellphone = student.Cellphone,
            Telephone = student.Telephone,
            Undergraduate = student.Undergraduate,
        };

    private static DtoLogInResponse StudentToLogInResponse(Student student) => new DtoLogInResponse
    {

        Id = student.Id,
        Code = student.Code,
        Surname = student.Surname,
        LastName = student.LastName,
        Email = student.Email,
    };

    private static bool MatchName(string name, string part)
    {
        return Regex.IsMatch(name, part);
    } 

    [HttpGet]
    public async Task<ActionResult<ICollection<Student>>> Get()
    {
        ICollection<DtoStudentResponse> response = await _context.Students.Select(s => StudentToResponse(s)).ToListAsync();

        return Ok(response);
    }
    
    [HttpGet("id/{id:int}")]
    public async Task<ActionResult<string>> GetById(int id)
    {
        var student = await _context.Students.Where(s => s.Id == id).FirstAsync();

        if (student == null) return NotFound("There is no student with such id");

        return Ok(StudentToResponse(student));
    }
    
    [HttpGet("code/{code}")]
    public async Task<ActionResult<string>> GetByCode(string code)
    {
        var student = await _context.Students.Where(s => s.Code == code).FirstAsync();

        if (student == null) return NotFound("There is no student with such code");

        return Ok(StudentToResponse(student));
    }
    
    [HttpPost("name")]
    public async Task<ActionResult<string>> GetByName(NamePart part)
    {
        
        var students = await _context.Students.Where(s => s.Surname.Contains(part.Surname))
            .Where(s => s.LastName.Contains(part.LastName)).Select(s => StudentToResponse(s)).ToListAsync();

        if (students.Count() == 0) return NotFound("No matches for the student name provided.");

        return Ok(students);
    }

    [HttpGet("access/{code}")]
    public async Task<ActionResult<string>> AccessUsername(string code)
    {
        var student = await _context.Students.Where(s => s.Code == code).FirstAsync();

        if (student == null) return NotFound("Invalid student code");

        return Ok(
        new
        {
            Code = code
        });
    }
    
    [HttpPost("access")]
    public async Task<ActionResult<Student>> LogIn(DtoLogIn credentials)
    {
        var student = await _context.Students.Where(s => s.Code == credentials.Username)
            .Where(s => s.Password == credentials.Password).FirstAsync();

        if (student == null) return BadRequest("Incorrect password");

        return Ok(StudentToLogInResponse(student));
    }
    
    [HttpPut("register")]
    public async Task<ActionResult<Student>> SignIn(DtoSignIn request)
    {
        var student = await _context.Students.Where(s => s.Code == request.Username).FirstAsync();

        if (student == null) return NotFound("Student does not exist");

        if (student.Password != "") return BadRequest("A password has been already set");

        student.Password = request.Password;

        _context.Entry(student).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(StudentToLogInResponse(student));
    }

    [HttpPost]
    public async Task<ActionResult> Post(List<DtoStudent> requests)
    {
        foreach (DtoStudent request in requests)
        {
            var career = await _context.Careers.Where(c => c.Name == request.Career).FirstAsync();
            if (career == null) return BadRequest("Invalid career name");
        
            var campus = await _context.Campuses.Where(c => c.Location == request.Campus).FirstAsync();
            if (campus == null) return BadRequest("Invalid campus name");

            var student = await _context.Students.Where(s => s.Code == request.Code).FirstOrDefaultAsync();

            if (student != null) return BadRequest("Student already exists");
        
            var entity = new Student
            {
                Code = request.Code,
                Password = "",
                Dni = request.Dni,
                Surname = request.Surname,
                LastName = request.LastName,
                BirthDate = request.BirthDate.Date,
                Email = request.Email,
                Cellphone = request.Cellphone,
                Telephone = request.Telephone,
                Undergraduate = request.Undergraduate,
                CareerId = career.Id,
                CampusId = campus.Id
            };

            _context.Students.Add(entity);
            await _context.SaveChangesAsync();
        
            // HttpContext.Response.Headers.Add("location{entity.Id}", $"/api/student/{entity.Id}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null) return NotFound("Student does not exist");

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}